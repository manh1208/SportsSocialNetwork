using SkyWeb.DatVM.Mvc;
using SportsSocialNetwork.Models.Entities;
using SportsSocialNetwork.Models.Entities.Services;
using SportsSocialNetwork.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Teek.Models;
using Microsoft.AspNet.Identity;
using AutoMapper.QueryableExtensions;

namespace SportsSocialNetwork.Areas.Moderator.Controllers
{
    public class ArticleController : BaseController
    {
        private String userImagePath = "Article";
        // GET: Moderator/Article
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult IndexList(JQueryDataTableParamModel request)
        {
            var service = this.Service<INewsService>();
            var totalRecord = 0;
            var count = 1;

            var results = service.GetNews(request, out totalRecord)
                .AsEnumerable()
                .Select(a => new IConvertible[] {
                        count++,
                        a.Title,
                        a.CreateDate.ToString("dd-MM-yyyy HH:MM:ss"),
                        a.Image,
                        a.NumOfRead,
                        a.Id
                }).ToArray();

            var model = new
            {
                draw = request.sEcho,
                data = results,
                recordsFiltered = totalRecord,
                recordsTotal = totalRecord
            };
            return Json(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            var model = new NewsViewModel();

            var cateService = this.Service<ICategoryService>();
            model.Categories = cateService.GetActive().ProjectTo<CategoryViewModel>(MapperConfig);

            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult Create(NewsViewModel model, HttpPostedFileBase ImageFile)
        {
            var result = new AjaxOperationResult();
            if (ModelState.IsValid)
            {
                var service = this.Service<INewsService>();
                var uploader = new FileUploader();

                model.CreateDate = DateTime.Now;
                model.UserId = User.Identity.GetUserId();
                if (ImageFile != null)
                {
                    model.Image = uploader.UploadImage(ImageFile, userImagePath);
                }
                var entity = model.ToEntity();
                service.Create(entity);
            }else
            {
                result.Succeed = false;
            }
            return Json(result);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var service = this.Service<INewsService>();
            var news = service.GetActive(q => q.Id == id).SingleOrDefault();
            if (news != null)
            {
                var model = new NewsViewModel(news);
                model.Categories = (this.Service<ICategoryService>()).GetActive().ProjectTo<CategoryViewModel>(MapperConfig);
                return View(model);
            }
            else
            {
                return View(new NewsViewModel());
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult Edit(NewsViewModel model, HttpPostedFileBase ImageFile)
        {
            var result = new AjaxOperationResult();
            if (ModelState.IsValid)
            {
                var service = this.Service<INewsService>();
                var news = service.Get(q => q.Id == model.Id).SingleOrDefault();
                if (news != null)
                {
                    var uploader = new FileUploader();

                    if (ImageFile != null)
                    {
                        news.Image = uploader.UploadImage(ImageFile, userImagePath);
                    }
                    news.Title = model.Title;
                    news.NewsContent = model.NewsContent;
                    news.CategoryId = model.CategoryId;

                    service.Update(news);
                }
                else
                {
                    result.Succeed = false;
                }
            }
            else
            {
                result.Succeed = false;
            }
            return Json(result);
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            var result = new AjaxOperationResult();
            var service = this.Service<INewsService>();
            var news = service.Get(id);
            if (news != null)
            {
                news.Active = false;
                service.Update(news);
            }
            return Json(result);
        }


        [HttpGet]
        public ActionResult Preview(int id)
        {
            var service = this.Service<INewsService>();
            var news = service.Get(id);
            if (news != null)
            {
                return View(new NewsViewModel(news));
            }
            else
            {
                return View(new NewsViewModel());
            }
        }
    }
}