
using QRCoder;
using SkyWeb.DatVM.Mvc;
using SportsSocialNetwork.Models;
using SportsSocialNetwork.Models.Entities;
using SportsSocialNetwork.Models.Entities.Services;
using SportsSocialNetwork.Models.Enumerable;
using SportsSocialNetwork.Models.Utilities;
using SportsSocialNetwork.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SportsSocialNetwork.Areas.API.Controllers
{
    public class OrderController : BaseController
    {
        private String systemError = "Đã có lỗi xảy ra!";


        [HttpPost]
        public ActionResult ShowAllOrderOfUser(String userId)
        {
            ResponseModel<List<OrderSimpleViewModel>> response = null;

            var service = this.Service<IOrderService>();

            List<Order> orderList = null;

            try
            {
                orderList = service.GetAllOrderOfUser(userId).ToList<Order>();

                List<OrderSimpleViewModel> result = new List<OrderSimpleViewModel>();

                foreach (var order in orderList)
                {
                    result.Add(PrepareOrderSimpleViewModel(order));
                }

                response = new ResponseModel<List<OrderSimpleViewModel>>(true, "Đơn đặt sân của bạn đã được tải thành công!", null, result);
            }
            catch (Exception)
            {
                response = ResponseModel<List<OrderSimpleViewModel>>.CreateErrorResponse("Đơn đặt sân của bạn đã tải thất bại!", systemError);
            }

            return Json(response);
        }

        [HttpPost]
        public ActionResult ShowAllOrderOfPlaceOwner(String ownerId)
        {
            var placeService = this.Service<IPlaceService>();

            var orderService = this.Service<IOrderService>();

            var fieldService = this.Service<IFieldService>();

            ResponseModel<List<OrderSimpleViewModel>> response = null;

            try
            {
                List<Place> placeList = placeService.FindAllPlaceOfPlaceOwner(ownerId).ToList();

                List<Field> fieldList = new List<Field>();

                List<Order> orderList = new List<Order>();

                foreach (var place in placeList)
                {
                    fieldList = fieldList.Concat(fieldService.FindAllFieldsOfPlace(place.Id)).ToList();
                }

                foreach (var field in fieldList)
                {

                    orderList = orderList.Concat(orderService.GetAllOrderByFieldId(field.Id)).ToList();
                }

                List<OrderSimpleViewModel> result = new List<OrderSimpleViewModel>();

                foreach (var order in orderList)
                {
                    result.Add(PrepareOrderSimpleViewModel(order));
                }

                response = new ResponseModel<List<OrderSimpleViewModel>>(true, "Danh sách đặt sân của bạn đã được tải thành công!", null, result);

            }
            catch (Exception)
            {
                response = ResponseModel<List<OrderSimpleViewModel>>.CreateErrorResponse("Danh sách đặt sân đã tải thất bại!", systemError);
            }


            return Json(response);


        }

        [HttpPost]
        public ActionResult ShowOrderDetail(int id)
        {
            ResponseModel<OrderSimpleViewModel> response = null;

            var service = this.Service<IOrderService>();


            try
            {
                Order order = service.GetOrderById(id);

                OrderSimpleViewModel result = PrepareOrderSimpleViewModel(order);

                response = new ResponseModel<OrderSimpleViewModel>(true, "Thông tin đặt sân:", null, result);
            }

            catch (Exception)
            {
                response = ResponseModel<OrderSimpleViewModel>.CreateErrorResponse("Không thể tải thông tin đặt sân", systemError);
            }


            return Json(response);

        }

        [HttpPost]
        public ActionResult ChangeOrderStatus(int id, int status)
        {
            ResponseModel<OrderSimpleViewModel> response = null;

            var service = this.Service<IOrderService>();

            Order order = null;

            try
            {
                order = service.ChangeOrderStatus(id, status);

                OrderSimpleViewModel result = PrepareOrderSimpleViewModel(order);

                response = new ResponseModel<OrderSimpleViewModel>(true, "Trạng thái đơn đặt sân đã thay đổi thành công", null, result);
            }

            catch (Exception)
            {
                response = ResponseModel<OrderSimpleViewModel>.CreateErrorResponse("Thay đổi trạng thái đơn đặt sân thất bại", systemError);
            }

            return Json(response);
        }

        [HttpPost]
        public ActionResult CancelOrder(int id)
        {
            ResponseModel<OrderSimpleViewModel> response = null;

            var service = this.Service<IOrderService>();

            Order order = null;

            try
            {
                order = service.ChangeOrderStatus(id, (int)OrderStatus.Cancel);

                OrderSimpleViewModel result = PrepareOrderSimpleViewModel(order);

                response = new ResponseModel<OrderSimpleViewModel>(true, "Đơn đặt sân đã được hủy", null, result);
            }

            catch (Exception)
            {
                response = ResponseModel<OrderSimpleViewModel>.CreateErrorResponse("Hủy đơn đặt sân thất bại", systemError);
            }

            return Json(response);
        }

        [HttpPost]
        public ActionResult CreateOrder(CreateOrderViewModel model)
        {
            ResponseModel<OrderSimpleViewModel> response = null;

            var orderService = this.Service<IOrderService>();

            var fieldService = this.Service<IFieldService>();

            var userService = this.Service<IAspNetUserService>();

            DateTime startTime = new DateTime();

            DateTime endTime = new DateTime();

            DateTime playDate = new DateTime();

            try
            {
                startTime = DateTime.ParseExact(model.StartTime, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);

                endTime = DateTime.ParseExact(model.EndTime, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);

                playDate = DateTime.ParseExact(model.PlayDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                response = ResponseModel<OrderSimpleViewModel>.CreateErrorResponse("Đặt sân thất bại", "Lỗi định dạng thời gian");

                return Json(response);
            }

            if (!CheckTimeValid(model.FieldId, startTime.TimeOfDay, endTime.TimeOfDay))
            {
                response = ResponseModel<OrderSimpleViewModel>.CreateErrorResponse("Đặt sân thất bại", "Thời gian không nằm trong các khung giờ");

                return Json(response);
            }
            else if (!CheckOrderTime(model.FieldId, startTime.TimeOfDay, endTime.TimeOfDay, playDate,playDate))
            {
                response = ResponseModel<OrderSimpleViewModel>.CreateErrorResponse("Đặt sân thất bại", "Sân đặt được đặt vào thời gian này");

                return Json(response);
            }
            double price = CalculatePrice(model.FieldId, startTime.TimeOfDay, endTime.TimeOfDay);

            try
            {
                //Get current user info
                var user = userService.FindUser(model.UserId);

                //Create order info
                String orderCode = DateTime.Now.ToString("yyyyMMddHHmmss");

                Order order = new Order();

                order.FieldId = model.FieldId;
                order.UserId = model.UserId;
                order.StartTime = startTime;
                order.EndTime = endTime;
                order.CreateDate = DateTime.Now;
                order.Price = price;
                order.Note = model.Note;
                order.PayerName = model.PayerName;
                order.PayerEmail = model.PayerEmail;
                order.PayerPhone = model.PayerPhone;
                order.Status = (int)OrderStatus.Pending;
                order.OrderCode = orderCode;
                order.QRCodeUrl = Utils.GenerateQRCode(orderCode, QRCodeGenerator.ECCLevel.Q);
                var transdate = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                order.TransactionTime = DateTime.Parse(transdate);

                if (model.PaidType == (int)OrderPaidType.ChosePayOnline)
                {
                    order.PaidType = (int)OrderPaidType.ChosePayOnline;

                }
                if (model.PaidType == (int)OrderPaidType.ChosePayByCash)
                {
                    order.PaidType = (int)OrderPaidType.ChosePayByCash;
                }


                //Create Noti
                var field = fieldService.FirstOrDefaultActive(p => p.Id == order.FieldId);
                var noti = new Notification();
                noti.UserId = field.Place.UserId;
                noti.Message = user.UserName + " đã đặt sân tại " + field.Name;
                noti.Title = "Đơn hàng mới";
                noti.Type = (int)NotificationType.Order;
                noti.Active = true;
                order.Notifications.Add(noti);


                //Save Order
                order = orderService.CreateOrder(order);


                //Send Email
                string subject = "[SSN] - Thông tin đặt sân";
                string body = "Hi <strong>" + user.UserName + "</strong>" +
                    ",<br/><br/>Bạn đã đặt sân: " + field.Name + "<br/> Thời gian: " + order.StartTime.ToString("HH:mm") + " - " +
                    order.EndTime.ToString("HH:mm") + ", ngày " + order.StartTime.ToString("dd/MM/yyyy") +
                    "<br/> Giá tiền : " + order.Price + " đồng" +
                    "<br/> <strong>Mã đặt sân của bạn : " + order.OrderCode + "</strong>" +
                    "<br/><img src='" + Utils.GetHostName() + order.QRCodeUrl + "'>" +
                    "<br/> Cảm ơn bạn đã sử dụng dịch vụ của SSN. Chúc bạn có những giờ phút thoải mái chơi thể thao!";
                EmailSender.Send(Setting.CREDENTIAL_EMAIL, new string[] { user.Email }, null, null, subject, body, true);

                OrderSimpleViewModel result = PrepareOrderSimpleViewModel(order);

                response = new ResponseModel<OrderSimpleViewModel>(true, "Đặt sân thành công", null, result);

            }
            catch (Exception)
            {
                response = ResponseModel<OrderSimpleViewModel>.CreateErrorResponse("Đặt sân thất bại", systemError);
            }

            return Json(response);

        }

        [HttpPost]
        public ActionResult CheckInOrder(String orderCode)
        {
            var service = this.Service<IOrderService>();

            ResponseModel<OrderSimpleViewModel> response = null;

            try
            {
                Order order = service.FindOrderByCode(orderCode);

                if (order != null)
                {
                    if (order.Status == int.Parse(OrderStatus.Approved.ToString("d")))
                    {
                        service.ChangeOrderStatus(order.Id, int.Parse(OrderStatus.CheckedIn.ToString("d")));

                        OrderSimpleViewModel result = PrepareOrderSimpleViewModel(order);

                        response = new ResponseModel<OrderSimpleViewModel>(true, "Đơn đặt sân đã được checkin", null, result);

                    }
                    else if (order.Status == int.Parse(OrderStatus.Cancel.ToString("d")))
                    {
                        OrderSimpleViewModel result = PrepareOrderSimpleViewModel(order);

                        response = new ResponseModel<OrderSimpleViewModel>(false, "Đơn đặt sân chưa được checkin", new List<string> { "Đơn đặt sân đã bị hủy" }, result);
                    }
                    else if (order.Status == int.Parse(OrderStatus.Unapproved.ToString("d")))
                    {
                        OrderSimpleViewModel result = PrepareOrderSimpleViewModel(order);

                        response = new ResponseModel<OrderSimpleViewModel>(false, "Đơn đặt sân chưa được checkin", new List<string> { "Đơn đặt sân đã bị từ chối" }, result);
                    }
                    else if (order.Status == int.Parse(OrderStatus.CheckedIn.ToString("d")))
                    {
                        OrderSimpleViewModel result = PrepareOrderSimpleViewModel(order);

                        response = new ResponseModel<OrderSimpleViewModel>(true, "Đơn đặt sân đã được checkin", null, result);
                    }
                    else
                    {
                        OrderSimpleViewModel result = PrepareOrderSimpleViewModel(order);

                        response = new ResponseModel<OrderSimpleViewModel>(false, "Đơn đặt sân chưa được checkin", new List<string> { "Đơn đặt sân chưa được chấp nhận" }, result);
                    }
                }

                else
                {
                    response = ResponseModel<OrderSimpleViewModel>.CreateErrorResponse("Đơn đặt sân chưa được checkin", "Code không hợp lệ");
                }
            }
            catch (Exception)
            {
                response = ResponseModel<OrderSimpleViewModel>.CreateErrorResponse("Đơn đặt sân chưa được checkin", systemError);
            }

            return Json(response);
        }

        [HttpPost]
        public ActionResult GetPrice(int fieldId, String startTime, String endTime, String playDate)
        {
            ResponseModel<double> response = null;

            TimeSpan StartTime = new TimeSpan();

            TimeSpan EndTime = new TimeSpan();

            DateTime PlayDate = new DateTime();

            try
            {
                StartTime = TimeSpan.Parse(startTime);
                EndTime = TimeSpan.Parse(endTime);
                PlayDate = DateTime.ParseExact(playDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                response = ResponseModel<double>.CreateErrorResponse("Lỗi tính giá", "Định dạng ngày giờ không chính xác");
                return Json(response);
            }

            try
            {
                bool valid = CheckOrderTime(fieldId, StartTime, EndTime, PlayDate, PlayDate);

                if (valid)
                {
                    double result = CalculatePrice(fieldId, StartTime, EndTime);

                    response = new ResponseModel<double>(true, "Giá:", null, result);
                }
                else
                {
                    response = ResponseModel<double>.CreateErrorResponse("Không thể đặt sân", "Sân đặt được đặt vào thời gian này");
                }

            }
            catch (Exception)
            {
                response = ResponseModel<double>.CreateErrorResponse("Lỗi tính giá", systemError);
            }
            return Json(response);
        }

        private OrderSimpleViewModel PrepareOrderSimpleViewModel(Order order)
        {
            OrderSimpleViewModel result = Mapper.Map<OrderSimpleViewModel>(order);

            result.UserId = order.AspNetUser.Id;

            result.UserName = order.AspNetUser.UserName;

            result.FullName = order.AspNetUser.FullName;

            result.PhoneNumber = order.AspNetUser.PhoneNumber;

            result.FieldName = order.Field.Name;

            result.PlaceId = order.Field.PlaceId;

            result.PlaceName = order.Field.Place.Name;

            result.CreateDateString = result.CreateDate.ToString("dd/MM/yyyy HH:mm:ss");

            result.StartTimeString = result.StartTime.ToString("dd/MM/yyyy HH:mm:ss");

            result.EndTimeString = result.EndTime.ToString("dd/MM/yyyy HH:mm:ss");

            result.StatusString = Utils.GetEnumDescription((OrderStatus)result.Status);

            result.PaidTypeString = Utils.GetEnumDescription((OrderPaidType)result.PaidType);

            return result;
        }

        private double CalculatePrice(int fieldId, TimeSpan startTime, TimeSpan endTime)
        {
            double price = 0;

            var timeBlockService = this.Service<ITimeBlockService>();

            bool rs = timeBlockService.checkTimeValid(fieldId, startTime, endTime);
            if (rs)
            {
                price = timeBlockService.calPrice(fieldId, startTime, endTime);
            }
            return price;
        }

        private bool CheckTimeValid(int fieldId, TimeSpan startTime, TimeSpan endTime)
        {
            bool result = true;

            var _timeBlockService = this.Service<ITimeBlockService>();

            try
            {
                bool rs = _timeBlockService.checkTimeValid(fieldId, startTime, endTime);
                if (rs)
                {
                    result = true;
                }
                else
                {
                    result = false;
                }

            }
            catch (Exception)
            {
                result = false;
            }

            return result;
        }

        private bool CheckOrderTime(int fieldId, TimeSpan startTime, TimeSpan endTime, DateTime startDate, DateTime endDate)
        {
            bool result = false;
            var orderService = this.Service<IOrderService>();
            var fieldScheduleService = this.Service<IFieldScheduleService>();
            try
            {
                bool rs1 = orderService.checkTimeValidInOrder(fieldId, startTime, endTime, startDate, endDate);
                bool rs2 = fieldScheduleService.checkTimeValidInFieldSchedule(fieldId, startTime, endTime, startDate, endDate);
                if (rs1 && rs2)
                {
                    result = true;
                }
                else
                {
                    result = false;
                }

            }
            catch (Exception)
            {
                result = false;
            }
            return result;
        }

    }

}








