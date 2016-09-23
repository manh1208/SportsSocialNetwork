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
    
    public partial class AspNetUserViewModel : SkyWeb.DatVM.Mvc.BaseEntityViewModel<SportsSocialNetwork.Models.Entities.AspNetUser>
    {
    	
    			public virtual string Id { get; set; }
    			public virtual string Email { get; set; }
    			public virtual bool EmailConfirmed { get; set; }
    			public virtual string PasswordHash { get; set; }
    			public virtual string SecurityStamp { get; set; }
    			public virtual string PhoneNumber { get; set; }
    			public virtual bool PhoneNumberConfirmed { get; set; }
    			public virtual bool TwoFactorEnabled { get; set; }
    			public virtual Nullable<System.DateTime> LockoutEndDateUtc { get; set; }
    			public virtual bool LockoutEnabled { get; set; }
    			public virtual int AccessFailedCount { get; set; }
    			public virtual string UserName { get; set; }
    			public virtual string Address { get; set; }
    			public virtual string City { get; set; }
    			public virtual string District { get; set; }
    			public virtual string Ward { get; set; }
    			public virtual string AvatarImage { get; set; }
    			public virtual string CoverImage { get; set; }
    			public virtual Nullable<System.DateTime> Birthday { get; set; }
    			public virtual Nullable<int> Gender { get; set; }
    			public virtual System.DateTime CreateDate { get; set; }
    			public virtual bool Active { get; set; }
    	
    	public AspNetUserViewModel() : base() { }
    	public AspNetUserViewModel(SportsSocialNetwork.Models.Entities.AspNetUser entity) : base(entity) { }
    
    }
}