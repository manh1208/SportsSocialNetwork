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
    
    
    public partial interface ILikeService : SkyWeb.DatVM.Data.IBaseService<Like>
    {
    }
    
    public partial class LikeService : SkyWeb.DatVM.Data.BaseService<Like>, ILikeService
    {
        public LikeService(SkyWeb.DatVM.Data.IUnitOfWork unitOfWork, Repositories.ILikeRepository repository) : base(unitOfWork, repository)
        {
        }
    }
}
