
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
    

public partial interface IRatingRepository : SkyWeb.DatVM.Data.IBaseRepository<Rating>
{
}

public partial class RatingRepository : SkyWeb.DatVM.Data.BaseRepository<Rating>, IRatingRepository
{
	public RatingRepository(System.Data.Entity.DbContext dbContext) : base(dbContext)
    {
    }
}

}
