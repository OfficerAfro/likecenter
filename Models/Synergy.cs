using System;
using System.ComponentModel.DataAnnotations;

namespace likecenter.Models
{
    public class Synergy 
    {

        [Key]
        public int SynergyId {get;set;}
        public int UserId {get;set;}
        public int PostId {get;set;}
        public User User {get;set;}
        public Post Post {get;set;}
        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;
    }
}