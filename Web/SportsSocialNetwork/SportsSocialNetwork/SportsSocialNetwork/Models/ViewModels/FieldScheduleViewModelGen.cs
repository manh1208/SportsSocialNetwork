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
    
    public partial class FieldScheduleViewModel : SkyWeb.DatVM.Mvc.BaseEntityViewModel<SportsSocialNetwork.Models.Entities.FieldSchedule>
    {
    	
    			public virtual int Id { get; set; }
    			public virtual int FieldId { get; set; }
    			public virtual Nullable<int> UserId { get; set; }
    			public virtual System.DateTime StartDate { get; set; }
    			public virtual System.DateTime EndDate { get; set; }
    			public virtual System.TimeSpan StartTime { get; set; }
    			public virtual System.TimeSpan EndTime { get; set; }
    			public virtual int AvailableDay { get; set; }
    			public virtual int Type { get; set; }
    			public virtual string Description { get; set; }
    			public virtual bool Active { get; set; }
    	
    	public FieldScheduleViewModel() : base() { }
    	public FieldScheduleViewModel(SportsSocialNetwork.Models.Entities.FieldSchedule entity) : base(entity) { }
    
    }
}
