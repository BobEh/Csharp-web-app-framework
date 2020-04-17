using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using CaseStudy.Utils;
using CaseStudy.Models;
using System.Collections.Generic;

namespace CaseStudy.Controllers
{
    public class TrayController : Controller
    {
        AppDbContext _db;
        public TrayController(AppDbContext context)
        {
            _db = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public ActionResult ClearTray() // clear out current tray
        {
            HttpContext.Session.Remove(SessionVariables.Tray);
            HttpContext.Session.SetString(SessionVariables.Message, "Cart Cleared");
            return Redirect("/Home");
        }
        [Route("[action]")]
        public IActionResult GetTrays()
        {
            ApplicationUser user = HttpContext.Session.Get<ApplicationUser>(SessionVariables.User);
            TrayModel model = new TrayModel(_db);
            return Ok(model.GetAll(user));
        }
        [Route("[action]/{tid:int}")]
        public IActionResult GetTrayDetails(int tid)
        {
            TrayModel model = new TrayModel(_db);
            ApplicationUser user = HttpContext.Session.Get<ApplicationUser>(SessionVariables.User);
            return Ok(model.GetTrayDetails(tid, user.Id));
        }
        public IActionResult List()
        {
            // they can't list Trays if they're not logged on
            if (HttpContext.Session.Get<ApplicationUser>(SessionVariables.User) == null)
            {
                return Redirect("/Login");
            }
            return View("List");
        }
        // Add the tray, pass the session variable info to the db
        public ActionResult AddTray()
        {
            TrayModel model = new TrayModel(_db);
            bool itemsBackOrdered = false;
            int retVal = -1;
            string retMessage = "";
            try
            {
                Dictionary<string, object> trayItems = HttpContext.Session.Get<Dictionary<string, object>>(SessionVariables.Tray);
                retVal = model.AddTray(trayItems, HttpContext.Session.Get<ApplicationUser>(SessionVariables.User), itemsBackOrdered);
                if (retVal > 0 && itemsBackOrdered)
                {
                    retMessage = "Cart " + retVal + " Created! Some items have been placed on back order";
                }
                if (retVal > 0) // Tray Added
                {
                    retMessage = "Cart " + retVal + " Created!";
                }
                else // problem
                {
                    retMessage = "Cart not added, try again later";
                }
            }
            catch (Exception ex) // big problem
            {
                retMessage = "Cart was not created, try again later! - " + ex.Message;
            }
            HttpContext.Session.Remove(SessionVariables.Tray); // clear out current tray once persisted
        HttpContext.Session.SetString(SessionVariables.Message, retMessage);
            return Redirect("/Home");
        }
    }
}