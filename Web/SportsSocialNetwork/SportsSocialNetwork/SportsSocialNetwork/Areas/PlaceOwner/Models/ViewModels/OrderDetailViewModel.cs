﻿using SportsSocialNetwork.Models.Entities;
using SportsSocialNetwork.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportsSocialNetwork.Areas.PlaceOwner.Models.ViewModels
{
    public class OrderDetailViewModel : OrderViewModel
    {
        public Field Field { get; set; }
    }
}