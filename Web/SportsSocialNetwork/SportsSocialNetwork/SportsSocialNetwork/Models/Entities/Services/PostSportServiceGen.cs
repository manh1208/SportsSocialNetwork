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
    
    
    public partial interface IPostSportService : SkyWeb.DatVM.Data.IBaseService<PostSport>
    {
    }
    
    public partial class PostSportService : SkyWeb.DatVM.Data.BaseService<PostSport>, IPostSportService
    {
        public PostSportService(SkyWeb.DatVM.Data.IUnitOfWork unitOfWork, Repositories.IPostSportRepository repository) : base(unitOfWork, repository)
        {
        }
    }
}
