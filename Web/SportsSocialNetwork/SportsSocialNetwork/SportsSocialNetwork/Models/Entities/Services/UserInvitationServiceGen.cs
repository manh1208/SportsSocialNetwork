
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
    

public partial interface IUserInvitationService : SkyWeb.DatVM.Data.IBaseService<UserInvitation>
{
}

public partial class UserInvitationService : SkyWeb.DatVM.Data.BaseService<UserInvitation>, IUserInvitationService
{
    public UserInvitationService(SkyWeb.DatVM.Data.IUnitOfWork unitOfWork, Repositories.IUserInvitationRepository repository) : base(unitOfWork, repository)
    {
    }
}

}
