
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


namespace SportsSocialNetwork.Models.Entities.Repositories
{

using System;
    using System.Collections.Generic;
    

public partial interface IFieldScheduleRepository : SkyWeb.DatVM.Data.IBaseRepository<FieldSchedule>
{
}

public partial class FieldScheduleRepository : SkyWeb.DatVM.Data.BaseRepository<FieldSchedule>, IFieldScheduleRepository
{
	public FieldScheduleRepository(System.Data.Entity.DbContext dbContext) : base(dbContext)
    {
    }
}

}