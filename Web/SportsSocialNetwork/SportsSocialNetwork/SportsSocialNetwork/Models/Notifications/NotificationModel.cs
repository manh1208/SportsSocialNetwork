﻿using SportsSocialNetwork.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportsSocialNetwork.Models.Notifications
{
    public class NotificationModel : NotificationCustomViewModel
    {

    }

    public class IOSNotification
    {
        public NotificationModel aps { get; set; }
    }

}