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
    
    public partial class FieldViewModel : SkyWeb.DatVM.Mvc.BaseEntityViewModel<SportsSocialNetwork.Models.Entities.Field>
    {
    	
    			public virtual int Id { get; set; }
    			public virtual int PlaceId { get; set; }
    			public virtual string Name { get; set; }
    			public virtual int Status { get; set; }
    			public virtual int FieldTypeId { get; set; }
    			public virtual string Description { get; set; }
    			public virtual bool Active { get; set; }
    	
    	public FieldViewModel() : base() { }
    	public FieldViewModel(SportsSocialNetwork.Models.Entities.Field entity) : base(entity) { }
    
    }
}
