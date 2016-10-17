using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportsSocialNetwork.Models.ViewModels
{
    public class ParticipationDetailViewModel: ParticipationViewModel
    {
        public AspNetUserSimpleModel AspNetUser { get; set; }
        public int ParticipantCount { get; set; }
    }
}