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
    
    public partial class FieldPriceViewModel : SkyWeb.DatVM.Mvc.BaseEntityViewModel<SportsSocialNetwork.Models.Entities.FieldPrice>
    {
    	
    			public virtual int Id { get; set; }
    			public virtual int FieldId { get; set; }
    			public virtual int TimeBlockId { get; set; }
    	
    	public FieldPriceViewModel() : base() { }
    	public FieldPriceViewModel(SportsSocialNetwork.Models.Entities.FieldPrice entity) : base(entity) { }
    
    }
}