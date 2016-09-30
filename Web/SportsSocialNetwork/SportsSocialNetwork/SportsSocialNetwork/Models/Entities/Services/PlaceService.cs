using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportsSocialNetwork.Models.Entities.Services
{
    public partial interface IPlaceService
    {

        #region Code from here
        IEnumerable<Place> GetAll();

        Place GetPlaceById(int id);

        Place ChangeStatus(int id, int status);


        #endregion

        void test();
    }

    public partial class PlaceService: IPlaceService
    {
        #region Code from here

        public IEnumerable<Place> GetAll()
        {
            IEnumerable<Place> placeList;
            placeList = this.GetActive();
            return placeList;
        }

        public Place GetPlaceById(int id)
        {
            Place place = this.FirstOrDefault(x => x.Id == id);
            if (place != null)
            {
                return place;
            }
            return null;
        }

        public Place ChangeStatus(int id, int status)
        {
            Place place = this.FirstOrDefault(x => x.Id == id);
            this.Get();
            if (place != null)
            {
                place.Status = status;
                this.Save();
                return place;
            }
            return null;
        }

        #endregion

        public void test()
        {

        }
    }
}