using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AnimeSite.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Email { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string PhotoLink { get; set; } //byte
         public bool Admin { get; set; }
    }
    public class LogUser
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public bool Admin { get; set; }
    }


    public enum SortState
    {
        IdAsc,
        IdDesc,
        EmailAsc,
        EmailDesc,
    }
}
