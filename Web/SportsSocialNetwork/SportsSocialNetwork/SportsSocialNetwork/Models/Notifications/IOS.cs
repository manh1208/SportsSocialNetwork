using Newtonsoft.Json.Linq;
using PushSharp.Apple;
using SportsSocialNetwork.Models.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace HenchmenWeb.Models.Notifications
{
    public class IOS
    {
        public static bool Notify(IEnumerable<string> registrationIds, List<string> error, NotificationModel model)
        {
            var succeed = false;

            var cert = System.Web.HttpContext.Current.Server.MapPath(WebConfigurationManager.AppSettings["CertFile"]);
            // Configuration (NOTE: .pfx can also be used here)
            var config = new ApnsConfiguration(ApnsConfiguration.ApnsServerEnvironment.Sandbox,
                cert, " ");
            // Create a new broker
            var apnsBroker = new ApnsServiceBroker(config);

            // Wire up events
            #region OnNotificationFailed
            apnsBroker.OnNotificationFailed += (notification, aggregateEx) =>
            {

                aggregateEx.Handle(ex =>
                {

                    // See what kind of exception it was to further diagnose
                    if (ex is ApnsNotificationException)
                    {
                        var notificationException = (ApnsNotificationException)ex;

                        // Deal with the failed notification
                        var apnsNotification = notificationException.Notification;
                        var statusCode = notificationException.ErrorStatusCode;

                        error.Add($"Apple Notification Failed: ID={apnsNotification.Identifier}, Code={statusCode}");

                    }
                    else
                    {
                        // Inner exception might hold more useful information like an ApnsConnectionException           
                        error.Add($"Apple Notification Failed for some unknown reason : {ex.InnerException}");
                    }

                    // Mark it as handled
                    return true;
                });
            };
            #endregion

            apnsBroker.OnNotificationSucceeded += (notification) =>
            {
                succeed = true;
            };

            // Start the broker
            apnsBroker.Start();

            foreach (var deviceToken in registrationIds)
            {
                // Queue a notification to send
                apnsBroker.QueueNotification(new ApnsNotification
                {
                    DeviceToken = deviceToken.Replace(" ", "").Replace("<", "").Replace(">", ""),
                    Payload = JObject.FromObject(new IOSNotification
                    {
                        aps = model
                    })
                });
            }

            // Stop the broker, wait for it to finish   
            // This isn't done after every message, but after you're
            // done with the broker
            apnsBroker.Stop();

            return succeed;
        }
    }
}