
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
    

public partial interface INewsService : SkyWeb.DatVM.Data.IBaseService<News>
{
}

public partial class NewsService : SkyWeb.DatVM.Data.BaseService<News>, INewsService
{
    public NewsService(SkyWeb.DatVM.Data.IUnitOfWork unitOfWork, Repositories.INewsRepository repository) : base(unitOfWork, repository)
    {
    }
}

}