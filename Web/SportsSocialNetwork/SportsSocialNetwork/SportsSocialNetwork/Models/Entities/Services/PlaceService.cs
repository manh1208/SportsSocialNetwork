using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportsSocialNetwork.Models.Entities.Services
{
    public partial interface IPlaceService
    {

        #region Code from here
        IEnumerable<Place> getAllPlace();


        #endregion
        IEnumerable<Place> getPlace(string sport, string province, string district);
        void test();
    }

    public partial class PlaceService: IPlaceService
    {
        #region Code from here
        public IEnumerable<Place> getAllPlace()
        {
            return this.GetActive();
        }


        #endregion
        public IEnumerable<Place> getPlace(string sport, string province, string district)
        {
            IEnumerable<Place> places = new List<Place>();
            if (sport != null && sport != "")
            {
                var sportId = Int32.Parse(sport);
                if (province != null && province != "")
                {
                    if (district != null && district != "")
                    {
                        places = this.Get(p => p.Fields.Where(f => f.FieldType.SportId == sportId).ToList().Count > 0 && p.City == province && p.District == district).ToList();
                    }
                    else
                    {
                        places = this.Get(p => p.Fields.Where(f => f.FieldType.SportId == sportId).ToList().Count > 0 && p.City == province).ToList();
                    }
                }
                else
                {
                    places = this.Get(p => p.Fields.Where(f => f.FieldType.SportId == sportId).ToList().Count > 0).ToList();
                }
            }
            else
            {
                if (province != null && province != "")
                {
                    if (district != null && district != "")
                    {
                        places = this.Get(p => p.City == province && p.District == district).ToList();
                    }
                    else
                    {
                        places = this.Get(p => p.City == province).ToList();
                    }
                }
                else
                {
                    places = this.getAllPlace();
                }
            }
            
            return places;
        }
        public void test()
        {

        }
    }
}