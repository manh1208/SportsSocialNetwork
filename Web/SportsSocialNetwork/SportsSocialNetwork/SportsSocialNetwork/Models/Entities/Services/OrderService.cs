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
        bool checkOrderTimeValid(int fieldId, TimeSpan startTime, TimeSpan endTime, DateTime playDate);

        Order CreateOrder(String userId, int fieldId, DateTime startTime, DateTime endTime, String note, double price, int? paidType);
        Order CheckInOrder(String orderCode);

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
                order.Status = status;
                this.Save();
                return order;
            }
            return null;
        }

        public Order CreateOrder(String userId, int fieldId, DateTime startTime, DateTime endTime, String note, double price, int? paidType)
        {
            Order order = new Order();
            order.UserId = userId;
            order.FieldId = fieldId;
            order.CreateDate = DateTime.Now;
            order.StartTime = startTime;
            order.EndTime = endTime;
            order.Note = note;
            order.Price = price;
            order.Status = 0;
            order.PaidType = paidType;
            this.Create(order);
            this.Save();
            return order;

        }

        public bool checkOrderTimeValid(int fieldId, TimeSpan startTime, TimeSpan endTime, DateTime useDate)
        {
            IEnumerable<Order> orders = this.GetActive(p => p.FieldId == fieldId).OrderBy(p => p.StartTime).ToList();
            bool isValid = true;
            foreach (var order in orders)
            {
                if (DateTime.Compare(useDate, order.StartTime.Date) == 0)
                if ((startTime >= order.StartTime.TimeOfDay && startTime < order.EndTime.TimeOfDay) ||
                    (endTime > order.StartTime.TimeOfDay && endTime <= order.EndTime.TimeOfDay) ||
                    (startTime <= order.StartTime.TimeOfDay && endTime >= order.EndTime.TimeOfDay))
                {
                    if ((startTime >= order.StartTime.TimeOfDay && startTime < order.EndTime.TimeOfDay) ||
                                        (endTime > order.StartTime.TimeOfDay && endTime <= order.EndTime.TimeOfDay) ||
                                        (startTime <= order.StartTime.TimeOfDay && endTime >= order.EndTime.TimeOfDay))
                    {
                        isValid = false;
                        break;
                    }
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
        public IEnumerable<Order> GetAllOrderByFieldId(int fieldId)
        {
            return this.GetActive(x => x.FieldId == fieldId);
        }

        public Order CheckInOrder(String orderCode) {
            Order order= this.FirstOrDefaultActive(x => x.OrderCode == orderCode);
            order.Status = 5;
            this.Update(order);
            this.Save();
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