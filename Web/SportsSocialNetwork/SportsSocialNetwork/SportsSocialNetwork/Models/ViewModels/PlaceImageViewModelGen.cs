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
    
    public partial class PlaceImageViewModel : SkyWeb.DatVM.Mvc.BaseEntityViewModel<SportsSocialNetwork.Models.ViewModels.PlaceImage>
    {
    	
    			public virtual int Id { get; set; }
    			public virtual int PlaceId { get; set; }
    			public virtual string Image { get; set; }
    	
    	public PlaceImageViewModel() : base() { }
    	public PlaceImageViewModel(SportsSocialNetwork.Models.ViewModels.PlaceImage entity) : base(entity) { }
    
    }
}
