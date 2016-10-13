using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace SportsSocialNetwork.Models.Utilities
{
    public class EmailSender
    {


        public static void Send(string sender, string[] recivers, string[] ccs, string[] bccs,
                         string subject, string body, bool isBodyHtml)
        {
            using (SmtpClient client = new SmtpClient())
            {

                MailMessage msg = new MailMessage();

                msg.From = new MailAddress(sender);

                foreach (var reciever in recivers)
                {
                    msg.To.Add(reciever);
                }
                if (ccs != null)
                {
                    foreach (var cc in ccs)
                    {
                        msg.CC.Add(cc);
                    }
                }


                if (bccs != null)
                {
                    foreach (var bcc in bccs)
                    {
                        msg.Bcc.Add(bcc);
                    }
                }



                msg.Subject = subject;
                msg.Body = body;
                msg.IsBodyHtml = isBodyHtml;

                client.EnableSsl = true;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential("ssncapstoneproject@gmail.com", "sportsocialnetwork");
                client.Host = "smtp.gmail.com";
                client.Port = 587;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.Send(msg);
            }
        }
    }
}