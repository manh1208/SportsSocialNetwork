using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportsSocialNetwork.Models.ViewModels
{
    public class ChallengeDetailViewModel : ChallengeViewModel
    {
        public GroupViewModel Group { get; set; }
        public GroupViewModel Group1 { get; set; }
    }
}