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
    
    public partial class TimeBlockViewModel : SkyWeb.DatVM.Mvc.BaseEntityViewModel<SportsSocialNetwork.Models.Entities.TimeBlock>
    {
    	
    			public virtual int Id { get; set; }
    			public virtual int FieldId { get; set; }
    			public virtual System.TimeSpan StartTime { get; set; }
    			public virtual System.TimeSpan EndTime { get; set; }
    			public virtual double Price { get; set; }
    			public virtual bool Active { get; set; }
    	
    	public TimeBlockViewModel() : base() { }
    	public TimeBlockViewModel(SportsSocialNetwork.Models.Entities.TimeBlock entity) : base(entity) { }
    
    }
}
