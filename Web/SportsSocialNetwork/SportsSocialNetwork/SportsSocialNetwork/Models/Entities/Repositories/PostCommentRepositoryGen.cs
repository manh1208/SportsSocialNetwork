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
    
    
    public partial interface IPostCommentRepository : SkyWeb.DatVM.Data.IBaseRepository<PostComment>
    {
    }
    
    public partial class PostCommentRepository : SkyWeb.DatVM.Data.BaseRepository<PostComment>, IPostCommentRepository
    {
    	public PostCommentRepository(System.Data.Entity.DbContext dbContext) : base(dbContext)
        {
        }
    }
}
