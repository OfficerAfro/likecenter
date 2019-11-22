using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace likecenter.Models
{
    public class Post
    {
        [Key]
        public int PostId {get;set;}
        [Required(ErrorMessage="Post Info Required")]
        [MinLength(5, ErrorMessage="Longer please")]
        public string PostDetail {get;set;}
        public int LikeCount {get;set;} = 0;
        public int CreatorId {get;set;}
        public string CreatorName {get;set;}
        public List<Synergy> Likers {get;set;}
        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;


    }
}