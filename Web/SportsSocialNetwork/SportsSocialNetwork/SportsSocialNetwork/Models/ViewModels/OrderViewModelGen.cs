//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SportsSocialNetwork.Models.ViewModels
{
    using System;
    using System.Collections.Generic;
    
    public partial class OrderViewModel : SkyWeb.DatVM.Mvc.BaseEntityViewModel<SportsSocialNetwork.Models.Entities.Order>
    {
    	
    			public virtual int Id { get; set; }
    			public virtual string UserId { get; set; }
    			public virtual int FieldId { get; set; }
    			public virtual System.DateTime CreateDate { get; set; }
    			public virtual System.DateTime StartTime { get; set; }
    			public virtual System.DateTime EndTime { get; set; }
    			public virtual string Note { get; set; }
    			public virtual double Price { get; set; }
    			public virtual int Status { get; set; }
    			public virtual Nullable<int> PaidType { get; set; }
    			public virtual string OrderCode { get; set; }
    			public virtual Nullable<System.DateTime> TransactionTime { get; set; }
    			public virtual string PayerName { get; set; }
    			public virtual string PayerPhone { get; set; }
    			public virtual string PayerEmail { get; set; }
    	
    	public OrderViewModel() : base() { }
    	public OrderViewModel(SportsSocialNetwork.Models.Entities.Order entity) : base(entity) { }
    
    }
}
