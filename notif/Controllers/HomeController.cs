using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using PusherServer;
using System.Net;

namespace notif.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Views Dot Net | A pusher - .Net Tutorial";

            ViewData["CurrentTime"] = DateTime.Now.ToString();

            var visitors = 0;

            if(System.IO.File.Exists("visitors.txt"))
            {
                string noOfVisitors = System.IO.File.ReadAllText("visitors.txt");
                visitors = Int32.Parse(noOfVisitors);
            }
            

            ++visitors;

            var visit_text = (visitors == 1) ? " view" : " views";
            System.IO.File.WriteAllText("visitors.txt", visitors.ToString());

			var options = new PusherOptions();
			options.Cluster = "PUSHER_APP_CLUSTER";


			var pusher = new Pusher(
			"PUSHER_APP_ID",
			"PUSHER_APP_KEY",
			"PUSHER_APP_SECRET", options);

			var result =  pusher.TriggerAsync(
			"general",
			"newVisit",
            new { visits = visitors.ToString(), message = visit_text });

            ViewData["visitors"] = visitors;
            ViewData["visitors_txt"] = visit_text;


            return View();
        }

    }
}
