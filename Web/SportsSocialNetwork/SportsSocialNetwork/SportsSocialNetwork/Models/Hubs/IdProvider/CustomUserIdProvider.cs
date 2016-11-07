using Microsoft.AspNet.SignalR;
using SportsSocialNetwork.Controllers;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportsSocialNetwork.Models.Hubs.IdProvider
{
    public class CustomUserIdProvider : IUserIdProvider
    {
        public string GetUserId(IRequest request)
        {
            NotificationController controller = new NotificationController();

            return controller.GetUserId(request.User.Identity.GetUserId());
        }
    }
}