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

        void DeactivateUser(AspNetUser user);

        String FindUserName(String userId);

        IQueryable<AspNetUser> GetPlaceOwner(JQueryDataTableParamModel request, out int totalRecord);

        AspNetUser FindUserByUserName(string username);

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

        public void DeactivateUser(AspNetUser user)
        {
            user.Active = false;
            this.Update(user);
        }


        public String FindUserName(String userId) {
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

        #endregion

        public void test()
        {

        }
    }
}