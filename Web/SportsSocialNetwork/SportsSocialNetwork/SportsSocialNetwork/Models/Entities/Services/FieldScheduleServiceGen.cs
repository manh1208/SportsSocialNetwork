
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


namespace SportsSocialNetwork.Models.Entities.Services
{

using System;
    using System.Collections.Generic;
    

public partial interface IFieldScheduleService : SkyWeb.DatVM.Data.IBaseService<FieldSchedule>
{
}

public partial class FieldScheduleService : SkyWeb.DatVM.Data.BaseService<FieldSchedule>, IFieldScheduleService
{
    public FieldScheduleService(SkyWeb.DatVM.Data.IUnitOfWork unitOfWork, Repositories.IFieldScheduleRepository repository) : base(unitOfWork, repository)
    {
    }
}

}
