
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


namespace SportsSocialNetwork.Models.Entities
{

using System;
    using System.Collections.Generic;
    
public partial class UserInvitation
{

    public int Id { get; set; }

    public int InvitationId { get; set; }

    public string ReciverId { get; set; }

    public Nullable<bool> Accept { get; set; }



    public virtual Invitation Invitation { get; set; }

    public virtual User User { get; set; }

}

}
