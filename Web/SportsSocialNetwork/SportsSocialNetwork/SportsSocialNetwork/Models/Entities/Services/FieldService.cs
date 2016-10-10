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

        IEnumerable<Field> FindAllFieldsOfPlace(int id);

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

        public IEnumerable<Field> FindAllFieldsOfPlace(int id) {
            return this.GetActive(x => x.PlaceId == id);
        }

        #endregion

        public void test()
        {

        }
    }
}