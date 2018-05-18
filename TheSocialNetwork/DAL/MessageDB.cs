using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using TheSocialNetwork.Models;

namespace TheSocialNetwork.DAL
{
    public class MessageDB : Message
    {
        /// <summary>
        /// Get all messages to the user from the database
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns>List of messages to the user</returns>
        public static List<Message> GetAllMessages(string UserId)
        {
            using (var db = new ApplicationDbContext())
            {
                var messagesToUser = db.Users.Include("Messages").ToList()
                    .Find(m => m.Id == UserId).Messages.ToList();
                return messagesToUser;
            }
        }

        /// <summary>
        /// Get a message from user with specefied Id, this way a message can only be found if user is logged in.
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="MessageId"></param>
        /// <returns>A single message to user</returns>
        public static Message GetMessage(string UserId, int MessageId)
        {
            Message message = new Message();
            using (var db = new ApplicationDbContext())
            {
                message = db.Users.Include("Messages").ToList().
                Find(user => user.Id == UserId).Messages.ToList().Find(m => m.ID == MessageId);

                //Update message to be read
                message.IsRead = true;
                db.SaveChanges();
            }
            return message;
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
        public static bool SendMessage(string senderId, string title, string text, 
            List<String> receivers, List<int> groups)
        {
            int i = 0;
            using (var db = new ApplicationDbContext())
            {
                //Loop through selected groups and send to members
                if (groups == null && receivers == null)
                {
                    return false;
                }
                if (groups != null)
                {
                    foreach (int gId in groups)
                    {
                        Group group = db.Groups.Find(gId);
                        foreach (string id in group.Receviers.Select(r => r.Id).ToList())
                        {
                            Message msg = new Message();
                            msg.Title = title;
                            msg.Text = text;
                            msg.sender = db.Users.Find(senderId);
                            msg.SendDate = DateTime.Now;
                            msg.Recevier = db.Users.Find(id);
                            db.Messages.Add(msg);
                        }
                    }
                }
                //Loop through selected members and send
                if (receivers != null)
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
                }
                i = db.SaveChanges();
            }
            return i > 0;
        }

        public static bool DeleteMessage(string userId, int messageId)
        {
            int i = 0;
            using (var db = new ApplicationDbContext()) {
                Message message = db.Messages.Find(messageId);
                ApplicationUser user = db.Users.Find(userId);
                user.AmountOfDeletedMessages++;
                db.Entry(user).State = EntityState.Modified;
                db.Messages.Remove(message);
                i = db.SaveChanges();
            }           
            return i > 0;
        }
    }
}