using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IntoSport.Models;
using System.Net.Mail;
namespace IntoSport.Helpers
{
    public class MailerHelper
    {
        public MailerHelper(String body, String subject,IntoSport.Models.User user)
        {
            try
            {
                MailMessage mail = new MailMessage("IntoSport@hotmail.com", user.email);
                SmtpClient client = new SmtpClient();
                client.Port = 25;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;

                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential("IntoSport@hotmail.com", "Werkgroep6C");
                client.EnableSsl = true;
                client.Host = "smtp.live.com";
                mail.Subject = subject;
                mail.Body = body;
                client.Send(mail);
            }catch(Exception e)
            {
                
            }
           
        }
    }
}