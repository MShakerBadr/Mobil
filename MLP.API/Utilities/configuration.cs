using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Unifonic;

namespace MLP.API.Utilities
{
    public class configuration
    {
        Random random = new Random();
        public string Sendsms(string msg, string mob, string username, string Password)
        {
            //string msg = "Verify code:111";
            //string mob = "+201116210695,+201225601715";
            string sUrl = "https://bulksms.vsms.net/eapi/submission/send_sms/2/2.0?username=" + username + "&password=" + Password + "&message=" + msg + "&msisdn=+2" + mob;
            string sResponse = GetResonse(sUrl);
            return sResponse;
        }

        public void SendSMSNew(string Msg, string Mobile)
        {
            try
            {
                var urc = new UnifonicRestClient("yPYUxPEOy00mT1KqmwshxGBESjGX9");

                var sendSmsMessageResult = urc.SendSmsMessage(Mobile, Msg);

                var Status = sendSmsMessageResult.Status;

                //var baseAddress = new Uri("http://api.unifonic.com/rest/");

                //using (var httpClient = new HttpClient { BaseAddress = baseAddress })
                //{
                //    using (var content = new StringContent("AppSid=yPYUxPEOy00mT1KqmwshxGBESjGX9&Recipient=201272526735&Body=Test", System.Text.Encoding.Default, "application/x-www-form-urlencoded"))
                //    {
                //        using (var response = await httpClient.PostAsync("undefined", content))
                //        {
                //            string responseData = await response.Content.ReadAsStringAsync();

                //        }
                //    }
                // }

                //return responseData;
                //string URL = "http://api.unifonic.com/rest/Messages/Send?AppSid=yPYUxPEOy00mT1KqmwshxGBESjGX9&Recipient=" + Mobile + "&Body=" + Msg;
                //string sResponse = GetResonse(URL);
                //return sResponse;
            }
            catch (Exception ex)
            {

                throw;
            }
        }


        public static string GetResonse(string sUrl)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(sUrl);
            request.MaximumAutomaticRedirections = 4;
            request.Credentials = CredentialCache.DefaultCredentials;
            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream recieveStream = response.GetResponseStream();
                StreamReader readStream = new StreamReader(recieveStream, Encoding.UTF8);
                string sResponse = readStream.ReadToEnd();
                response.Close();
                readStream.Close();
                return sResponse;
            }
            catch
            {
                return "";
            }
        }


        public string GenerateVerficationCode()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, 5)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public string GenerateToken(string Text)
        {
            var data = Encoding.ASCII.GetBytes(Text);
            var md5 = new MD5CryptoServiceProvider();
            var md5data = md5.ComputeHash(data);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < md5data.Length / 2; i++)
            {
                sb.Append(md5data[i].ToString("x2"));
            }

            return sb.ToString();
        }

        public static bool SendMail(string Subject, string Body, string ToMail)
        {
            MailAddress fromAddress = new MailAddress(ConfigurationManager.AppSettings["FromMail"], ConfigurationManager.AppSettings["FromMailName"]);

            string BodyToSend = Body;

            //Create the MailMessage instance 
            //MailMessage myMailMessage = new MailMessage(fromAddress, toAddress);

            MailMessage myMailMessage = new MailMessage();

            myMailMessage.From = fromAddress; //From Email Id

            string[] ToMuliArr = ToMail.Split(',');
            foreach (string ToEMail in ToMuliArr)
            {
                myMailMessage.To.Add(new MailAddress(ToEMail)); //adding multiple TO Email Id
            }

            //Assign the MailMessage's properties 
            myMailMessage.Subject = Subject;
            myMailMessage.Body = BodyToSend;
            myMailMessage.IsBodyHtml = true;


            //Create the SmtpClient object
            SmtpClient smtp = new SmtpClient();
            //smtp.EnableSsl = true;
            try
            {
                smtp.Send(myMailMessage);
                return true;
            }
            catch (Exception ex)
            {
                //Logger.WriteError(ex.ToString());
                return false;
            }
        }

        internal string GenerateNumericVerficationCode()
        {
            const string chars = "0123456789";
            return new string(Enumerable.Repeat(chars, 7)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}