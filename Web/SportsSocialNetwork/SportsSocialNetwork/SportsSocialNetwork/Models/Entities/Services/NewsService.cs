using SportsSocialNetwork.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportsSocialNetwork.Models.Entities.Services
{
    public partial interface INewsService
    {
        #region Code from here

        IQueryable<News> GetNews(JQueryDataTableParamModel request, out int totalRecord);


        #endregion

        void test();
    }
    public partial class NewsService : INewsService
    {
        #region Code from here
        public IQueryable<News> GetNews(JQueryDataTableParamModel request, out int totalRecord)
        {
            var filter = request.sSearch;

            var list = this.GetActive().Where(
                u => filter == null ||
                u.Title.ToLower().Contains(filter.ToLower()));

            //list = list.Where(u => u.AspNetRoles.Where(r => r.Id.Equals(UserRole.Member.ToString())).Count()>0);
            totalRecord = list.Count();
            var result = list.OrderByDescending(u => u.CreateDate)
                .Skip(request.iDisplayStart)
                             .Take(request.iDisplayLength);

            return result;
        }


        #endregion

        public void test()
        {

        }
    }
}