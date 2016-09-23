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

        public void test()
        {

        }
    }
}