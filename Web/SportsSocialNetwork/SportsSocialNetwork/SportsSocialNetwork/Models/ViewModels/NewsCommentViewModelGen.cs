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
    
    public partial class NewsCommentViewModel : SkyWeb.DatVM.Mvc.BaseEntityViewModel<SportsSocialNetwork.Models.Entities.NewsComment>
    {
    	
    			public virtual int Id { get; set; }
    			public virtual string UserId { get; set; }
    			public virtual int NewsId { get; set; }
    			public virtual string Comment { get; set; }
    			public virtual System.DateTime CreateDate { get; set; }
    	
    	public NewsCommentViewModel() : base() { }
    	public NewsCommentViewModel(SportsSocialNetwork.Models.Entities.NewsComment entity) : base(entity) { }
    
    }
}
