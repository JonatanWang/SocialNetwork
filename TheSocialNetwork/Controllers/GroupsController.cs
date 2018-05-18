using Microsoft.AspNet.Identity;
using System.Data;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web.Routing;
using TheSocialNetwork.DAL;
using TheSocialNetwork.Models;
using TheSocialNetwork.Models.Groups;

namespace TheSocialNetwork.Controllers
{
    public class GroupsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Groups
        public ActionResult Index()
        {
            GroupIndexViewModel model = new GroupIndexViewModel();
            model.Groups = Facade.GetAllGroups();
            return View(model);
        }

        // GET: Groups/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Group group = db.Groups.Find(id);
            GroupDetailViewModel model = new GroupDetailViewModel();
            if (group == null)
            {
                return HttpNotFound();
            }

            model.Id = group.ID;
            //model.IsCreator = group.CreatorID==User.Identity.GetUserId();
            model.Name = group.Name;
            model.Date = group.CreatedDate;
            model.Members = group.Receviers.Select(g => g.UserName).ToList();

            return View(model);
        }


        // POST: Groups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Join")]
        [ValidateAntiForgeryToken]
        public ActionResult Join(int id)
        {
            if (ModelState.IsValid)
            {
                if(Facade.JoinGroup(User.Identity.GetUserId(), id)) { 
                    TempData["message"] = string.Format("Successfully joined group!");
                    return RedirectToAction("Details", new RouteValueDictionary(
                                            new { controller = "Groups", action = "Details", Id = id }));
                }
            }
            return RedirectToAction("Index");
        }

        // POST: Groups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Leave")]
        [ValidateAntiForgeryToken]
        public ActionResult Leave(int id)
        {
            if (ModelState.IsValid)
            {
                if (Facade.LeaveGroup(User.Identity.GetUserId(), id))
                {
                    TempData["message"] = string.Format("Successfully left group!");
                    return RedirectToAction("Details", new RouteValueDictionary(
                                            new { controller = "Groups", action = "Details", Id = id }));
                }
            }
            return RedirectToAction("Index");
        }

        // GET: Groups/Create
        public ActionResult Create()
        {
            GroupCreateViewModel model = new GroupCreateViewModel();
            return View(model);
        }

        // POST: Groups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(GroupCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (Facade.CreateGroup(User.Identity.GetUserId(), model.Name))
                {
                    TempData["message"] = string.Format("Group was created successfully!");
                    return RedirectToAction("Index");
                }           
            }
            TempData["message"] = string.Format("Creation failed!");
            return View(model);
        }

        // GET: Groups/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Group group = db.Groups.Find(id);
            if (group == null)
            {
                return HttpNotFound();
            }
            return View(group);
        }

        // POST: Groups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Group group = db.Groups.Find(id);
            db.Groups.Remove(group);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
