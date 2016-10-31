using SkyWeb.DatVM.Mvc;
using SkyWeb.DatVM.Mvc.Autofac;
using SportsSocialNetwork.Areas.PlaceOwner.Models.ViewModels;
using SportsSocialNetwork.Models.Entities;
using SportsSocialNetwork.Models.Entities.Services;
using SportsSocialNetwork.Models.Identity;
using SportsSocialNetwork.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SportsSocialNetwork.Models.Utilities;
using SportsSocialNetwork.Models.Enumerable;

namespace SportsSocialNetwork.Areas.PlaceOwner.Controllers
{
    [MyAuthorize(Roles = "Chủ sân")]
    public class OrderController : BaseController
    {
        // GET: PlaceOwner/Order
        public ActionResult Index()
        {
            return View();
        }

        public string updateStatusOrder(int id, int status, string reason)
        {
            var _orderService = this.Service<IOrderService>();
            var _userService = this.Service<IAspNetUserService>();
            var _notificationService = this.Service<INotificationService>();

            
            Order order = _orderService.ChangeOrderStatus(id, status);
            if(order != null)
            {
                //save noti
                Notification noti = new Notification();
                noti.UserId = order.UserId;
                noti.Title = Utils.GetEnumDescription(NotificationType.Order);
                noti.Type = (int)NotificationType.Order;
                //send mail
                string receiverEmail = _userService.FirstOrDefaultActive(u => u.Id.Equals(order.UserId)).Email;
                string subject = "";
                string body = "";
                if (status == (int)OrderStatus.Approved)
                {
                    subject = "SSN - Đơn đặt sân được chấp nhận";
                    body = "<p>Đơn đặt sân <strong>" + order.Field.Name + "</strong> từ <strong>" + order.StartTime.ToString() + "</trong> đến <strong>" + order.EndTime.ToString() + "</strong> đã được chấp nhận</p>"
                        + "<p>Quý khách vui lòng đến sân <strong>30 phút</strong> trước giờ đặt để xác nhận.</p>"
                        + "<p>Chúc quý khách có được những giây phút thư giãn vui vẻ!</p>";

                    noti.Message = "Đơn đặt sân " + order.Field.Name + "(" + order.StartTime.ToString() + " - " + order.EndTime.ToString() + ") đã được chấp nhận";

                }
                if (status == (int)OrderStatus.Unapproved)
                {
                    subject = "SSN - Đơn đặt sân đã bị từ chối";
                    body = "<p>Đơn đặt sân <strong>" + order.Field.Name + "</strong> từ <strong>" + order.StartTime.ToString() + "</trong> đến <strong>" + order.EndTime.ToString() + "</strong> đã bị chủ sân từ chối</p>"
                        + "<p>Lí do: " + reason + "</p>"
                        + "<p>Chúng tôi xin lỗi vì sự bất tiện này.</p>"
                        + "<p>Hẹn gặp lại quý khách lần sau!</p>";

                    noti.Message = "Đơn đặt sân " + order.Field.Name + "(" + order.StartTime.ToString() + " - " + order.EndTime.ToString() + ") đã bị từ chối";
                }
                //EmailSender.Send(Setting.CREDENTIAL_EMAIL, new string[] { receiverEmail, "itspace.quy@gmail.com" }, null, null, subject, body, true);
                _notificationService.Create(noti);
                _notificationService.Save();
                
                
                return "success";
            }
            else
            {
                return "false";
            }
        }

        public ActionResult OrderDetail(int id)
        {
            var _orderService = this.Service<IOrderService>();
            ////var _fieldService = this.Service<IFieldService>();
            Order order = _orderService.FirstOrDefault(o => o.Id == id);

            OrderDetailViewModel model = Mapper.Map<OrderDetailViewModel>(order);

            //OrderDetailViewModel model;

            //if (order != null)
            //{
            //    model = Mapper.Map<OrderDetailViewModel>(order);
            //    Field field = _fieldService.FirstOrDefault(f => f.Id == order.FieldId);
            //}
            return this.PartialView(model);
        }

