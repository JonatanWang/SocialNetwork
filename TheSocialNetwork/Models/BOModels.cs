using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TheSocialNetwork.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {

        public DateTime LastLogin { get; set; }
        public int AmountOfLogins { get; set; }
        public int AmountOfDeletedMessages { get; set; }
        public virtual ICollection<Message> Messages { get; set; }
        public virtual ICollection<Group> Groups { get; set; }


        public ApplicationUser()
        {
            Messages = new List<Message>();
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class Message
    {
        [Key]
        public int ID { get; set; }
        public virtual ApplicationUser Recevier { get; set; }
        public virtual ApplicationUser sender { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public DateTime SendDate { get; set; }
        public bool IsRead { get; set; }
    }

    public class Group
    {
        [Key]
        public int ID { get; set; }
        public virtual ICollection<ApplicationUser> Receviers { get; set; }
        public virtual ApplicationUser creator { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}