using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using SportsSocialNetwork.Models.ViewModels;

namespace SportsSocialNetwork.Models.Hubs
{
    public class RealTimeHub : Hub
    {
        public void Notify(string userId, NotificationCustomViewModel noti)
        {
            Clients.User(userId).send(noti);
        }
    }
}