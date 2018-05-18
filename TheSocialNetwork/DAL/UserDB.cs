using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using TheSocialNetwork.Models;

namespace TheSocialNetwork.DAL
{
    public class UserDB : ApplicationUser
    {

        /// <summary>
        /// Get all messages to the user from the database
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns>List of messages to the user</returns>
        public static List<ApplicationUser> GetAllUsers()
        {
            using (var db = new ApplicationDbContext())
            {
                var users = db.Users.ToList();
                return users;
            }
        }

        /// <summary>
        /// Get a message from user with specefied Id, this way a message can only be found if user is logged in.
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="MessageId"></param>
        /// <returns>A single message to user</returns>
        public static ApplicationUser GetUser(string UserId)
        {
            using (var db = new ApplicationDbContext())
            {
                ApplicationUser user = db.Users.Include("Messages").ToList().
                        Find(usr => usr.Id == UserId);
                return user;
            }
        }

        /// <summary>
        /// Create a message from parameters and add it to the database.
        /// The message can be sent to multiple receviers specified in the list
        /// </summary>
        /// <param name="senderId"></param>
        /// <param name="title"></param>
        /// <param name="text"></param>
        /// <param name="receivers"></param>
        /// <returns>True if anything was added to the database</returns>
        public static bool SendMessage(string senderId, string title, string text, List<String> receivers)
        {
            using (var db = new ApplicationDbContext())
            {
                foreach (string id in receivers)
                {
                    Message msg = new Message();
                    msg.Title = title;
                    msg.Text = text;
                    msg.sender = db.Users.Find(senderId);
                    msg.SendDate = DateTime.Now;
                    msg.Recevier = db.Users.Find(id);
                    db.Messages.Add(msg);
                }
                return db.SaveChanges() > 0;
            }
        }

        public static bool DeleteMessage(string userId, int messageId)
        {
            using (var db = new ApplicationDbContext())
            {
                Message message = db.Messages.Find(messageId);
                db.Messages.Remove(message);
                db.SaveChanges();
                return db.SaveChanges() > 0;
            }
        }
    }
}