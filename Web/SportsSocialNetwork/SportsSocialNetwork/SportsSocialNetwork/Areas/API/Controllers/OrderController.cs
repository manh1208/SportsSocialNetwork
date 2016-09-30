using SkyWeb.DatVM.Mvc;
using SportsSocialNetwork.Models;
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
            ResponseModel<List<OrderDetailViewModel>> response = null;

            var service = this.Service<IOrderService>();

            List<Order> orderList = null;

            try
            {
                orderList = service.GetAllOrderOfPlaceOwner(ownerId).ToList<Order>();
            }
            catch (Exception e)
            {
                response = ResponseModel<List<OrderDetailViewModel>>.CreateErrorResponse("Your orders have failed to load!", systemError);
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            
            List<OrderDetailViewModel> result = Mapper.Map<List<OrderDetailViewModel>>(orderList);

            foreach (var o in result) {
                o.CreateDateStrings();
            }

            response = new ResponseModel<List<OrderDetailViewModel>>(true, "Your orders have been loaded!", null, result);

            return Json(response, JsonRequestBehavior.AllowGet);
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

                result.CreateDateStrings();

                response = new ResponseModel<OrderDetailViewModel>(true, "Order Detail", null, result);
            }

            catch (Exception e)
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

                result.CreateDateStrings();

                response = new ResponseModel<OrderDetailViewModel>(true, "Your order status has been changed", null, result);
            }

            catch (Exception e)
            {
                response = ResponseModel<OrderDetailViewModel>.CreateErrorResponse("Your order status has NOT been changed", systemError);
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
                Order order = service.CreateOrder(model.UserId, model.FieldId, model.StartTime, model.EndTime, model.Note, model.Price, model.PaidType);

                OrderDetailViewModel result = Mapper.Map<OrderDetailViewModel>(order);

                result.CreateDateStrings();

                response = new ResponseModel<OrderDetailViewModel>(true, "Order created successfully", null, result);

            }
            catch (Exception e)
            {
                response = ResponseModel<OrderDetailViewModel>.CreateErrorResponse("Your order has NOT been created", systemError);
            }

            return Json(response);

        }


    }

}