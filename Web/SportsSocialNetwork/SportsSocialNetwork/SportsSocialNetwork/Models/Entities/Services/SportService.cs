using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportsSocialNetwork.Models.Entities.Services
{
    public partial interface ISportService
    {
        IEnumerable<Sport> getAllSport();
    }

    public partial class SportService : ISportService
    {
        public IEnumerable<Sport> getAllSport()
        {
            return this.GetActive();
        }
    }
}