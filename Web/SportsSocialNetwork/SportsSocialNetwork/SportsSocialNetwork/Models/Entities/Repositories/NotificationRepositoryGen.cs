
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
    

public partial interface INotificationRepository : SkyWeb.DatVM.Data.IBaseRepository<Notification>
{
}

public partial class NotificationRepository : SkyWeb.DatVM.Data.BaseRepository<Notification>, INotificationRepository
{
	public NotificationRepository(System.Data.Entity.DbContext dbContext) : base(dbContext)
    {
    }
}

}