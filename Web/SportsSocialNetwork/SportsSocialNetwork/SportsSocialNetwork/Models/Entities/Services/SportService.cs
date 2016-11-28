using SportsSocialNetwork.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportsSocialNetwork.Models.Entities.Services
{
    public partial interface ISportService
    {
        IEnumerable<Sport> getAllSport();
        IQueryable<Sport> GetSport(JQueryDataTableParamModel request, out int totalRecord);
        String GetSportName(int id);
    }

    public partial class SportService : ISportService
    {
        public IEnumerable<Sport> getAllSport()
        {
            return this.GetActive();
        }

        public IQueryable<Sport> GetSport(JQueryDataTableParamModel request, out int totalRecord)
        {
            var filter = request.sSearch;
            var list1 = this.GetActive();

            var list = list1.Where(
                u => filter == null ||
                u.Name.ToLower().Contains(filter.ToLower()) ||
                u.Name.ToLower().Contains(filter.ToLower())
                );

            //list = list.Where(u => u.AspNetRoles.Where(r => r.Id.Equals(UserRole.Member.ToString())).Count()>0);
            totalRecord = list.Count();
            var result = list.OrderBy(u => u.Name)
                .Skip(request.iDisplayStart)
                             .Take(request.iDisplayLength);

            return result;
        }

        public String GetSportName(int id)
        {
            Sport sport = this.FirstOrDefaultActive(x => x.Id == id);
            return sport.Name;
        }
    }
}