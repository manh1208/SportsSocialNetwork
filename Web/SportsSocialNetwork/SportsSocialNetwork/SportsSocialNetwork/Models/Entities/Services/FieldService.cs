using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SportsSocialNetwork.Models.Enumerable;

namespace SportsSocialNetwork.Models.Entities.Services
{
    public partial interface IFieldService
    {
        #region Code from here

        IEnumerable<Field> GetFieldList(int placeId);

        Field GetFieldInfo(int id);

        Field ChangeFieldStatus(int id, int status);

        void saveField(Field field);

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

        public void saveField(Field field)
        {
            Field searchField = this.FirstOrDefault(f => f.Id == field.Id);

            if(searchField == null)
            {
                field.Status = (int)PlaceStatus.Active;
                this.Create(field);
                this.Save();
            }
            else
            {
                searchField.Name = field.Name;
                searchField.FieldTypeId = field.FieldTypeId;
                searchField.Description = field.Description;
                searchField.Status = field.Status;
                this.Update(searchField);
                this.Save();
            }
        }

        #endregion

        public void test()
        {

        }
    }
}