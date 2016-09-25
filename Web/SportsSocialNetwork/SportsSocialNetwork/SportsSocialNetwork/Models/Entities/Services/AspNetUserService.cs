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


        #endregion 

        void test();
    }
    public partial class AspNetUserService:IAspNetUserService
    {
        #region Code from here

        public IQueryable<AspNetUser> GetUsers(JQueryDataTableParamModel request, out int totalRecord)
        {
            var filter = request.sSearch;
            var list1 = this.GetActive();

            var list = this.GetActive(
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


        #endregion

        public void test()
        {

        }
    }
}