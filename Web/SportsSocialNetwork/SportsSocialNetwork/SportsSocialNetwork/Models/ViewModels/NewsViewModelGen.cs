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
    
    public partial class NewsViewModel : SkyWeb.DatVM.Mvc.BaseEntityViewModel<SportsSocialNetwork.Models.Entities.News>
    {
    	
    			public virtual int Id { get; set; }
    			public virtual string UserId { get; set; }
    			public virtual System.DateTime CreateDate { get; set; }
    			public virtual string Title { get; set; }
    			public virtual string NewsContent { get; set; }
    			public virtual string Image { get; set; }
    			public virtual int CategoryId { get; set; }
    			public virtual Nullable<bool> Active { get; set; }
    	
    	public NewsViewModel() : base() { }
    	public NewsViewModel(SportsSocialNetwork.Models.Entities.News entity) : base(entity) { }
    
    }
}
