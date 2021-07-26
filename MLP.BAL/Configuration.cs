using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace MLP.BAL
{
    public class Configurationtool
    {
        public static bool sendmail(string Subject, string body, string frommail, string FromMailName, string ToMail)
        {
            MailAddress fromAddress = new MailAddress(frommail, FromMailName);
            // MailAddress fromAddress =new MailAddress(
            MailAddress toAddress = new MailAddress(ToMail);
            string BodyToSend = body;
            MailMessage myMailMessage = new MailMessage(fromAddress, toAddress);

            //Assign the MailMessage's properties
            myMailMessage.Subject = Subject;
            myMailMessage.IsBodyHtml = true;
            myMailMessage.Body = BodyToSend;

            //Create the SmtpClient object
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "ayeaye.arvixe.com";
            smtp.Port = 587;



            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new System.Net.NetworkCredential("a.ebada@mobilksa.com", "Reports@2017");
            //mailer.Credentials = new System.Net.NetworkCredential("yourusername", "yourpassword");
            try
            {
                smtp.Send(myMailMessage);
                return true;
            }
            catch (Exception ex)
            {
                // Logger.WriteError(ex.ToString());
                return false;
            }
        }

    }
}
