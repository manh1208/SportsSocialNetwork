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

        public string updateStatusOrder(int id, int status)
        {
            var _orderSeervice = this.Service<IOrderService>();
            Order order = _orderSeervice.FirstOrDefault(o => o.Id == id);
            if(order != null)
            {
                order.Status = status;
                _orderSeervice.Update(order);
                _orderSeervice.Save();
                return "success";
            }
            return "false";
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
                case 2:
                    filteredListItems = sortDirection == "asc"
                        ? filteredListItems.OrderBy(o => o.Field.Name)
                        : filteredListItems.OrderByDescending(o => o.Field.Name);
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