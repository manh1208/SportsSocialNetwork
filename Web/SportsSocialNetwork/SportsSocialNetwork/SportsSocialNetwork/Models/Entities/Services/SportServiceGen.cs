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
    
    
    public partial interface ISportService : SkyWeb.DatVM.Data.IBaseService<Sport>
    {
    }
    
    public partial class SportService : SkyWeb.DatVM.Data.BaseService<Sport>, ISportService
    {
        public SportService(SkyWeb.DatVM.Data.IUnitOfWork unitOfWork, Repositories.ISportRepository repository) : base(unitOfWork, repository)
        {
        }
    }
}
