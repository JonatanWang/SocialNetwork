using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using TheSocialNetwork.Models;
using TheSocialNetwork.Models.Messages;

namespace TheSocialNetwork.Controllers
{
    [Authorize]
    public class MessagesController : Controller
    {       
        // GET: Messages
        public ActionResult Index()
        {
            List<MessageViewModel> messages = Facade.GetAllMessages(User.Identity.GetUserId());
            ReadIndexViewModel model = new ReadIndexViewModel
            {
                Messages = messages.GroupBy(m => m.SenderUserName)
            };

            MessageInforationViewModel infoModel = new MessageInforationViewModel();
            infoModel.TotalAmountOfMessages = messages.Count;
            infoModel.TotalAmountOfReadMessages = messages.Where(m => m.IsRead).ToList().Count;
            infoModel.TotalAmountOfDeletedMessages = Facade.GetUser(User.Identity.GetUserId())
                .AmountOfDeletedMessages;
            model.infoModel = infoModel;

            model.UnreadMessages = messages.Where(m => !m.IsRead).Count();
            return View(model);
        }

        // GET: Messages/Details/5
        public ActionResult MessagesByUser(string id)
        {
            if (id.Equals(null))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReadUsersMessagesViewModel model = new ReadUsersMessagesViewModel();
            List<MessageViewModel> messages = Facade.GetAllMessages(User.Identity.GetUserId());

            model.Messages = messages.Where(m => m.SenderID == id);
            model.UnreadMessages = messages.Where(m => !m.IsRead).Count();

            if (model.Messages == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        // GET: Messages/Details/5
        public ActionResult Details(int id)
        {
            MessageViewModel message = Facade.GetMessage(User.Identity.GetUserId(),id);
            if (message == null)
            {
                return HttpNotFound();
            }

            //Prepare view model
            ReadMessageViewModel model = new ReadMessageViewModel();
            model.Text = message.Text;
            model.FromId = message.SenderID;
            model.From = message.SenderUserName;
            model.Date = message.SendDate;
            return View(model);
        }

        // GET: Messages/Create
        public ActionResult Create()
        {
           //TODO: fix this
           SendMessageViewModel model = new SendMessageViewModel();
           model.users = new SelectList(Facade.GetAllUsers(), "Id","Username"); //Poupulate selectlist in viewmodel with all users
           model.GroupsList = new SelectList(Facade.GetAllGroups(), "Id", "Name");
           return View(model);
        }

        // POST: Messages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SendMessageViewModel message)
        {
            if (ModelState.IsValid)
            {
                if(Facade.SendMessage(User.Identity.GetUserId(), message.Title, message.Text, message.Receviers, message.Groups))
                    TempData["message"] =string.Format("Message was sent successfully!");
                else
                    TempData["message"] = string.Format("Message failed!");

                return RedirectToAction("Index");
            }
            return View(message);
        }

        // GET: Messages/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MessageViewModel message = Facade.GetMessage(User.Identity.GetUserId(),(int)id);
            if (message == null)
            {
                return HttpNotFound();
            }
            //Load in data into the view model
            DeleteMessageViewModel model = new DeleteMessageViewModel();
            model.Text = message.Text;
            model.To = User.Identity.GetUserName();
            model.From = message.SenderUserName;
            model.Date = message.SendDate;
            return View(model);
        }

        // POST: Messages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Facade.DeleteMessage(User.Identity.GetUserId(), id);
            return RedirectToAction("Index");
        }        
    }
}
