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
using SportsSocialNetwork.Models.Utilities;
using QRCoder;
using Microsoft.AspNet.SignalR;
using SportsSocialNetwork.Models.Hubs;
using SportsSocialNetwork.Models.Notifications;
using HenchmenWeb.Models.Notifications;

namespace SportsSocialNetwork.Controllers
{
    [MyAuthorize(Roles = IdentityMultipleRoles.SSN)]
    public class OrderController : BaseController
    {

        // GET: Order
        public ActionResult Index()
        {
            var _followService = this.Service<IFollowService>();
            var _userService = this.Service<IAspNetUserService>();
            var _sportService = this.Service<ISportService>();
            string curUserId = User.Identity.GetUserId();

            var sports = _sportService.GetActive()
                            .Select(s => new SelectListItem
                            {
                                Text = s.Name,
                                Value = s.Id.ToString()
                            }).OrderBy(s => s.Value);
            ViewBag.Sport = sports;

            //get list of user that this user is following
            List<Follow> followingList = _followService.GetActive(f => f.FollowerId == curUserId).ToList();
            List<FollowDetailViewModel> followingListVM = Mapper.Map<List<FollowDetailViewModel>>(followingList);
            foreach (var item in followingListVM)
            {
                AspNetUser user = _userService.FirstOrDefaultActive(u => u.Id.Equals(item.UserId));
                AspNetUserViewModel userVM = Mapper.Map<AspNetUserViewModel>(user);
                item.User = userVM;
            }
            ViewBag.followingList = followingListVM;

            //load group name
            var _groupService = this.Service<IGroupService>();
            List<Group> groupList = _groupService.GetActive(p => p.GroupMembers.Where(f =>
           f.UserId == curUserId && f.Status == (int)GroupMemberStatus.Approved).ToList().Count > 0).ToList();
            if (groupList != null)
            {
                ViewBag.GroupList = groupList;
            }
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

        public ActionResult FieldScheduleOrderDetail(int id)
        {
            var _placeService = this.Service<IPlaceService>();
            var _fieldService = this.Service<IFieldService>();
            var _fieldScheduleService = this.Service<IFieldScheduleService>();
            FieldSchedule fs = _fieldScheduleService.FirstOrDefault(o => o.Id == id);
            var fieldId = fs.FieldId;
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
            FieldScheduleViewModel model = new FieldScheduleViewModel(fs);
            model.StartTimeString = model.StartTime.Hours.ToString("00") + ":" + model.StartTime.Minutes.ToString("00");
            model.EndTimeString = model.EndTime.Hours.ToString("00") + ":" + model.EndTime.Minutes.ToString("00");
            var bits = new bool[8];
            for (var i = 7; i >= 0; i--)
            {
                bits[i] = (model.AvailableDay & (1 << i)) != 0;
            }
            var dayOfWeek = "";
            if (bits[1])
            {
                dayOfWeek += "CN ";
            }
            if (bits[2])
            {
                dayOfWeek += "T2 ";
            }
            if (bits[3])
            {
                dayOfWeek += "T3 ";
            }
            if (bits[4])
            {
                dayOfWeek += "T4 ";
            }
            if (bits[5])
            {
                dayOfWeek += "T5 ";
            }
            if (bits[6])
            {
                dayOfWeek += "T6 ";
            }
            if (bits[7])
            {
                dayOfWeek += "T7 ";
            }
            model.availableDayOfWeek = dayOfWeek;
            return this.PartialView(model);
        }

        public ActionResult GetData(JQueryDataTableParamModel param)
        {
            //var blogPostList = _blogPostService.GetBlogPostbyStoreId();
            var _orderService = this.Service<IOrderService>();
            var userId = User.Identity.GetUserId();
            var orderList = _orderService.GetActive(p => p.UserId == userId).OrderByDescending(p => p.Id);
            if(orderList != null)
            {
                _orderService.AutoCancelOrder(orderList.ToList());
            }
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
                
                //case 0:
                //    filteredListItems = sortDirection == "asc"
                //        ? filteredListItems.OrderBy(o => o.OrderCode)
                //        : filteredListItems.OrderByDescending(o => o.OrderCode);
                //    break;
                //case 1:
                //    filteredListItems = sortDirection == "asc"
                //        ? filteredListItems.OrderBy(o => o.Field.Name)
                //        : filteredListItems.OrderByDescending(o => o.Field.Name);
                //    break;
                case 2:
                    filteredListItems = sortDirection == "asc"
                        ? filteredListItems.OrderBy(o => o.CreateDate)
                        : filteredListItems.OrderByDescending(o => o.CreateDate);
                    break;
                case 3:
                    filteredListItems = sortDirection == "asc"
                        ? filteredListItems.OrderBy(o => o.EndTime)
                        : filteredListItems.OrderByDescending(o => o.EndTime);
                    break;
                case 6:
                    filteredListItems = sortDirection == "asc"
                        ? filteredListItems.OrderBy(o => o.PaidType)
                        : filteredListItems.OrderByDescending(o => o.PaidType);
                    break;
                case 7:
                    filteredListItems = sortDirection == "asc"
                        ? filteredListItems.OrderBy(o => o.Status)
                        : filteredListItems.OrderByDescending(o => o.Status);
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
                img = "/Content/images/no_image.jpg.";
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
            DateTime PlayDate = DateTime.ParseExact(Request["CreateDate"], "dd/MM/yyyy", CultureInfo.InvariantCulture);
            var _timeBlockService = this.Service<ITimeBlockService>();
            var _placeService = this.Service<IPlaceService>();
            var _fieldService = this.Service<IFieldService>();
            var _orderService = this.Service<IOrderService>();
            var _fieldScheduleService = this.Service<IFieldScheduleService>();

            bool rs1 = _orderService.checkTimeValidInOrder(fieldId, startTime, endTime, PlayDate, PlayDate);
            bool rs2 = _fieldScheduleService.checkTimeValidInFieldSchedule(null,fieldId, startTime, endTime, PlayDate, PlayDate);
            if (!rs1 || !rs2)
            {
                return RedirectToAction("PageNotFound", "Errors");
            }


            var field = _fieldService.FirstOrDefaultActive(p => p.Id == fieldId);
            Place place = new Place();
            if (field != null)
            {
                place = _placeService.FirstOrDefaultActive(p => p.Id == field.PlaceId);
                ViewBag.field = field;
            }
            order.Price = _timeBlockService.calPrice(fieldId, startTime, endTime);
            //ViewBag.playDate = order.CreateDate;
            order.CreateDate = DateTime.Today;
            DateTime sTime = new DateTime(PlayDate.Year, PlayDate.Month, PlayDate.Day, order.StartTime.Hour,
                    order.StartTime.Minute, order.StartTime.Second);
            DateTime eTime = new DateTime(PlayDate.Year, PlayDate.Month, PlayDate.Day, order.EndTime.Hour,
                order.EndTime.Minute, order.EndTime.Second);
            order.StartTime = sTime;
            order.EndTime = eTime;
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
                price = price.ToString("n0")
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
                bool rs1 = _orderService.checkTimeValidInOrder(fieldId, StartTime, EndTime, PlayDate, PlayDate);
                bool rs2 = _fieldScheduleService.checkTimeValidInFieldSchedule(null,fieldId, StartTime, EndTime, PlayDate, PlayDate);
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
                return RedirectToAction("PageNotFound", "Errors");
            }
            else
            {
                order = _orderService.FirstOrDefaultActive(p => p.OrderCode == model.OrderCode);
                if (order != null)
                {
                    String transaction_info = "Thanh toan dat san";
                    String receiver = "m249bornbeast@gmail.com";//Tài khoản nhận tiền
                    String return_url = Url.Action("verifyOrder", "Order",
                               new { area = "", orderCode = order.OrderCode }, Request.Url.Scheme);
                    String cancel_url = "http://ssn.techeco.net/Order";
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
                    return RedirectToAction("PageNotFound", "Errors");
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
            String cancel_url = "http://ssn.techeco.net/Order";
            //String price = model.Price.ToString();
            String price = "2000";
            NL_Checkout nl = new NL_Checkout();
            String url;
            url = nl.buildCheckoutUrl(return_url, cancel_url, receiver, transaction_info, order_code, price);

            var _orderService = this.Service<IOrderService>();
            var _timeBlockService = this.Service<ITimeBlockService>();
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
            order.Price = _timeBlockService.calPrice(model.FieldId, sTime.TimeOfDay, eTime.TimeOfDay);
            //order.OnlinePaymentMethod = info.Payment_method;
            //order.BankCode = info.bank_code;
            order.Note = model.Note;
            order.PayerName = model.PayerName;
            order.PayerEmail = model.PayerEmail;
            order.PayerPhone = model.PayerPhone;
            order.Status = (int)OrderStatus.Pending;
            order.OrderCode = order_code;
            order.QRCodeUrl = Utils.GenerateQRCode(order_code, QRCodeGenerator.ECCLevel.Q);
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

            var _userService = this.Service<IAspNetUserService>();
            var userId = User.Identity.GetUserId();
            var user = _userService.FirstOrDefaultActive(p => p.Id == userId);
            if (user == null)
            {
                return RedirectToAction("PageNotFound", "Errors");
            }

            var _fieldService = this.Service<IFieldService>();
            var field = _fieldService.FirstOrDefaultActive(p => p.Id == order.FieldId);
            if(field == null)
            {
                return RedirectToAction("PageNotFound", "Errors");
            }
            var noti = new Notification();
            noti.UserId = field.Place.UserId;
            noti.FromUserId = userId;
            noti.CreateDate = DateTime.Now;
            noti.Message = user.FullName + " đã đặt sân tại " + field.Name;
            noti.Title = "Đơn hàng mới";
            noti.Type = (int)NotificationType.Order;
            noti.MarkRead = false;
            noti.Active = true;
            order.Notifications.Add(noti);
            _orderService.Create(order);


            //Fire base noti
            List<string> registrationIds = GetToken(noti.UserId);

            //registrationIds.Add("dgizAK4sGBs:APA91bGtyQTwOiAgNHE_mIYCZhP0pIqLCUvDzuf29otcT214jdtN2e9D6iUPg3cbYvljKbbRJj5z7uaTLEn1WeUam3cnFqzU1E74AAZ7V82JUlvUbS77mM42xHZJ5DifojXEv3JPNEXQ");

            NotificationModel Amodel = Mapper.Map<NotificationModel>(PrepareNotificationCustomViewModel(noti));

            if (registrationIds != null && registrationIds.Count != 0)
            {
                Android.Notify(registrationIds, null, Amodel);
            }

            //SignalR Noti
            var notiService = this.Service<INotificationService>();
            NotificationFullInfoViewModel notiModelR = notiService.PrepareNoti(Mapper.Map<NotificationFullInfoViewModel>(noti));

            // Get the context for the Pusher hub
            IHubContext hubContext = GlobalHost.ConnectionManager.GetHubContext<RealTimeHub>();

            // Notify clients in the group
            hubContext.Clients.User(notiModelR.UserId).send(notiModelR);


            string subject = "[SSN] - Thông tin đặt sân";
            string body = "Hi <strong>" + user.FullName + "</strong>" +
                ",<br/><br/>Bạn đã đặt sân: "+field.Name+"<br/> Tại địa điểm: "+field.Place.Name+
                "<br/> Thời gian: "+order.StartTime.ToString("HH:mm")+" - "+
                order.EndTime.ToString("HH:mm") +", ngày "+order.StartTime.ToString("dd/MM/yyyy")+
                "<br/> Giá tiền : " + order.Price.ToString("n0") + " đồng" +
                "<br/> <strong>Mã đặt sân của bạn : " + order.OrderCode + "</strong>"+
                "<br/><img src='ssn.techeco.net/" + order.QRCodeUrl + "'>"+
                "<br/> Cảm ơn bạn đã sử dụng dịch vụ của SSN. Chúc bạn có những giờ phút thoải mái chơi thể thao!";
            EmailSender.Send(Setting.CREDENTIAL_EMAIL, new string[] { model.PayerEmail }, null, null, subject, body, true);

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
                    var _userService = this.Service<IAspNetUserService>();
                    var userId = User.Identity.GetUserId();
                    var user = _userService.FirstOrDefaultActive(p => p.Id == userId);
                    if (user == null)
                    {
                        return RedirectToAction("PageNotFound", "Errors");
                    }
                    string subject = "[SSN] - Thông tin thanh toán";
                    string body = "Hi <strong>" + user.FullName + "</strong>" +
                        ",<br/><br/>Bạn đã thanh toán đơn đặt sân: "+ order.OrderCode +" thành công"+
                        "<br/><strong>Thông tin hóa đơn:</strong><ul> " +
                        "<li> Tên sân: "+order.Field.Name + "</li>" + 
                        "<li> Tại địa điểm: " + order.Field.Place.Name + "</li>" +
                        "<li> Thời gian: " + order.StartTime.ToString("HH:mm") + " - " +
                        order.EndTime.ToString("HH:mm") + ", ngày " + order.StartTime.ToString("dd/MM/yyyy") +"</li>"+
                        "<li> Giá tiền : " + order.Price.ToString("n0") + " đồng</li></ul>" +
                        "<br/> Cảm ơn bạn đã sử dụng dịch vụ của SSN. Chúc bạn có những giờ phút thoải mái chơi thể thao!";
                    EmailSender.Send(Setting.CREDENTIAL_EMAIL, new string[] { order.PayerEmail }, null, null, subject, body, true);
                    var url = Url.Action("PaymentSuccessful", "Order",
                       new { area = "", orderCode = order_code }, Request.Url.Scheme);
                    return Redirect(url);
                }
                else
                {
                    return RedirectToAction("PageNotFound", "Errors");
                }

            }
            else
            {
                return RedirectToAction("PaymentFail", "Order");
            }
        }

