using SkyWeb.DatVM.Mvc;
using SportsSocialNetwork.Models.Entities;
using SportsSocialNetwork.Models.Entities.Services;
using SportsSocialNetwork.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using SportsSocialNetwork.Areas.PlaceOwner.Models.ViewModels;
using System.Threading.Tasks;
using NganLuong;
using SportsSocialNetwork.Models.Enumerable;
using SportsSocialNetwork.Models.Identity;
using System.Globalization;

namespace SportsSocialNetwork.Controllers
{
    [MyAuthorize(Roles = IdentityMultipleRoles.SSN)]
    public class OrderController : BaseController
    {

        // GET: Order
        public ActionResult Index()
        {
            return View();
        }

        public string CancelOrder(int id)
        {
            var _orderSeervice = this.Service<IOrderService>();
            Order order = _orderSeervice.FirstOrDefault(o => o.Id == id);
            if (order != null)
            {
                order.Status = (int)OrderStatus.Cancel;
                _orderSeervice.Update(order);
                _orderSeervice.Save();
                return "success";
            }
            return "false";
        }

        public ActionResult OrderDetail(int id)
        {
            var _placeService = this.Service<IPlaceService>();
            var _fieldService = this.Service<IFieldService>();
            var _orderService = this.Service<IOrderService>();
            Order order = _orderService.FirstOrDefault(o => o.Id == id);
            var fieldId = order.FieldId;
            var field = _fieldService.FirstOrDefaultActive(p => p.Id == fieldId);
            Place place = new Place();
            if (field != null)
            {
                place = _placeService.FirstOrDefaultActive(p => p.Id == field.PlaceId);
                ViewBag.field = field;
            }
            if (place != null)
            {
                ViewBag.place = place;
            }
            OrderViewModel model = new OrderViewModel(order);
            return this.PartialView(model);
        }

        public ActionResult GetData(JQueryDataTableParamModel param)
        {
            //var blogPostList = _blogPostService.GetBlogPostbyStoreId();
            var _orderService = this.Service<IOrderService>();
            var userId = User.Identity.GetUserId();
            var orderList = _orderService.GetActive(p => p.UserId == userId).OrderByDescending(p => p.CreateDate);
            //IEnumerable<BlogPost> filteredListItems;
            IEnumerable<Order> filteredListItems;
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                filteredListItems = orderList.Where(
                    o => (o.Field.Name != null && o.Field.Name.ToLower().Contains(param.sSearch.ToLower()))
                    || (o.OrderCode != null && o.OrderCode.ToLower().Contains(param.sSearch.ToLower()))
                    );
                
            }
            else
            {
                //filteredListItems = blogPostList;
                filteredListItems = orderList;
            }
            // Sort.
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            var sortDirection = Request["sSortDir_0"]; // asc or desc

            switch (sortColumnIndex)
            {
                case 2:
                    filteredListItems = sortDirection == "asc"
                        ? filteredListItems.OrderBy(o => o.Field.Name)
                        : filteredListItems.OrderByDescending(o => o.Field.Name);
                    break;
            }

            var displayedList = filteredListItems.Skip(param.iDisplayStart).Take(param.iDisplayLength);
            var result = displayedList.Select(o => new IConvertible[]{
                o.Id,
                o.OrderCode,
                o.Field.Name,
                o.CreateDate.ToString("dd/MM/yyyy HH:mm"),
                o.StartTime.ToString("HH:mm"),
                o.StartTime.ToString("dd/MM/yyyy"),
                o.EndTime.ToString("HH:mm"),
                o.PaidType,
                o.Status
            }.ToArray());

