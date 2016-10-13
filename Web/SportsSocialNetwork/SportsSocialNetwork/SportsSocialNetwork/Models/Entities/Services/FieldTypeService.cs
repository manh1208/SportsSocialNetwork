using SportsSocialNetwork.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportsSocialNetwork.Models.Entities.Services
{
    public partial interface IFieldTypeService
    {
        #region Code from here

        IQueryable<FieldType> GetFieldType(JQueryDataTableParamModel request, out int totalRecord);

        #endregion

        void test();
        
    }
    public partial class FieldTypeService : IFieldTypeService
    {

        #region Code from here

        public IQueryable<FieldType> GetFieldType(JQueryDataTableParamModel request, out int totalRecord)
        {
            var filter = request.sSearch;
            var list1 = this.GetActive(u => u.Active == true);

            var list = list1.Where(
                u => filter == null ||
                u.Name.ToLower().Contains(filter.ToLower())
                );

            totalRecord = list.Count();
            var result = list.OrderBy(u => u.Name)
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