
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
    

public partial interface INotificationService : SkyWeb.DatVM.Data.IBaseService<Notification>
{
}

public partial class NotificationService : SkyWeb.DatVM.Data.BaseService<Notification>, INotificationService
{
    public NotificationService(SkyWeb.DatVM.Data.IUnitOfWork unitOfWork, Repositories.INotificationRepository repository) : base(unitOfWork, repository)
    {
    }
}

}
