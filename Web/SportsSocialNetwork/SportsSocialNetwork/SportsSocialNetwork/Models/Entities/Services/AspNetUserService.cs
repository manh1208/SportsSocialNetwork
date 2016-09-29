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

        void DeactivateUser(AspNetUser user);
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


        #endregion

        public void test()
        {

        }
    }
}