using SportsSocialNetwork.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportsSocialNetwork.Models.Entities.Services
{
    public partial interface IParticipationService
    {
        #region Code from here
        IQueryable<Participation> GetPaticipation(JQueryDataTableParamModel request, out int totalRecord);

        #endregion

        void test();
    }
    public partial class ParticipationService: IParticipationService
    {
        #region Code from here
        public IQueryable<Participation> GetPaticipation(JQueryDataTableParamModel request, out int totalRecord)
        {
            var filter = request.sSearch;
            var list1 = this.GetActive();

            var list = list1.Where(
                u => filter == null ||
                u.TeamName.ToLower().Contains(filter.ToLower()) ||
                u.TeamName.ToLower().Contains(filter.ToLower())
                );

            //list = list.Where(u => u.AspNetRoles.Where(r => r.Id.Equals(UserRole.Member.ToString())).Count()>0);
            totalRecord = list.Count();
            var result = list.OrderBy(u => u.TeamName)
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