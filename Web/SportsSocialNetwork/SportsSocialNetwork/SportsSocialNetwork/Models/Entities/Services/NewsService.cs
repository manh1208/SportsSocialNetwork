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
        List<News> GetPopularNews();
        Dictionary<Category, List<News>> GetNewsDependOnHobbies(List<Category> categories);
        News GetNewsById(int id);
        List<News> GetNewsByCategory(int categoryId);
        List<News> GetPopularNewsByCategory(int categoryId);
        List<News> GetRelativeNews(int id);
        void UpdateNumOfRead(int id);

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

        public List<News> GetPopularNews()
        {
            return this.GetActive().OrderByDescending(n => n.NumOfRead).Take(11).ToList();
        }

        public Dictionary<Category, List<News>> GetNewsDependOnHobbies(List<Category> categories)
        {
            Dictionary<Category, List<News>> newsList = new Dictionary<Category, List<News>>();
            foreach (var item in categories)
            {
                List<News> news = this.GetActive(n => n.CategoryId == item.Id).OrderByDescending(n => n.NumOfRead).Take(10).ToList();
                newsList.Add(item, news);
            }
            return newsList;
        }

        public News GetNewsById(int id)
        {
            return this.FirstOrDefaultActive(n => n.Id == id);
        }

        public List<News> GetNewsByCategory(int categoryId)
        {
            return this.GetActive(n => n.CategoryId == categoryId).OrderByDescending(n => n.Id).ToList();
        }

        public List<News> GetPopularNewsByCategory(int categoryId)
        {
            return this.GetActive(n => n.CategoryId == categoryId).OrderByDescending(n => n.NumOfRead).Take(11).ToList();
        }

        public List<News> GetRelativeNews(int id)
        {
            News mainNews = this.FirstOrDefaultActive(n => n.Id ==  id);
            List<News> relativeNews = new List<News>();
            relativeNews.AddRange(this.GetActive(n => n.CategoryId == mainNews.CategoryId && n.Id < id).Take(3).ToList());
            relativeNews.AddRange(this.GetActive(n => n.CategoryId == mainNews.CategoryId && n.Id > id).Take(3).ToList());
            return relativeNews;
        }

        public void UpdateNumOfRead(int id)
        {
            News news = this.FirstOrDefaultActive(n => n.Id == id);
            if (news.NumOfRead == null)
            {
                news.NumOfRead = 1;
            }
            news.NumOfRead = news.NumOfRead + 1;
            this.Update(news);
            this.Save();
        }
        #endregion

        public void test()
        {

        }
    }
}