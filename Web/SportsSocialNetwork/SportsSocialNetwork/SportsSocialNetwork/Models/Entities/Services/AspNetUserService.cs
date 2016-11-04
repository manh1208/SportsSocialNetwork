using Microsoft.AspNet.Identity.Owin;
using SportsSocialNetwork.Models.Enumerable;
using SportsSocialNetwork.Models.Utilities;
using SportsSocialNetwork.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace SportsSocialNetwork.Models.Entities.Services
{
    public partial interface IAspNetUserService
    {
        #region Code from here
        IQueryable<AspNetUser> GetUsers(JQueryDataTableParamModel request, out int totalRecord);
        AspNetUser FindUser(string Id);

        IEnumerable<AspNetUser> FindUserByName(String name, int skip, int take);

        IEnumerable<AspNetUser> FindUserByName(SSNEntities context, String name, int skip, int take);

        AspNetUser UpdateUser(AspNetUser userInfo);

        void DeactivateUser(AspNetUser user);

        String FindUserName(String userId);

        IQueryable<AspNetUser> GetPlaceOwner(JQueryDataTableParamModel request, out int totalRecord);

        AspNetUser FindUserByUserName(string username);

        String ChangeAvatar(String userId, String image);

        String ChangeCover(String userId, String image);
        

        #endregion

        void test();
    }
    public partial class AspNetUserService : IAspNetUserService
    {
        #region Code from here
        

        public IQueryable<AspNetUser> GetUsers(JQueryDataTableParamModel request, out int totalRecord)
        {
            var filter = request.sSearch;
            var list1 = this.GetActive(u => u.Active == true);

            var list = list1.Where(
                u => filter == null ||
                u.UserName.ToLower().Contains(filter.ToLower()) ||
                u.FullName.ToLower().Contains(filter.ToLower()) ||
                u.Email.ToLower().Contains(filter.ToLower())
                );

            //list = list.Where(u => u.AspNetRoles.Where(r => r.Id.Equals(UserRole.Member.ToString())).Count()>0);
            totalRecord = list.Count();
            var result = list.OrderBy(u => u.FullName)
                .Skip(request.iDisplayStart)
                             .Take(request.iDisplayLength);

            return result;
        }

        public AspNetUser FindUser(string Id)
        {
            return this.FirstOrDefault(u => u.Id.Equals(Id));

        }

        public IEnumerable<AspNetUser> FindUserByName(String name, int skip, int take)
        {
            return GetActive(x => x.Email.Contains(name)||x.UserName.Contains(name)||x.FullName.Contains(name)).OrderBy(x => x.FullName).Skip(skip).Take(take);

        }
        
        public IEnumerable<AspNetUser> FindUserByName(SSNEntities context, String name, int skip, int take)
        {
            string query = string.Format("select * from AspNetUsers where Contains((FullName,Email,UserName), '\" *{0} *\"')", name);
            var list = context.AspNetUsers.SqlQuery(query).Where(u => u.Active == true).Skip(skip).Take(take);
            return list;

        }

        public void DeactivateUser(AspNetUser user)
        {
            user.Active = false;
            this.Update(user);
        }


        public String FindUserName(String userId)
        {
            return this.FirstOrDefault(x => x.Id == userId).UserName;
        }

        public IQueryable<AspNetUser> GetPlaceOwner(JQueryDataTableParamModel request, out int totalRecord)
        {
            var filter = request.sSearch;
            var list1 = this.GetActive(u => u.Active == true);
            List<AspNetUser> users = new List<AspNetUser>();
            foreach (var item in list1)
            {
                if (item.AspNetRoles.FirstOrDefault().Id.Equals(UserRole.PlaceOwner.ToString("d")))
                {
                    users.Add(item);
                }
            }
            var list = users.AsQueryable().Where(
                u => filter == null ||
                u.UserName.ToLower().Contains(filter.ToLower()) ||
                u.FullName.ToLower().Contains(filter.ToLower()) ||
                u.Email.ToLower().Contains(filter.ToLower())
                );

            //list = list.Where(u => u.AspNetRoles.Where(r => r.Id.Equals(UserRole.Member.ToString())).Count()>0);
            totalRecord = list.Count();
            var result = list.OrderBy(u => u.FullName)
                .Skip(request.iDisplayStart)
                             .Take(request.iDisplayLength);

            return result;
        }

        public AspNetUser FindUserByUserName(string username)
        {
            return this.FirstOrDefaultActive(u => u.UserName.Equals(username));
        }

        public AspNetUser UpdateUser(AspNetUser userInfo)
        {
            AspNetUser user = this.FirstOrDefaultActive(x => x.Id == userInfo.Id);
            user.Birthday = userInfo.Birthday;
            user.City = userInfo.City;
            user.District = userInfo.District;
            user.Ward = userInfo.Ward;
            user.Email = userInfo.Email;
            user.Gender = userInfo.Gender;
            user.FullName = userInfo.FullName;
            user.Address = userInfo.Address;
            user.PhoneNumber = userInfo.PhoneNumber;
            this.Update(user);
            return user;
        }

        public String ChangeAvatar(String userId, String image)
        {
            AspNetUser user = FirstOrDefaultActive(x => x.Id == userId);
            user.AvatarImage = image;
            this.Update(user);
            return user.AvatarImage;
        }

        public String ChangeCover(String userId, String image)
        {
            AspNetUser user = FirstOrDefaultActive(x => x.Id == userId);
            user.CoverImage = image;
            this.Update(user);
            return user.CoverImage;
        }


        #endregion

        public void test()
        {

        }
    }
}