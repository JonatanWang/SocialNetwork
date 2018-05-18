using System;
using System.Collections.Generic;
using System.Linq;
using TheSocialNetwork.Models;

namespace TheSocialNetwork.DAL
{
    public class GroupDB : Group
    {
        public static List<Group> GetAllGroups()
        {
            using (var db = new ApplicationDbContext())
            {
                List<Group> groups = db.Groups.Include("Receviers").ToList();
                return groups;
            }
        }

        /// <summary>
        /// Get a message from user with specefied Id, this way a message can only be found if user is logged in.
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="MessageId"></param>
        /// <returns>A single message to user</returns>
        public static Group GetGroup(int GroupId)
        {
            using (var db = new ApplicationDbContext())
            {
                Group group = db.Groups.Include("Receivers").ToList().
                Find(g => g.ID == GroupId);
                return group;
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
        public static bool CreateGroup(string UserId, string GroupName)
        {
            using (var db = new ApplicationDbContext())
            {
                Group group = new Group();
                ApplicationUser curUser = db.Users.Find(UserId);
                group.Name = GroupName;
                group.CreatedDate = DateTime.Now;
                group.creator = curUser;
                group.Receviers = new List<ApplicationUser>();
                group.Receviers.Add(curUser);

                db.Groups.Add(group);
                return db.SaveChanges() > 0;
            }
        }

        public static bool JoinGroup(string UserId, int GroupId)
        {
            using (var db = new ApplicationDbContext())
            {
                Group group = db.Groups.Find(GroupId);
                ApplicationUser curUser = db.Users.Find(UserId);
                group.Receviers.Add(curUser);
                return db.SaveChanges() > 0;
            }
        }

        public static bool LeaveGroup(string UserId, int GroupId)
        {
            using (var db = new ApplicationDbContext())
            {
                Group group = db.Groups.Find(GroupId);
                ApplicationUser user = group.Receviers.Where(usr => usr.Id == UserId).FirstOrDefault();
                group.Receviers.Remove(user);
                return db.SaveChanges() > 0;
            }
        }  
    }
}