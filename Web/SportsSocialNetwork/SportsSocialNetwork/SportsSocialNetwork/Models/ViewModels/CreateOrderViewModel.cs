using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportsSocialNetwork.Models.ViewModels
{
    public class CreateOrderViewModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int FieldId { get; set; }
        public String CreateDate { get; set; }
        public String PlayDate { get; set; }
        public String StartTime { get; set; }
        public String EndTime { get; set; }
        public string Note { get; set; }
        public double Price { get; set; }
        public int Status { get; set; }
        public Nullable<int> PaidType { get; set; }
        public string OrderCode { get; set; }
        public Nullable<System.DateTime> TransactionTime { get; set; }
        public string PayerName { get; set; }
        public string PayerPhone { get; set; }
        public string PayerEmail { get; set; }
        public string QRCodeUrl { get; set; }

    }
}