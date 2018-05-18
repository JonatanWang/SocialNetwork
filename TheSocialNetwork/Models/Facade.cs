using System;
using System.Collections.Generic;
using System.Linq;
using TheSocialNetwork.DAL;
using TheSocialNetwork.Models.Groups;
using TheSocialNetwork.Models.Messages;

namespace TheSocialNetwork.Models
{
    public class Facade
    {
        public static List<MessageViewModel> GetAllMessages(string UserId)
        {
            var messagesToUser = MessageDB.GetAllMessages(UserId);
            List<MessageViewModel> messages = new List<MessageViewModel>();

            foreach (Message msg in messagesToUser)
            {
                messages.Add(new MessageViewModel(msg.ID, msg.sender.UserName, msg.sender.Id, 
                    msg.Title, msg.Text, msg.SendDate, msg.IsRead));
            }
            return messages.OrderByDescending(m=> m.SendDate).ToList();
        }

        public static UserViewModel GetUser(string UserId)
        {
            ApplicationUser user = UserDB.GetUser(UserId);
            UserViewModel usr = new UserViewModel(user.Id, user.UserName);
            usr.LastLogin = user.LastLogin;
            usr.AmountOfLogins = user.AmountOfLogins;
            usr.AmountOfDeletedMessages = user.AmountOfDeletedMessages;
            return usr;
        }

        public static List<UserViewModel> GetAllUsers()
        {
            List<ApplicationUser> users = UserDB.GetAllUsers();
            List<UserViewModel> usersViewModel = new List<UserViewModel>();
            users.ForEach(u => usersViewModel.Add(new UserViewModel(u.Id, u.UserName)));
            return usersViewModel;
        }

        public static MessageViewModel GetMessage(string UserId, int MessageId)
        {
            Message msg = MessageDB.GetMessage(UserId, MessageId);
            MessageViewModel msgView = new MessageViewModel(msg.ID, msg.sender.UserName, msg.sender.Id, msg.Title, msg.Text, msg.SendDate, msg.IsRead);
            return msgView;

        }
         public static bool SendMessage(string senderId, string title, string text, List<String> receivers, List<int> groups)
        {
            return MessageDB.SendMessage(senderId,title,text, receivers,groups);
        }

        public static bool DeleteMessage(string userId, int messageId)
        {
            return MessageDB.DeleteMessage(userId, messageId);
        }

        public static List<GroupViewModel> GetAllGroups()
        {
            List<GroupViewModel> groups = new List<GroupViewModel>();
            List<Group> groupList = GroupDB.GetAllGroups();
            groupList.ForEach(g => groups.Add(new GroupViewModel(g.ID, g.Name, g.CreatedDate, 
                g.Receviers.Select(r => r.UserName).ToList())));
            return groups;
        }

        public static GroupViewModel GetGroup(int GroupId)
        {
            Group group = GroupDB.GetGroup(GroupId);
            return new GroupViewModel(group.ID, group.Name, group.CreatedDate, 
                group.Receviers.Select(r => r.UserName).ToList());
        }

        public static bool CreateGroup(string UserId, string GroupName)
        {
            return GroupDB.CreateGroup(UserId, GroupName);
        }

        public static bool JoinGroup(string UserId, int GroupId)
        {
            return GroupDB.JoinGroup(UserId, GroupId);
        }

        public static bool LeaveGroup(string UserId, int GroupId)
        {
            return GroupDB.LeaveGroup(UserId, GroupId);
        }
    }
}