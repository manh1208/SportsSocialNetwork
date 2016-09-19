
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
    
public partial class Event
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public Event()
    {

        this.Participations = new HashSet<Participation>();

    }


    public int Id { get; set; }

    public int Name { get; set; }

    public string CreatorId { get; set; }

    public System.DateTime StartDate { get; set; }

    public System.DateTime EndDate { get; set; }

    public int PlaceId { get; set; }

    public string Description { get; set; }

    public string Image { get; set; }

    public int Status { get; set; }

    public bool Avtive { get; set; }



    public virtual User User { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<Participation> Participations { get; set; }

}

}
