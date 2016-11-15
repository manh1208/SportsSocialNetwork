
using SportsSocialNetwork.Models.Enumerable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportsSocialNetwork.Models.Entities.Services
{
    public partial interface IOrderService
    {
        #region Code from here
        IEnumerable<Order> GetAllOrderOfUser(String ownerId);

        Order GetOrderById(int id);

        IEnumerable<Order> GetAllOrderByFieldId(int fieldId);

        Order ChangeOrderStatus(int id, int status);

        bool checkTimeValidInOrder(int fieldId, TimeSpan startTime, TimeSpan endTime, DateTime startDate, DateTime endDate);

        Order CreateOrder(Order order);
        
        //Order CheckInOrder(String orderCode);
        
        Order FindOrderByCode(String orderCode);

        #endregion

        void test();
    }

    public partial class OrderService : IOrderService
    {
        #region Code from here
        public IEnumerable<Order> GetAllOrderOfUser(String ownerId)
        {
            return this.GetActive(x => x.UserId.Equals(ownerId));
        }

        public bool checkTimeValidInOrder(int fieldId, TimeSpan startTime, TimeSpan endTime, DateTime startDate, DateTime endDate)
        {
            IEnumerable<Order> orders = this.GetActive(p => p.FieldId == fieldId &&
            p.Status != (int)OrderStatus.Cancel && p.Status != (int)OrderStatus.Unapproved).OrderBy(p => p.StartTime).ToList();
            bool isValid = true;
            DateTime sTime = new DateTime(startDate.Year, startDate.Month, startDate.Day, startTime.Hours,
                startTime.Minutes, startTime.Seconds);
            DateTime eTime = new DateTime(endDate.Year, endDate.Month, endDate.Day, endTime.Hours,
                endTime.Minutes, endTime.Seconds);
            foreach (var order in orders)
            {
                if ((order.StartTime > sTime && order.StartTime >= eTime) || (order.EndTime <= sTime &&
                    order.EndTime < sTime))
                {
                    isValid = true;
                }
                else
                {
                    isValid = false;
                    break;
                }
            }
            if (isValid)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public Order GetOrderById(int id)
        {
            Order order = this.FirstOrDefault(x => x.Id == id);
            return order;
        }

        public Order ChangeOrderStatus(int id, int status)
        {
            Order order = this.FirstOrDefault(x => x.Id == id);
            if (order != null)
            {
                if(order.PaidType == (int)OrderPaidType.ChosePayByCash)
                {
                    order.PaidType = (int)OrderPaidType.PaidByCash;
                }
                else if(order.PaidType == (int)OrderPaidType.ChosePayOnline)
                {
                    order.PaidType = (int)OrderPaidType.PaidOnline;
                }
                order.Status = status;
                this.Save();
                return order;
            }
            return null;
        }

        public Order CreateOrder(Order o)
        {
            this.Create(o);
            this.Save();
            return o;

        }

        //public bool checkOrderTimeValid(int fieldId, TimeSpan startTime, TimeSpan endTime, DateTime useDate)
        //{
        //    IEnumerable<Order> orders = this.GetActive(p => p.FieldId == fieldId).OrderBy(p => p.StartTime).ToList();
        //    bool isValid = true;
        //    foreach (var order in orders)
        //    {
        //        if (DateTime.Compare(useDate, order.StartTime.Date) == 0)
        //        if ((startTime >= order.StartTime.TimeOfDay && startTime < order.EndTime.TimeOfDay) ||
        //            (endTime > order.StartTime.TimeOfDay && endTime <= order.EndTime.TimeOfDay) ||
        //            (startTime <= order.StartTime.TimeOfDay && endTime >= order.EndTime.TimeOfDay))
        //        {
        //            if ((startTime >= order.StartTime.TimeOfDay && startTime < order.EndTime.TimeOfDay) ||
        //                                (endTime > order.StartTime.TimeOfDay && endTime <= order.EndTime.TimeOfDay) ||
        //                                (startTime <= order.StartTime.TimeOfDay && endTime >= order.EndTime.TimeOfDay))
        //            {
        //                isValid = false;
        //                break;
        //            }
        //        }

        //    }
        //    if (isValid)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }

        //}
        public IEnumerable<Order> GetAllOrderByFieldId(int fieldId)
        {
            return this.GetActive(x => x.FieldId == fieldId);
        }

        public Order FindOrderByCode(String orderCode) {
            Order order= this.FirstOrDefaultActive(x => x.OrderCode == orderCode);

            return order;
        }

        //private float CalculatePrice(Order order)
        //{
        //    float price = order.EndTime.Hour - order.StartTime.Hour;
        //}
        #endregion

        public void test()
        {

        }
    }
}