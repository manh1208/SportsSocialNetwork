using SportsSocialNetwork.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportsSocialNetwork.Models
{
    public class OrderDetailViewModel
    {
        public int Id { get; set; }
        public AspNetUserSimpleModel AspNetUser { get; set; }
        public int FieldId { get; set; }
        public String CreateDate { get; set; }
        public String StartTime { get; set; }
        public String EndTime { get; set; }
        public string Note { get; set; }
        public double Price { get; set; }
        public String Status { get; set; }
        public String PaidType { get; set; }
        public string Token { get; set; }
        public string OrderCode { get; set; }
        public string OnlinePaymentMethod { get; set; }
        public string BankCode { get; set; }
        public String TransactionTime { get; set; }
        public string PayerName { get; set; }
        public string PayerPhone { get; set; }
        public string PayerEmail { get; set; }
    }
}