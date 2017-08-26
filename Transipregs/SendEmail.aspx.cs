using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Transipregs
{
    public partial class SendEmail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod]
        public static string mailsend(string Name, string phone, string emailaddress, string servicename, string message)
        {
            string Username = ConfigurationManager.AppSettings["Useremail"];
            string Password = ConfigurationManager.AppSettings["UserPassword"];


            string body = string.Empty;
            //using streamreader for reading my htmltemplate   
            using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/email.html")))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{usname}", Name);
            body = body.Replace("{Content}", message);
            body = body.Replace("{servicename}", servicename);
            using (MailMessage mm = new MailMessage(Username, emailaddress))
            {
                //string body = "Hello, " + message
                mm.Subject = "Thank you for your Inquiry";
                mm.Body = body;

                mm.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.EnableSsl = true;
                mm.Bcc.Add("info@transpiregs.com");
                NetworkCredential NetworkCred = new NetworkCredential(Username, Password);
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = NetworkCred;
                smtp.Port = 587;
                smtp.Send(mm);
                //ClientScript.RegisterStartupScript(GetType(), "alert", "alert('Email sent.');", true);
            }
            return string.Empty;
        }
    }
}