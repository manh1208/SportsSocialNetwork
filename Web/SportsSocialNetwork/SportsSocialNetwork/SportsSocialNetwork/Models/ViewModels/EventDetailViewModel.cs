using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportsSocialNetwork.Models.ViewModels
{
    public class EventDetailViewModel:EventOveralViewModel
    {
        public List<ParticipationDetailViewModel> Participations { get; set; }
        public int ParticipantCount { get; set; }
    }
}