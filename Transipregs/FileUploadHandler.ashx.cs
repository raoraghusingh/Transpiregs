using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace Transipregs
{
    /// <summary>
    /// Summary description for FileUploadHandler
    /// </summary>
    public class FileUploadHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            if (context.Request.Files.Count > 0)
            {
                HttpFileCollection files = context.Request.Files;
                string fname = string.Empty;
                HttpPostedFile file = null;
                Random r = new Random();
                int Randomnumber = r.Next(11111, 99999999);
                string extenstion = string.Empty;
                
                for (int i = 0; i < 2; i++)
                {
                    

                    if (i == 0)
                    {
                         file = files[0];
                         fname = context.Server.MapPath("~/Resume/" + file.FileName);
                        extenstion = Path.GetExtension(fname);
                        file.SaveAs(fname);
                    }

                    string Username = ConfigurationManager.AppSettings["Useremail"];
                    string Password = ConfigurationManager.AppSettings["UserPassword"];


                    string body = string.Empty;
                    string toemail = string.Empty;
                    //using streamreader for reading my htmltemplate   
                    if (i == 0)
                    {
                        using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/CareerTemplate.html")))
                        {
                            body = reader.ReadToEnd();
                            body = body.Replace("{usname}", context.Request.Form["Name"]);
                            toemail = context.Request.Form["Email"];
                        }
                    }
                    else
                    {
                        using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/Admincar.html")))
                        {
                            body = reader.ReadToEnd();
                            body = body.Replace("{usname}", context.Request.Form["Name"]);
                            toemail = "info@transpiregs.com";
                        }
                    }
                   
                    using (MailMessage mm = new MailMessage(Username, toemail))
                    {
                        //string body = "Hello, " + message
                        if(i==0)
                        { mm.Subject = "Thank you for apply"; }
                        else
                        {
                            mm.Subject = "Candidate Resume";
                        }
                        
                        mm.Body = body;

                        mm.IsBodyHtml = true;
                        if (i == 1)
                        {
                            System.Net.Mail.Attachment attachment;
                            attachment = new System.Net.Mail.Attachment(fname);
                            mm.Attachments.Add(attachment);
                        }

                        SmtpClient smtp = new SmtpClient();
                        smtp.Host = "smtp.gmail.com";
                        smtp.EnableSsl = true;
                    
                        NetworkCredential NetworkCred = new NetworkCredential(Username, Password);
                        smtp.UseDefaultCredentials = true;
                        smtp.Credentials = NetworkCred;
                        smtp.Port = 587;
                        smtp.Send(mm);
                    }
                }
               // context.Response.ContentType = "text/plain";
               // context.Response.Write("File Uploaded Successfully!");
            }


            
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}