using SportsSocialNetwork.Models.Entities.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportsSocialNetwork.Models.Entities.Services
{
    public partial interface IPlaceService
    {
        IEnumerable<Place> getAllPlace();
    }
    public partial class PlaceService : IPlaceService
    {
        public IEnumerable<Place> getAllPlace()
        {
            return this.GetActive();
        }

    }


}