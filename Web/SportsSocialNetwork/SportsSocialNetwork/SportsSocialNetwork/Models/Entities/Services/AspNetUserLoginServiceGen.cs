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
    
    
    public partial interface IAspNetUserLoginService : SkyWeb.DatVM.Data.IBaseService<AspNetUserLogin>
    {
    }
    
    public partial class AspNetUserLoginService : SkyWeb.DatVM.Data.BaseService<AspNetUserLogin>, IAspNetUserLoginService
    {
        public AspNetUserLoginService(SkyWeb.DatVM.Data.IUnitOfWork unitOfWork, Repositories.IAspNetUserLoginRepository repository) : base(unitOfWork, repository)
        {
        }
    }
}