            return Json(new
            {
                param.sEcho,
                iTotalRecords = result.Count(),
                iTotalDisplayRecords = filteredListItems.Count(),
                aaData = result
            }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult _Index(int id)
        {

            var model = new OrderViewModel();
            var userId = User.Identity.GetUserId();
            var _userService = this.Service<IAspNetUserService>();
            var img = _userService.FirstOrDefaultActive(p => p.Id == userId).AvatarImage;
            model.FieldId = id;
            if (String.IsNullOrEmpty(img))
            {
                img = "http://i57.servimg.com/u/f57/16/18/15/10/1104.png";
            }
            ViewBag.avatar = img;
            model.UserId = userId;
            return this.PartialView(model);
        }

        [HttpPost]
        public ActionResult ConfirmOrder(OrderViewModel order)
        {
            HttpContext.Response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));
            HttpContext.Response.Cache.SetValidUntilExpires(false);
            HttpContext.Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
            HttpContext.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            HttpContext.Response.Cache.SetNoStore();
            var fieldId = order.FieldId;
            TimeSpan startTime = order.StartTime.TimeOfDay;
            TimeSpan endTime = order.EndTime.TimeOfDay;
            var _timeBlockService = this.Service<ITimeBlockService>();
            var _placeService = this.Service<IPlaceService>();
            var _fieldService = this.Service<IFieldService>();
            var _orderService = this.Service<IOrderService>();
            var _fieldScheduleService = this.Service<IFieldScheduleService>();

            bool rs1 = _orderService.checkOrderTimeValid(fieldId, startTime, endTime, order.CreateDate);
            bool rs2 = _fieldScheduleService.checkScheduleTimeValid(fieldId, startTime, endTime, order.CreateDate);
            if (!rs1 || !rs2)
            {
                return RedirectToAction("PageNotFound", "Error");
            }


            var field = _fieldService.FirstOrDefaultActive(p => p.Id == fieldId);
            Place place = new Place();
            if (field != null)
            {
                place = _placeService.FirstOrDefaultActive(p => p.Id == field.PlaceId);
                ViewBag.field = field;
            }
            order.Price = _timeBlockService.calPrice(fieldId, startTime, endTime);
            ViewBag.playDate = order.CreateDate;
            order.CreateDate = DateTime.Today;
            if (place != null)
            {
                ViewBag.place = place;
            }
            var userId = User.Identity.GetUserId();
            var _userService = this.Service<IAspNetUserService>();
            var user = _userService.FirstOrDefaultActive(p => p.Id == userId);
            if (user != null)
            {
                ViewBag.user = user;
            }
            order.UserId = userId;
            return View(order);
        }

        public ActionResult CalculatePrice(int fieldId, string startTime, string endTime)
        {
            var result = new AjaxOperationResult();
            double price = 0;
            TimeSpan StartTime = TimeSpan.Parse(startTime);
            TimeSpan EndTime = TimeSpan.Parse(endTime);
            var _timeBlockService = this.Service<ITimeBlockService>();
            bool rs = _timeBlockService.checkTimeValid(fieldId, StartTime, EndTime);
            if (rs)
            {
                price = _timeBlockService.calPrice(fieldId, StartTime, EndTime);
            }
            return Json(new
            {
                price = price
            }, JsonRequestBehavior.AllowGet);

        }

        public JsonResult CheckTimeValidInTimeBlock(int fieldId, string startTime, string endTime)
        {
            
            var result = new AjaxOperationResult();
            TimeSpan StartTime = TimeSpan.Parse(startTime);
            TimeSpan EndTime = TimeSpan.Parse(endTime);
            var _timeBlockService = this.Service<ITimeBlockService>();

            try
            {
                bool rs = _timeBlockService.checkTimeValid(fieldId, StartTime, EndTime);
                if (rs)
                {
                    result.Succeed = true;
                }
                else
                {
                    result.Succeed = false;
                }

            }
            catch (Exception)
            {
                result.Succeed = false;
            }
            return Json(result);
        }

