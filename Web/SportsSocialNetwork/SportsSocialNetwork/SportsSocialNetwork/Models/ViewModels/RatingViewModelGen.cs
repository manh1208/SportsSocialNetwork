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
    
    public partial class RatingViewModel : SkyWeb.DatVM.Mvc.BaseEntityViewModel<SportsSocialNetwork.Models.Entities.Rating>
    {
    	
    			public virtual int Id { get; set; }
    			public virtual string UserId { get; set; }
    			public virtual int PlaceId { get; set; }
    			public virtual int Point { get; set; }
    	
    	public RatingViewModel() : base() { }
    	public RatingViewModel(SportsSocialNetwork.Models.Entities.Rating entity) : base(entity) { }
    
    }
}
