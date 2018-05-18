using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

namespace TheSocialNetwork.Models.Messages
{

    public class MessageViewModel{
        
        public int ID { get; set; }
        public string SenderUserName { get; set; }
        public string SenderID { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }

        [DisplayName("Date sent")]
        public DateTime SendDate { get; set; }
        public bool IsRead { get; set; }

        public MessageViewModel(int iD, string senderUserName, string senderID, string title, string text, DateTime sendDate, bool isRead)
        {
            ID = iD;
            SenderUserName = senderUserName;
            SenderID = senderID;
            Title = title;
            Text = text;
            SendDate = sendDate;
            IsRead = isRead;
        }
    }

    public class ReadIndexViewModel
    {
        public IEnumerable<IGrouping<string, MessageViewModel>> Messages { get; set; }
        public MessageInforationViewModel infoModel  { get; set; }
        public int UnreadMessages { get; set; }
    }

    public class ReadUsersMessagesViewModel
    {
        public IEnumerable<MessageViewModel> Messages { get; set; }
        public int UnreadMessages { get; set; }
    }

    public class ReadMessageViewModel
    {
        [DataType(DataType.MultilineText)]
        [UIHint("DisplayMessage")]
        public string Text { get; set; }
        public string From { get; set; }
        public string FromId { get; set; }
        public DateTime Date { get; set; }
    }

    public class SendMessageViewModel
    {
        public SelectList users { get; set; }
        public SelectList GroupsList { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Text { get; set; }
        public List<string> Receviers { get; set; }
        public List<int> Groups { get; set; }
    }

    public class DeleteMessageViewModel
    {
        public string Text { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public DateTime Date { get; set; }

    }

    public class MessageInforationViewModel
    {
        public int TotalAmountOfMessages { get; set; }
        public int TotalAmountOfReadMessages { get; set; }
        public int TotalAmountOfDeletedMessages { get; set; }

    }
}