        [AllowAnonymous]
        public ActionResult PaymentSuccessful(string orderCode)
        {
            var _orderService = this.Service<IOrderService>();
            var entity = _orderService.FirstOrDefaultActive(q => q.OrderCode == orderCode);

            if (entity == null)
            {
                return RedirectToAction("PageNotFound", "Errors");
            }

            return View(new OrderViewModel(entity));
        }

        public ActionResult BookFieldSuccessful(string orderCode)
        {
            var _orderService = this.Service<IOrderService>();
            var entity = _orderService.FirstOrDefaultActive(q => q.OrderCode == orderCode);

            if (entity == null)
            {
                return RedirectToAction("PageNotFound", "Errors");
            }

            return View(new OrderViewModel(entity));
        }

        public ActionResult PaymentFail()
        {

            return View();
        }

        public ActionResult BookFieldNow(int? id)
        {
            var _fieldService = this.Service<IFieldService>();
            var _placeService = this.Service<IPlaceService>();
            var fieldList = _fieldService.GetActive(p => p.PlaceId == id && p.Status != (int)FieldStatus.Deactive);
            if (fieldList == null || fieldList.ToList().Count == 0)
            {
                return RedirectToAction("PageNotFound", "Errors");
            }
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
            var place = _placeService.FirstOrDefaultActive(p => p.Id == id);
            var placeOwner = _userService.FirstOrDefaultActive(p => p.Id == place.UserId);
            ViewBag.PlaceOwnerNganLuong = placeOwner.NganLuongAccount;
            IEnumerable<SelectListItem> selectList = fieldList.Select(s => new SelectListItem
            {
                Text = s.Name + " - " + s.FieldType.Name + " (" +s.FieldType.Sport.Name + " )",
                Value = s.Id.ToString()
            }).ToArray();
            ViewBag.FieldList = selectList;
            var model = new OrderViewModel();
            return View(model);
        }

        private NotificationCustomViewModel PrepareNotificationCustomViewModel(Notification noti)
        {
            NotificationCustomViewModel result = Mapper.Map<NotificationCustomViewModel>(noti);

            result.CreateDateString = result.CreateDate.ToString("dd/MM/yyyy HH:mm:ss");

            result.Avatar = noti.AspNetUser1.AvatarImage;

            return result;

        }

        private List<string> GetToken(String userId)
        {
            var service = this.Service<IFirebaseTokenService>();

            List<FirebaseToken> tokenList = service.Get(x => x.UserId.Equals(userId)).ToList();

            List<string> registrationIds = new List<string>();
            if (tokenList != null)
            {
                foreach (var token in tokenList)
                {
                    registrationIds.Add(token.Token);
                }
            }

            return registrationIds;
        }
    }
}