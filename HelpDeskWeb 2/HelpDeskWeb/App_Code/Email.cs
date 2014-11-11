using System;
using System.Collections.Generic;
using System.Web;
using System.Net.Mail;

/// <summary>
/// Summary description for Email
/// </summary>
public class HDEmail
{
    #region Properties
    #endregion

    #region Attributes
        private MailMessage mail = new MailMessage();
        private string Displayname = "HelpDesk Web";
    #endregion

    #region Constructors
        public HDEmail(string from, string subject, string body)
        {
            mail.From = new MailAddress(from, Displayname);
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;
        }
    #endregion

    #region Member Methods
        public bool Send()
        {
            //try
            //{
                SmtpClient smtp = new SmtpClient("mail.cofo.edu");
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(mail);
                return true;
            //}
            //catch
            //{
            //    return false;
            //}
        }
        public bool Add(string address)
        {
            //try
            //{ 
                mail.To.Add(address);
                return true;
            //}
            //catch
           // { 
             //   return false;
            //}
        }
        public bool From(string address)
        {
            try
            {
                mail.From = new MailAddress(address);
                return true;
            }
            catch
            {
                return false;
            }
        }
    #endregion

    #region Static Methods



    #endregion
}