        public JsonResult CheckOrderTimeExist(int fieldId, string startTime, string endTime, string playDate)
        {
            var result = new AjaxOperationResult();
            TimeSpan StartTime = TimeSpan.Parse(startTime);
            TimeSpan EndTime = TimeSpan.Parse(endTime);
            DateTime PlayDate = DateTime.ParseExact(playDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            var _orderService = this.Service<IOrderService>();
            var _fieldScheduleService = this.Service<IFieldScheduleService>();

            try
            {
                bool rs1 = _orderService.checkOrderTimeValid(fieldId, StartTime, EndTime, PlayDate);
                bool rs2 = _fieldScheduleService.checkScheduleTimeValid(fieldId, StartTime, EndTime, PlayDate);
                if (rs1 && rs2)
                {
                    result.Succeed = true;
                }
                else
                {
                    result.Succeed = false;
                }

            }
            catch (Exception)
            {
                result.Succeed = false;
            }
            return Json(result);
        }
        //public JsonResult CreateOrder(int fieldId, string startTime, string endTime, string note,
        //    double price, string paidType, string useDate)
        //{
        //    TimeSpan StartTime = TimeSpan.Parse(startTime);
        //    TimeSpan EndTime = TimeSpan.Parse(endTime);
        //    DateTime UseDate = DateTime.Parse(useDate);
        //    DateTime sTime = new DateTime(UseDate.Year, UseDate.Month, UseDate.Day, StartTime.Hours,
        //        StartTime.Minutes, StartTime.Seconds);
        //    DateTime eTime = new DateTime(UseDate.Year, UseDate.Month, UseDate.Day, EndTime.Hours,
        //        EndTime.Minutes, EndTime.Seconds);
        //    var userId = User.Identity.GetUserId();
        //    var _orderService = this.Service<IOrderService>();
        //    int pType = Int32.Parse(paidType);
        //    Order result = _orderService.CreateOrder(userId, fieldId, sTime, eTime, note, price, pType);
        //    if (result != null)
        //    {
        //        return Json(new
        //        {
        //            success = true
        //        });
        //    }
        //    else
        //    {
        //        return Json(new
        //        {
        //            success = false
        //        });
        //    }
        //}

        public ActionResult PayOnlineNow(object sender, EventArgs e, OrderViewModel model)
        {
            
            var _orderService = this.Service<IOrderService>();
            var order = new Order();
            if (String.IsNullOrEmpty(model.OrderCode))
            {
                return RedirectToAction("PageNotFound", "Error");
            }
            else
            {
                order = _orderService.FirstOrDefaultActive(p => p.OrderCode == model.OrderCode);
                if(order != null)
                {
                    String transaction_info = "Thanh toan dat san";
                    String receiver = "m249bornbeast@gmail.com";//Tài khoản nhận tiền
                    String return_url = Url.Action("verifyOrder", "Order",
                               new { area = "", orderCode = order.OrderCode }, Request.Url.Scheme);
                    String cancel_url = Url.Action("Index", "Order");
                    //String price = model.Price.ToString();
                    String price = "2000";
                    NL_Checkout nl = new NL_Checkout();
                    String url;
                    url = nl.buildCheckoutUrl(return_url, cancel_url, receiver, transaction_info, order.OrderCode, price);

                    order.PaidType = (int)OrderPaidType.ChosePayOnline;
                    _orderService.Update(order);
                    return Redirect(url);
                }
                else
                {
                    return RedirectToAction("PageNotFound", "Error");
                }
            }
        }

        public ActionResult btnSubmit_Click(object sender, EventArgs e, OrderViewModel model)
        {

            String transaction_info = "Thanh toan dat san";
            String order_code = DateTime.Now.ToString("yyyyMMddHHmmss");
            String receiver = "m249bornbeast@gmail.com";//Tài khoản nhận tiền
            String return_url = Url.Action("verifyOrder", "Order",
                       new { area = "", orderCode = order_code }, Request.Url.Scheme);
            String cancel_url = Url.Action("Index", "Order");
            //String price = model.Price.ToString();
            String price = "2000";
            NL_Checkout nl = new NL_Checkout();
            String url;
            url = nl.buildCheckoutUrl(return_url, cancel_url, receiver, transaction_info, order_code, price);

            var _orderService = this.Service<IOrderService>();
            var order = new Order();
            DateTime PlayDate = DateTime.ParseExact(Request["CreateDate"], "dd/MM/yyyy", CultureInfo.InvariantCulture);
            order.UserId = User.Identity.GetUserId();
                order.FieldId = model.FieldId;
                DateTime sTime = new DateTime(PlayDate.Year, PlayDate.Month, PlayDate.Day, model.StartTime.Hour,
                    model.StartTime.Minute, model.StartTime.Second);
                DateTime eTime = new DateTime(PlayDate.Year, PlayDate.Month, PlayDate.Day, model.EndTime.Hour,
                    model.EndTime.Minute, model.EndTime.Second);
                order.StartTime = sTime;
                order.EndTime = eTime;
                order.CreateDate = DateTime.Now;
                //order.Token = result.Token;
                order.Price = model.Price;
                //order.OnlinePaymentMethod = info.Payment_method;
                //order.BankCode = info.bank_code;
                order.Note = model.Note;
                order.PayerName = model.PayerName;
                order.PayerEmail = model.PayerEmail;
                order.PayerPhone = model.PayerPhone;
                order.Status = (int)OrderStatus.Pending;
                order.OrderCode = order_code;
                var transdate = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                order.TransactionTime = DateTime.Parse(transdate);

                if (model.PaidType == (int)OrderPaidType.ChosePayOnline)
                {
                    order.PaidType = (int)OrderPaidType.ChosePayOnline;

                }
                if (model.PaidType == (int)OrderPaidType.ChosePayByCash)
                {
                    order.PaidType = (int)OrderPaidType.ChosePayByCash;
                    url = Url.Action("BookFieldSuccessful", "Order",
                           new { area = "", orderCode = order_code }, Request.Url.Scheme);
                }
                _orderService.Create(order);
            return Redirect(url);
        }

        public ActionResult verifyOrder(object sender, EventArgs e, string orderCode)
        {
            String transaction_info = Request.QueryString["transaction_info"];
            String order_code = Request.QueryString["order_code"];
            String payment_id = Request.QueryString["payment_id"];
            String payment_type = Request.QueryString["payment_type"];
            String secure_code = Request.QueryString["secure_code"];
            String price = Request.QueryString["price"];
            String error_text = Request.QueryString["error_text"];
            NL_Checkout nl = new NL_Checkout();
            bool check = nl.verifyPaymentUrl(transaction_info, order_code, price, payment_id, payment_type, error_text, secure_code);
            if (check)
            {
                var _orderService = this.Service<IOrderService>();
                var order = _orderService.FirstOrDefaultActive(p => p.OrderCode == orderCode);
                if (order != null)
                {
                    order.PaidType = (int)OrderPaidType.PaidOnline;
                    _orderService.Update(order);
                    var url = Url.Action("PaymentSuccessful", "Order",
                       new { area = "", orderCode = order_code }, Request.Url.Scheme);
                    return Redirect(url);
                }
                else
                {
                    return RedirectToAction("PageNotFound", "Error");
                }

            }
            else
            {
                return RedirectToAction("PaymentFail", "Error");
            }
        }

        [AllowAnonymous]
        public ActionResult PaymentSuccessful(string orderCode)
        {
            var _orderService = this.Service<IOrderService>();
            var entity = _orderService.FirstOrDefaultActive(q => q.OrderCode == orderCode);

            if (entity == null)
            {
                return RedirectToAction("PageNotFound", "Error");
            }

            return View(new OrderViewModel(entity));
        }

        public ActionResult BookFieldSuccessful(string orderCode)
        {
            var _orderService = this.Service<IOrderService>();
            var entity = _orderService.FirstOrDefaultActive(q => q.OrderCode == orderCode);

            if (entity == null)
            {
                return RedirectToAction("PageNotFound", "Error");
            }

            return View(new OrderViewModel(entity));
        }

        public ActionResult PaymentFail()
        {

            return View();
        }

        public ActionResult BookFieldNow(int? placeId)
        {
            HttpContext.Response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));
            HttpContext.Response.Cache.SetValidUntilExpires(false);
            HttpContext.Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
            HttpContext.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            HttpContext.Response.Cache.SetNoStore();
            var userId = User.Identity.GetUserId();
            var _userService = this.Service<IAspNetUserService>();
            var user = _userService.FirstOrDefaultActive(p => p.Id == userId);
            if (user != null)
            {
                ViewBag.user = user;
            }
            var _fieldService = this.Service<IFieldService>();
            var fieldList = _fieldService.GetActive(p => p.PlaceId == placeId);
            IEnumerable<SelectListItem> selectList = fieldList.Select(s => new SelectListItem
            {
                Text = s.Name,
                Value = s.Id.ToString()
            }).ToArray();
            ViewBag.FieldList = selectList;
            var model = new OrderViewModel();
            return View(model);
        }
    }
}