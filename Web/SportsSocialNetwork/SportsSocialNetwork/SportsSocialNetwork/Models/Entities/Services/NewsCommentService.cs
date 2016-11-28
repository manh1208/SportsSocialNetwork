using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportsSocialNetwork.Models.Entities.Services
{
    public partial interface INewsCommentService
    {
        #region Code from here

        IEnumerable<NewsComment> GetNewsComments(int newsId, int skip, int take);

        #endregion
    }

    public partial class NewsCommentService
    {
        #region Code from here

        public IEnumerable<NewsComment> GetNewsComments(int newsId, int skip, int take)
        {
            return this.GetActive(n => n.NewsId == newsId).OrderByDescending(n => n.Id).Skip(skip).Take(take);
        }

        #endregion
    }
}