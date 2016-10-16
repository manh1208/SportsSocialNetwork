using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SportsSocialNetwork.Models.ViewModels
{
    public class MemberOrderViewModel : OrderViewModel
    {
        
        public override int Id { get; set; }
        public override string UserId { get; set; }
        public override int FieldId { get; set; }

        [Required(ErrorMessage = "{0} không được để trống")]
        public override System.DateTime CreateDate { get; set; }

        [Required(ErrorMessage = "{0} không được để trống")]
        public override System.DateTime StartTime { get; set; }

        [Required(ErrorMessage = "{0} không được để trống")]
        public override System.DateTime EndTime { get; set; }
        public override string Note { get; set; }
        public override double Price { get; set; }
        public override int Status { get; set; }
        public override Nullable<int> PaidType { get; set; }
        public override string OrderCode { get; set; }
        public override Nullable<System.DateTime> TransactionTime { get; set; }

        [Required(ErrorMessage = "{0} không được để trống")]
        public override string PayerName { get; set; }

        [Required(ErrorMessage = "{0} không được để trống")]
        public override string PayerPhone { get; set; }

        [Required(ErrorMessage = "{0} không được để trống")]
        [EmailAddress(ErrorMessage = "{0} không đúng định dạng")]
        public override string PayerEmail { get; set; }
    }
}