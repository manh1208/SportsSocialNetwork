
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
    
public partial class GroupMember
{

    public int Id { get; set; }

    public int GroupId { get; set; }

    public string UserId { get; set; }

    public bool Admin { get; set; }

    public int Status { get; set; }



    public virtual Group Group { get; set; }

    public virtual User User { get; set; }

}

}