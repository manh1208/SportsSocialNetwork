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
    
    public partial class PostSportViewModel : SkyWeb.DatVM.Mvc.BaseEntityViewModel<SportsSocialNetwork.Models.Entities.PostSport>
    {
    	
    			public virtual int Id { get; set; }
    			public virtual int PostId { get; set; }
    			public virtual int SportId { get; set; }
    	
    	public PostSportViewModel() : base() { }
    	public PostSportViewModel(SportsSocialNetwork.Models.Entities.PostSport entity) : base(entity) { }
    
    }
}