
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
    

public partial interface IFieldImageService : SkyWeb.DatVM.Data.IBaseService<FieldImage>
{
}

public partial class FieldImageService : SkyWeb.DatVM.Data.BaseService<FieldImage>, IFieldImageService
{
    public FieldImageService(SkyWeb.DatVM.Data.IUnitOfWork unitOfWork, Repositories.IFieldImageRepository repository) : base(unitOfWork, repository)
    {
    }
}

}
