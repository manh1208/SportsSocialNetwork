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
    
    public partial class PostViewModel : SkyWeb.DatVM.Mvc.BaseEntityViewModel<SportsSocialNetwork.Models.ViewModels.Post>
    {
    	
    			public virtual int Id { get; set; }
    			public virtual string UserId { get; set; }
    			public virtual System.DateTime CreateDate { get; set; }
    			public virtual string PostContent { get; set; }
    			public virtual Nullable<System.DateTime> EditDate { get; set; }
    			public virtual string Image { get; set; }
    			public virtual bool Active { get; set; }
    			public virtual Nullable<int> GroupId { get; set; }
    	
    	public PostViewModel() : base() { }
    	public PostViewModel(SportsSocialNetwork.Models.ViewModels.Post entity) : base(entity) { }
    
    }
}
