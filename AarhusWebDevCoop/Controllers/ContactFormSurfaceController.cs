using AarhusWebDevCoop.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using Umbraco.Core.Models;
using Umbraco.Web.Mvc;
namespace AarhusWebDevCoop.Controllers
{
    public class ContactFormSurfaceController : Umbraco.Web.Mvc.SurfaceController
    {

        public ContactForm contactForm;
        // GET: ContactFormSurface
        public ActionResult Index()
        {
            return PartialView("ContactUSPage", new ContactForm());
        }
        public void createContent(ContactForm model)
        {
            IContent comment = Services.ContentService.CreateContent(model.Subject, CurrentPage.Id, "Message");

            comment.SetValue("messageName",model.Name);
            comment.SetValue("email",model.Email);
            comment.SetValue("subject",model.Subject);
            comment.SetValue("messageContent",model.Message);
            //Save
            Services.ContentService.Save(comment);
            //Save and pusblish
            //Services.ContentService.SaveAndPublishWithStatus(comment);

        }
        [HttpPost]
        public ActionResult HandleFormSubmit(ContactForm contact)
        {
            if (!ModelState.IsValid)
            {
                return CurrentUmbracoPage();
            }
                MailMessage message = new MailMessage();
            message.To.Add(contact.Email);
            message.Subject = contact.Subject;
            message.From = new MailAddress(contact.Email, contact.Name);
            message.Body = contact.Message;
            using (SmtpClient smtp = new SmtpClient())
            {
                Debug.WriteLine("hello");
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = false;
                smtp.EnableSsl = true;
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.Credentials = new System.Net.NetworkCredential("bradborn133@gmail.com", "dimgyvivbvufkczs");
                // send mail
                smtp.Send(message);
                TempData["success"] = true;
                createContent(contact);
            }
                return  RedirectToCurrentUmbracoPage();
        }
    }
}