        public ActionResult GetData(JQueryDataTableParamModel param)
        {
            string userID = Request["userID"];
            //var blogPostList = _blogPostService.GetBlogPostbyStoreId();
            var _orderService = this.Service<IOrderService>();
            var _placeSerivce = this.Service<IPlaceService>();
            var _fieldService = this.Service<IFieldService>();

            List<Place> placeList = _placeSerivce.Get(p => p.UserId == userID).ToList();
            List<Field> fieldList =  new List<Field>();
            foreach (var item in placeList)
            {
                fieldList.AddRange(_fieldService.Get(f => f.PlaceId == item.Id).ToList());
            }

            //var orderList = _orderService.GetActive();
            List<Order> orderList = new List<Order>();
            foreach (var item in fieldList)
            {
                orderList.AddRange(_orderService.Get(o => o.FieldId == item.Id).ToList());
            }
            //IEnumerable<BlogPost> filteredListItems;
            IEnumerable<Order> filteredListItems;
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                filteredListItems = orderList.Where(o => o.Field.Name != null && o.Field.Name.ToLower().Contains(param.sSearch.ToLower()));

                //filteredListItems = blogPostList.Where(
                //    d => (d.Title != null && d.Title.ToLower().Contains(param.sSearch.ToLower()))
                //    || (d.BlogCategory.Title != null && d.BlogCategory.Title.ToLower().Contains(param.sSearch.ToLower()))
                //    || (d.MetaDescription != null && d.MetaDescription.ToLower().Contains(param.sSearch.ToLower()))
                //);
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
                case 0:
                    filteredListItems = sortDirection == "asc"
                        ? filteredListItems.OrderBy(o => o.Field.Name)
                        : filteredListItems.OrderByDescending(o => o.Field.Name);
                    break;
                case 1:
                    filteredListItems = sortDirection == "asc"
                        ? filteredListItems.OrderBy(o => o.AspNetUser.FullName)
                        : filteredListItems.OrderByDescending(o => o.AspNetUser.FullName);
                    break;
                case 2:
                    filteredListItems = sortDirection == "asc"
                        ? filteredListItems.OrderBy(o => o.CreateDate.ToString("dd/MM/yyyy HH:mm"))
                        : filteredListItems.OrderByDescending(o => o.CreateDate.ToString("dd/MM/yyyy HH:mm"));
                    break;
                case 3:
                    filteredListItems = sortDirection == "asc"
                        ? filteredListItems.OrderBy(o => o.StartTime.ToString("dd/MM/yyyy HH:mm"))
                        : filteredListItems.OrderByDescending(o => o.StartTime.ToString("dd/MM/yyyy HH:mm"));
                    break;
                case 4:
                    filteredListItems = sortDirection == "asc"
                        ? filteredListItems.OrderBy(o => o.EndTime.ToString("dd/MM/yyyy HH:mm"))
                        : filteredListItems.OrderByDescending(o => o.EndTime.ToString("dd/MM/yyyy HH:mm"));
                    break;
                case 5:
                    filteredListItems = sortDirection == "asc"
                        ? filteredListItems.OrderBy(o => o.PaidType)
                        : filteredListItems.OrderByDescending(o => o.PaidType);
                    break;
                case 6:
                    filteredListItems = sortDirection == "asc"
                        ? filteredListItems.OrderBy(o => o.Status)
                        : filteredListItems.OrderByDescending(o => o.Status);
                    break;
            }

            var displayedList = filteredListItems.Skip(param.iDisplayStart).Take(param.iDisplayLength);
            var result = displayedList.Select(o => new IConvertible[]{
                //c.Id,
                //c.Image,
                //c.Title,
                //c.BlogCategoryId,
                //c.BlogCategory.Title,
                //c.Author,
                //c.Active,
                //c.Id

                o.Id,
                o.Field.Name,
                o.AspNetUser.FullName,
                o.CreateDate.ToString("dd/MM/yyyy HH:mm"),
                o.StartTime.ToString("dd/MM/yyyy HH:mm"),
                o.EndTime.ToString("dd/MM/yyyy HH:mm"),
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
    }
}