using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace SportsSocialNetwork.Models.Hubs
{
    public class RealTimeHub : Hub
    {
        public void notifyComment(PostCommentDetailViewModel comment)
        {
            Clients.Others.broadcastMessage(comment);
        }
    }
}