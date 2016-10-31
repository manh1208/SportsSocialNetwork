using SkyWeb.DatVM.Mvc;
using SportsSocialNetwork.Models.Entities;
using SportsSocialNetwork.Models.Entities.Services;
using SportsSocialNetwork.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace SportsSocialNetwork.Controllers
{
    public class UserController : BaseController
    {
        // GET: User
        public ActionResult Index(string id)
        {
            var _userService = this.Service<IAspNetUserService>();
            AspNetUser user = _userService.FindUser(id);
            AspNetUserFullInfoViewModel userVM = Mapper.Map<AspNetUserFullInfoViewModel>(user);
            this.PrepareUserInfo(userVM);
            return View(userVM);
        }

        public void PrepareUserInfo(AspNetUserFullInfoViewModel p)
        {
            var _followService = this.Service<IFollowService>();
            var _postService = this.Service<IPostService>();

            //number that this person following
            p.FollowCount = _followService.GetFollowingCount(p.Id);
            //number of people that follow this person
            p.FollowedCount = _followService.GetFollowerCount(p.Id);

            p.Followed = _followService.FollowUnfollowUser(p.Id, User.Identity.GetUserId());

            if(User.Identity.GetUserId().Equals(p.Id))
            {
                p.isOwner = true;
            }
            else
            {
                p.isOwner = false;
            }

            p.PostCount = _postService.GetPostCountOfUser(p.Id);
        }
    }
}