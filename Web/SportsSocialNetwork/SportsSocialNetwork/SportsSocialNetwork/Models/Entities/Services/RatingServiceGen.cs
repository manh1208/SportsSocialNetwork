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
    
    
    public partial interface IRatingService : SkyWeb.DatVM.Data.IBaseService<Rating>
    {
    }
    
    public partial class RatingService : SkyWeb.DatVM.Data.BaseService<Rating>, IRatingService
    {
        public RatingService(SkyWeb.DatVM.Data.IUnitOfWork unitOfWork, Repositories.IRatingRepository repository) : base(unitOfWork, repository)
        {
        }
    }
}
