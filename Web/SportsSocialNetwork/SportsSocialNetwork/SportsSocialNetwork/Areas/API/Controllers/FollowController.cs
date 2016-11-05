using SkyWeb.DatVM.Mvc;
using SportsSocialNetwork.Models;
using SportsSocialNetwork.Models.Entities;
using SportsSocialNetwork.Models.Entities.Services;
using SportsSocialNetwork.Models.Enumerable;
using SportsSocialNetwork.Models.Utilities;
using SportsSocialNetwork.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SportsSocialNetwork.Areas.Api.Controllers
{
    public class FollowController : BaseController
    {
        private String systemError = "Đã có lỗi xảy ra!";

        [HttpPost]
        public ActionResult FollowUnfollowUser(String userId, String followerId)
        {
            var service = this.Service<IFollowService>();

            ResponseModel<bool> response = null;

            try
            {
                bool result = service.FollowUnfollowUser(userId, followerId);

                if (result)
                {
                    response = new ResponseModel<bool>(true, "Đã theo dõi", null);
                }
                else
                {
                    response = new ResponseModel<bool>(true, "Đã bỏ theo dõi", null);
                }

            }
            catch (Exception)
            {
                response = ResponseModel<bool>.CreateErrorResponse("Theo dõi thất bại", systemError);
            }

            return Json(response);
        }

        [HttpPost]
        public ActionResult GetFollowList(string userId)
        {
            var service = this.Service<IFollowService>();

            ResponseModel<List<AspNetUserOveralViewModel>> response = null;

            try
            {
                List<Follow> followList = service.GetActive(x => x.FollowerId.Equals(userId)).ToList();

                List<AspNetUserOveralViewModel> result = new List<AspNetUserOveralViewModel>();

                if (followList != null)
                {
                    foreach(var follow in followList)
                    {


                        result.Add(PrepareAspNetUserOveralViewModel(follow));
                    }
                }

                response = new ResponseModel<List<AspNetUserOveralViewModel>>(true, "Danh sách những người bạn theo dõi:", null, result);
            }
            catch (Exception) {
                response = ResponseModel<List<AspNetUserOveralViewModel>>.CreateErrorResponse("Tải danh sách theo dõi thất bại", systemError);
            }

            return Json(response);
        }

        [HttpPost]
        public ActionResult GetPeopleFollowYou(string userId)
        {
            var service = this.Service<IFollowService>();

            ResponseModel<List<AspNetUserOveralViewModel>> response = null;

            try
            {
                List<Follow> followList = service.GetActive(x => x.UserId.Equals(userId)).ToList();

                List<AspNetUserOveralViewModel> result = new List<AspNetUserOveralViewModel>();

                if (followList != null)
                {
                    foreach (var follow in followList)
                    {


                        result.Add(PreparePeopleFollowYou(follow));
                    }
                }

                response = new ResponseModel<List<AspNetUserOveralViewModel>>(true, "Danh sách những người theo dõi bạn:", null, result);
            }
            catch (Exception)
            {
                response = ResponseModel<List<AspNetUserOveralViewModel>>.CreateErrorResponse("Tải danh sách theo dõi thất bại", systemError);
            }

            return Json(response);
        }

        private AspNetUserOveralViewModel PrepareAspNetUserOveralViewModel(Follow follow)
        {
            var followService = this.Service<IFollowService>();

            AspNetUser user = follow.AspNetUser;

            AspNetUserOveralViewModel result = Mapper.Map<AspNetUserOveralViewModel>(user);

            if (result.Hobbies != null)
            {
                var service = this.Service<ISportService>();
                foreach (var hobby in result.Hobbies)
                {

                    hobby.SportName = service.GetSportName(hobby.SportId);
                }
            }

            if (user.Gender != null)
            {
                result.Gender = Utils.GetEnumDescription((Gender)user.Gender);
            }

            if (user.Birthday != null)
            {
                result.BirthdayString = result.Birthday.ToString("dd/MM/yyyy");
            }

            result.Followed = true;

            result.FollowCount = followService.GetActive(x=> x.FollowerId.Equals(user.Id)).Count();

            result.FollowedCount = user.Follows.Where(x => x.UserId == user.Id).Count();

            result.PostCount = user.Posts.Count();


            return result;
        }

        private AspNetUserOveralViewModel PreparePeopleFollowYou(Follow follow)
        {
            var followService = this.Service<IFollowService>();

            var userService = this.Service<IAspNetUserService>();

            AspNetUser user = userService.FirstOrDefaultActive(x => x.Id.Equals(follow.FollowerId));

            AspNetUserOveralViewModel result = Mapper.Map<AspNetUserOveralViewModel>(user);

            if (result.Hobbies != null)
            {
                var service = this.Service<ISportService>();
                foreach (var hobby in result.Hobbies)
                {

                    hobby.SportName = service.GetSportName(hobby.SportId);
                }
            }

            if (user.Gender != null)
            {
                result.Gender = Utils.GetEnumDescription((Gender)user.Gender);
            }

            if (user.Birthday != null)
            {
                result.BirthdayString = result.Birthday.ToString("dd/MM/yyyy");
            }

            result.Followed = true;

            result.FollowCount = followService.GetActive(x => x.FollowerId.Equals(user.Id)).Count();

            result.FollowedCount = user.Follows.Where(x => x.UserId == user.Id).Count();

            result.PostCount = user.Posts.Count();


            return result;
        }
    }
}