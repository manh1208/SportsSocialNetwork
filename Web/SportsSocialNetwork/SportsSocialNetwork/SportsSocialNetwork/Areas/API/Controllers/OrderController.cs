using SkyWeb.DatVM.Mvc;
using SportsSocialNetwork.Areas.Api.Models;
using SportsSocialNetwork.Areas.API.Models;
using SportsSocialNetwork.Models.Entities;
using SportsSocialNetwork.Models.Entities.Services;
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
        private String systemError = "An error has occured!";


        [HttpPost]
        public ActionResult ShowAllOrder(String ownerId)
        {
            ResponseModel<List<OrderViewModel>> response = null;

            var service = this.Service<IOrderService>();

            List<Order> orderList = null;

            try
            {
                orderList = service.GetAllOrderOfPlaceOwner(ownerId).ToList<Order>();
            }
            catch (Exception e)
            {
                response = ResponseModel<List<OrderViewModel>>.CreateErrorResponse("Your orders have failed to load!", systemError);
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            List<OrderViewModel> result = Mapper.Map<List<OrderViewModel>>(orderList);

            response = new ResponseModel<List<OrderViewModel>>(true, "Your orders have been loaded!", null, result);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ShowOrderDetail(int id)
        {
            ResponseModel<OrderViewModel> response = null;

            var service = this.Service<IOrderService>();


            try
            {
                Order order = service.GetOrderById(id);

                OrderViewModel result = Mapper.Map<OrderViewModel>(order);

                response = new ResponseModel<OrderViewModel>(true, "Order Detail", null, result);
            }

            catch (Exception e)
            {
                response = ResponseModel<OrderViewModel>.CreateErrorResponse("Can not load Order Detail", systemError);
            }


            return Json(response);

        }

        [HttpPost]
        public ActionResult ChangeOrderStatus(int id, int status)
        {
            ResponseModel<OrderViewModel> response = null;

            var service = this.Service<IOrderService>();

            Order order = null;

            try
            {
                order = service.ChangeOrderStatus(id, status);

                OrderDetailViewModel result = Mapper.Map<OrderDetailViewModel>(order);

                result.CreateDateStrings();

                response = new ResponseModel<OrderViewModel>(true, "Your order status has been changed", null, result);
            }

            catch (Exception e)
            {
                response = ResponseModel<OrderViewModel>.CreateErrorResponse("Your order status has NOT been changed", systemError);
            }

            return Json(response);
        }

        [HttpPost]
        public ActionResult CreateOrder(String userId, int fieldId, DateTime startTime, DateTime endTime, String note, float price, int paidType) {
            ResponseModel<OrderViewModel> response = null;

            var service = this.Service<IOrderService>();

            try {
                Order order = service.CreateOrder(userId, fieldId, startTime, endTime, note, price, paidType);

                OrderViewModel result = Mapper.Map<OrderViewModel>(order);

                response = new ResponseModel<OrderViewModel>(true, "Order created successfully", null, result);
                
            } catch (Exception e) {
                response = ResponseModel<OrderViewModel>.CreateErrorResponse("Your order has NOT been created", systemError);
            }

            return Json(response);

        }
    }

}