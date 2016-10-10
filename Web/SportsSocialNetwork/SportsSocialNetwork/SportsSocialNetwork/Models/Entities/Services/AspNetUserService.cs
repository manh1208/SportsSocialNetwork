using SportsSocialNetwork.Models.Enumerable;
using SportsSocialNetwork.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportsSocialNetwork.Models.Entities.Services
{
    public partial interface IAspNetUserService
    {
        #region Code from here
        IQueryable<AspNetUser> GetUsers(JQueryDataTableParamModel request, out int totalRecord);
        AspNetUser FindUser(string Id);

        IEnumerable<AspNetUser> FindUserByName(String name, int skip, int take);

        AspNetUser UpdateUser(AspNetUser userInfo);

        void DeactivateUser(AspNetUser user);

        String FindUserName(String userId);

        String ChangeAvatar(String userId, String image);

        String ChangeCover(String userId, String image);
        #endregion

        void test();
    }
    public partial class AspNetUserService:IAspNetUserService
    {
        #region Code from here


        public IQueryable<AspNetUser> GetUsers(JQueryDataTableParamModel request, out int totalRecord)
        {
            var filter = request.sSearch;
            var list1 = this.GetActive(u=>u.Active==true);

            var list = list1.Where(
                u=>filter == null || 
                u.UserName.ToLower().Contains(filter.ToLower()) ||
                u.FullName.ToLower().Contains(filter.ToLower()) ||
                u.Email.ToLower().Contains(filter.ToLower())
                );

            //list = list.Where(u => u.AspNetRoles.Where(r => r.Id.Equals(UserRole.Member.ToString())).Count()>0);
            totalRecord = list.Count();
            var result = list.OrderBy(u=>u.FullName)
                .Skip(request.iDisplayStart)
                             .Take(request.iDisplayLength);
            
            return result;
        }

        public AspNetUser FindUser(string Id)
        {
            return this.FirstOrDefault(u => u.Id.Equals(Id));
           
        }

        public IEnumerable<AspNetUser> FindUserByName(String name, int skip, int take) {
            return GetActive(x=> x.FullName.Contains(name)).OrderBy(x => x.FullName).Skip(skip).Take(take);
        }

        public void DeactivateUser(AspNetUser user)
        {
            user.Active = false;
            this.Update(user);
        }


        public String FindUserName(String userId) {
            return this.FirstOrDefault(x => x.Id == userId).UserName;
        }

        public AspNetUser UpdateUser(AspNetUser userInfo) {
            AspNetUser user = this.FirstOrDefaultActive(x=> x.Id == userInfo.Id);
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

        public String ChangeAvatar(String userId,String image) {
            AspNetUser user = FirstOrDefaultActive(x=> x.Id==userId);
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