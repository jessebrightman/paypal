using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Assignment1_WAS.Models;
using PagedList;

namespace Assignment1_WAS.Controllers
{
    public class HomeController : Controller
    {
        ticketsEntities db = new ticketsEntities();
            
        // GET: Home
        public ActionResult Index()
        {
            var events = db.Events.FirstOrDefault();
            return View(events);
        }

        public ActionResult PayPal()
        {
            PayPal_IPN paypalResponse = new PayPal_IPN("test");

            if (paypalResponse.TXN_ID != null)
            {
                IPN ipn = new IPN();
                ipn.transactionID = paypalResponse.TXN_ID;
                ipn.firstName = paypalResponse.PayerFirstName;
                ipn.lastName = paypalResponse.PayerLastName;
                ipn.buyerEmail = paypalResponse.PayerEmail;
                ipn.txTime = DateTime.Now;
                ipn.custom = System.Web.HttpContext.Current.Session.SessionID;
                ipn.ticketQty = paypalResponse.Quantity;
                decimal amount = Convert.ToDecimal(paypalResponse.PaymentGross);
                ipn.amount = amount;            
                ipn.paymentStatus = paypalResponse.PaymentStatus;
                db.IPNs.Add(ipn);
                db.SaveChanges();
            }

            return RedirectToAction("ThankYou");
        }
        public ActionResult ThankYou()
        {
            return View();
        }

        public ActionResult ViewAttendees(int? page)
        {
            var attendees = db.IPNs;
            return View(attendees);           
        }
    }
}