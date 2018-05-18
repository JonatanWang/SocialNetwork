using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheSocialNetwork.Models.Groups
{
    public class GroupViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<string> Members { get; set; }

        public GroupViewModel(int id, string name, DateTime createdDate, List<string> members)
        {
            Id = id;
            Name = name;
            CreatedDate = createdDate;
            Members = members;
        }
    }

    public class GroupIndexViewModel
    {
        public List<GroupViewModel> Groups { get; set; }
        public int UnreadMessages { get; set; }
    }

    public class GroupDetailViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public List<string> Members { get; set; }
    }

    public class GroupCreateViewModel
    {
        public string Name { get; set; }
    }
}