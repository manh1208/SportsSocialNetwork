using SportsSocialNetwork.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportsSocialNetwork.Models.Entities.Services
{
    public partial interface IFieldService
    {
        #region Code from here

        IEnumerable<Field> GetFieldList(int placeId);

        Field GetFieldInfo(int id);

        Field ChangeFieldStatus(int id, int status);
        IQueryable<Field> GetField(JQueryDataTableParamModel request, out int totalRecord);

        #endregion


        void test();
    }

    public partial class FieldService: IFieldService
    {
        #region Code from here

        public IEnumerable<Field> GetFieldList(int placeId)
        {
            IEnumerable<Field> fieldList = this.GetActive(x => x.PlaceId == placeId);
            return fieldList;
        }

        public Field GetFieldInfo(int id)
        {
            Field field = this.FirstOrDefault(x => x.Id == id);
            return field;
        }

        public Field ChangeFieldStatus(int id, int status)
        {
            Field field = this.FirstOrDefault(x => x.Id == id);
            field.Status = status;
            this.Save();
            return field;
        }

        public IQueryable<Field> GetField(JQueryDataTableParamModel request, out int totalRecord)
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