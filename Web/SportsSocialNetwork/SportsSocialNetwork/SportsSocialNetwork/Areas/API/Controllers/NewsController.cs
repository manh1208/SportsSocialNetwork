using SkyWeb.DatVM.Mvc;
using SportsSocialNetwork.Models;
using SportsSocialNetwork.Models.Entities;
using SportsSocialNetwork.Models.Entities.Services;
using SportsSocialNetwork.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SportsSocialNetwork.Areas.Api.Controllers
{
    public class NewsController : BaseController
    {
        private String systemError = "Đã có lỗi xảy ra!";

        [HttpPost]
        public ActionResult LoadAllCategory()
        {
            var service = this.Service<ICategoryService>();

            ResponseModel<List<CategoryViewModel>> response = null;

            try
            {
                List<Category> categoryList = service.GetActive().ToList();

                List<CategoryViewModel> result = Mapper.Map<List<CategoryViewModel>>(categoryList);

                response = new ResponseModel<List<CategoryViewModel>>(true, "Danh sách danh mục:", null, result);

            }
            catch (Exception)
            {
                response = ResponseModel<List<CategoryViewModel>>.CreateErrorResponse("Không thể tải danh mục", systemError);

            }
            return Json(response);
        }

        [HttpPost]
        public ActionResult LoadNewsOfCategory(int categoryId) {
            var service = this.Service<INewsService>();

            ResponseModel<List<NewsViewModel>> response = null;

            try
            {
                List<News> newsList = service.GetActive(x=> x.CategoryId==categoryId).ToList();

                List<NewsViewModel> result = Mapper.Map<List<NewsViewModel>>(newsList);

                response = new ResponseModel<List<NewsViewModel>>(true, "Danh sách tin tức:", null, result);
            }
            catch (Exception)
            {
                response = ResponseModel<List<NewsViewModel>>.CreateErrorResponse("Không thể tải tin", systemError);
            }
            return Json(response);

        }

        [HttpPost]
        public ActionResult ShowNewsDetail(int id)
        {
            var service = this.Service<INewsService>();

            ResponseModel<NewsViewModel> response = null;

            try
            {
                News news = service.FirstOrDefaultActive(x=> x.Id == id);

                if (news.NumOfRead != null)
                {
                    news.NumOfRead++;
                }
                else
                {
                    news.NumOfRead = 1;
                }
                service.Update(news);
                service.Save();


                NewsViewModel result = Mapper.Map<NewsViewModel>(news);

                PrepareNewsViewModel(result);

                response = new ResponseModel<NewsViewModel>(true, "Chi tiết tin tức:", null, result);

            }
            catch (Exception)
            {
                response = ResponseModel<NewsViewModel>.CreateErrorResponse("Không thể tải tin", systemError);
            }
            return Json(response);
        }

        private void PrepareNewsViewModel(NewsViewModel result) {
            var cateService = this.Service<ICategoryService>();

            var userService = this.Service<IAspNetUserService>();

            result.CreateDateString = result.CreateDate.ToString("dd/MM/yyyy HH:mm:ss");

            result.CategoryName = cateService.FirstOrDefaultActive(x=> x.Id == result.CategoryId).Name;

            result.Author = userService.FirstOrDefaultActive(x => x.Id.Equals(result.UserId)).FullName;

        }
    }
}