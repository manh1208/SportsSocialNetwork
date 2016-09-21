using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportsSocialNetwork.Models.Entities.Services
{
    public partial interface IPlaceService {
        IEnumerable<Place> GetAll();

        Place GetPlace(int id);

        Place ChageStatus(int id,int status);
    }

    public partial class PlaceService: IPlaceService
    {
        public IEnumerable<Place> GetAll() {
            IEnumerable<Place> placeList;
            placeList= this.GetActive();
            return placeList;
        }

        public Place GetPlace(int id)
        {
            Place place = this.FirstOrDefault(x => x.Id == id);
            if (place != null)
            {
                return place;
            }
            return null;
        }

        public Place ChangeStatus(int id, int status) {
            Place place = this.FirstOrDefault(x => x.Id == id);
            if (place != null) {
                place.Status = status;
                this.Save();
                return place;
            }
            return null;
        }
    }
}