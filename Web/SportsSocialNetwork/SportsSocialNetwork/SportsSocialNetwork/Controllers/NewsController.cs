using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using SportsSocialNetwork.Models.Entities;
using SkyWeb.DatVM.Mvc;
using SportsSocialNetwork.Models.Entities.Services;
using SportsSocialNetwork.Models.ViewModels;

namespace SportsSocialNetwork.Controllers
{
    public class NewsController : BaseController
    {
        // GET: News
        public ActionResult Index()
        {
            var _newsService = this.Service<INewsService>();
            var _categoryService = this.Service<ICategoryService>();

            List<Category> categories = _categoryService.GetActive().ToList();
            List<News> PopularNews = _newsService.GetPopularNews();
            Dictionary<Category, List<News>> ListNews = _newsService.GetNewsDependOnHobbies(categories);

            ViewBag.categories = categories;
            ViewBag.PopularNews = PopularNews;
            ViewBag.ListNews = ListNews;
            return View();
        }

        public ActionResult NewsDetail(int? id)
        {
            var _newsService = this.Service<INewsService>();
            var _categoryService = this.Service<ICategoryService>();

            _newsService.UpdateNumOfRead(id.Value);
            List<Category> categories = _categoryService.GetActive().ToList();
            News news = _newsService.GetNewsById(id.Value);
            NewsViewModel newsVM = Mapper.Map<NewsViewModel>(news);
            List<News> relativeNews = _newsService.GetRelativeNews(id.Value);

            ViewBag.categories = categories;
            ViewBag.relativeNews = relativeNews;

            return View(newsVM);
        }

        public ActionResult Category(int? id)
        {
            var _newsService = this.Service<INewsService>();
            var _categoryService = this.Service<ICategoryService>();

            List<Category> categories = _categoryService.GetActive().ToList();
            List<News> popularNewByCate = _newsService.GetPopularNewsByCategory(id.Value);
            List<News> newsByCate = _newsService.GetNewsByCategory(id.Value);

            ViewBag.categories = categories;
            ViewBag.popularNewByCate = popularNewByCate;
            ViewBag.newsByCate = newsByCate;
            return View();
        }

    }
}