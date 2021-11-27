using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AnimeSite.Models
{
    public class Post
    {
            [Key]
            public int Id { get; set; }
            public string Description { get; set; }
            public byte[]  Photo { get; set; }
            public int Rating { get; set; }     
            public int UserId{ get; set; }
        //public enum SortState
        //{
        //    UserIdEqual,
        //}
    }
}
