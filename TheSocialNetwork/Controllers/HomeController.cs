using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheSocialNetwork.DAL;
using TheSocialNetwork.Models;

namespace TheSocialNetwork.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            UserViewModel user = Facade.GetUser(User.Identity.GetUserId());
            IndexViewModel model = new IndexViewModel();
            model.LastLogin = user.LastLogin;
            model.AmountOfLogins = user.AmountOfLogins;
            model.UnreadMessages = Facade.GetAllMessages(User.Identity.GetUserId())
                .Where(m => !m.IsRead).ToList().Count;
            return View(model);
        }       
    }
}