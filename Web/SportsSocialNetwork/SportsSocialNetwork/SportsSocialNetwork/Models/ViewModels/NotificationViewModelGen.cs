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
    
    public partial class NotificationViewModel : SkyWeb.DatVM.Mvc.BaseEntityViewModel<SportsSocialNetwork.Models.Entities.Notification>
    {
    	
    			public virtual int Id { get; set; }
    			public virtual string UserId { get; set; }
    			public virtual string FromUserId { get; set; }
    			public virtual string Title { get; set; }
    			public virtual string Message { get; set; }
    			public virtual int Type { get; set; }
    			public virtual Nullable<int> PostId { get; set; }
    			public virtual Nullable<int> InvitationId { get; set; }
    			public virtual Nullable<int> OrderId { get; set; }
    			public virtual Nullable<System.DateTime> CreateDate { get; set; }
    			public virtual Nullable<bool> MarkRead { get; set; }
    			public virtual bool Active { get; set; }
    	
    	public NotificationViewModel() : base() { }
    	public NotificationViewModel(SportsSocialNetwork.Models.Entities.Notification entity) : base(entity) { }
    
    }
}
