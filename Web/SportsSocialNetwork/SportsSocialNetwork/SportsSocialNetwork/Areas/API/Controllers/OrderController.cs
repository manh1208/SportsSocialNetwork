using SkyWeb.DatVM.Mvc;
using SportsSocialNetwork.Models;
using SportsSocialNetwork.Models.Entities;
using SportsSocialNetwork.Models.Entities.Services;
using SportsSocialNetwork.Models.Enumerable;
using SportsSocialNetwork.Models.Utilities;
using SportsSocialNetwork.Models.ViewModels;
using System;
using System.Collections.Generic;
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
            ResponseModel<List<OrderDetailViewModel>> response = null;

            var service = this.Service<IOrderService>();

            List<Order> orderList = null;

            try
            {
                orderList = service.GetAllOrderOfUser(userId).ToList<Order>();
            }
            catch (Exception)
            {
                response = ResponseModel<List<OrderDetailViewModel>>.CreateErrorResponse("Đơn đặt sân của bạn đã tải thất bại!", systemError);
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            
            List<OrderDetailViewModel> result = Mapper.Map<List<OrderDetailViewModel>>(orderList);

            response = new ResponseModel<List<OrderDetailViewModel>>(true, "Đơn đặt sân của bạn đã được tải thành công!", null, result);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ShowAllOrderOfPlaceOwner(String ownerId) {
            var placeService = this.Service<IPlaceService>();

            var orderService = this.Service<IOrderService>();

            var fieldService = this.Service<IFieldService>();

            ResponseModel<List<OrderViewModel>> response = null;

            try {
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

                List<OrderViewModel> result = Mapper.Map<List<OrderViewModel>>(orderList);

                response = new ResponseModel<List<OrderViewModel>>(true, "Danh sách đặt sân của bạn đã được tải thành công!", null, result);

            } catch (Exception) {
                response = ResponseModel<List<OrderViewModel>>.CreateErrorResponse("Danh sách đặt sân đã tải thất bại!", systemError);
            }


            return Json(response);


        }

        [HttpPost]
        public ActionResult ShowOrderDetail(int id)
        {
            ResponseModel<OrderDetailViewModel> response = null;

            var service = this.Service<IOrderService>();


            try
            {
                Order order = service.GetOrderById(id);

                OrderDetailViewModel result = Mapper.Map<OrderDetailViewModel>(order);

                response = new ResponseModel<OrderDetailViewModel>(true, "Order Detail", null, result);
            }

            catch (Exception)
            {
                response = ResponseModel<OrderDetailViewModel>.CreateErrorResponse("Can not load Order Detail", systemError);
            }


            return Json(response);

        }

        [HttpPost]
        public ActionResult ChangeOrderStatus(int id, int status)
        {
            ResponseModel<OrderDetailViewModel> response = null;

            var service = this.Service<IOrderService>();

            Order order = null;

            try
            {
                order = service.ChangeOrderStatus(id, status);

                OrderDetailViewModel result = Mapper.Map<OrderDetailViewModel>(order);

                response = new ResponseModel<OrderDetailViewModel>(true, "Trạng thái đơn đặt sân đã thay đổi thành công", null, result);
            }

            catch (Exception)
            {
                response = ResponseModel<OrderDetailViewModel>.CreateErrorResponse("Thay đổi trạng thái đơn đặt sân thất bại", systemError);
            }

            return Json(response);
        }

        [HttpPost]
        public ActionResult CreateOrder(OrderViewModel model)
        {
            ResponseModel<OrderDetailViewModel> response = null;

            var service = this.Service<IOrderService>();

            try
            {
                Order order = service.CreateOrder(model.UserId, model.FieldId, model.StartTime, model.EndTime, model.Note, CalculatePrice(model.FieldId,model.StartTime.TimeOfDay, model.EndTime.TimeOfDay), model.PaidType);

                OrderDetailViewModel result = Mapper.Map<OrderDetailViewModel>(order);

                response = new ResponseModel<OrderDetailViewModel>(true, "Đặt sân thành công", null, result);

            }
            catch (Exception)
            {
                response = ResponseModel<OrderDetailViewModel>.CreateErrorResponse("Đặt sân thất bại", systemError);
            }

            return Json(response);

        }

        [HttpPost]
        public ActionResult CheckInOrder(String orderCode) {
            var service = this.Service<IOrderService>();

            ResponseModel<OrderSimpleViewModel> response = null;

            try {
                Order order = service.FindOrderByCode(orderCode);

                if (order != null)
                {
                    if (order.Status == int.Parse(OrderStatus.Approved.ToString("d")))
                    {
                        service.ChangeOrderStatus(order.Id, int.Parse(OrderStatus.CheckedIn.ToString("d")));

                        OrderSimpleViewModel result = PrepareOrderSimpleViewModel(order);

                        response = new ResponseModel<OrderSimpleViewModel>(true, "Đơn đặt sân đã được checkin", null, result);

                    }
                    else if (order.Status == int.Parse(OrderStatus.Cancel.ToString("d"))) {
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
            } catch (Exception) {
                response = ResponseModel<OrderSimpleViewModel>.CreateErrorResponse("Đơn đặt sân chưa được checkin", systemError);
            }

            return Json(response);
        }

        private OrderSimpleViewModel PrepareOrderSimpleViewModel(Order order) {
            OrderSimpleViewModel result = Mapper.Map<OrderSimpleViewModel>(order);

            result.UserName = order.AspNetUser.UserName;

            result.FullName = order.AspNetUser.FullName;

            result.FieldName = order.Field.Name;

            result.PlaceName = order.Field.Place.Name;

            result.Status = Utils.GetEnumDescription((OrderStatus)order.Status);

            result.PaidType = Utils.GetEnumDescription((OrderPaidType)order.PaidType);

            return result;
        }
        private double CalculatePrice(int fieldId, TimeSpan startTime, TimeSpan endTime) {
            var timeBlockService = this.Service<ITimeBlockService>();

            return timeBlockService.calPrice(fieldId, startTime, endTime);
        }
    }

}