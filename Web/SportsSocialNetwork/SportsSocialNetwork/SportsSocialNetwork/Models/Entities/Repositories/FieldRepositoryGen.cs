
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
    

public partial interface IFieldRepository : SkyWeb.DatVM.Data.IBaseRepository<Field>
{
}

public partial class FieldRepository : SkyWeb.DatVM.Data.BaseRepository<Field>, IFieldRepository
{
	public FieldRepository(System.Data.Entity.DbContext dbContext) : base(dbContext)
    {
    }
}

}