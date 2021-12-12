using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnimeSite.Models
{
    public class IndexViewModel
    {
        public IEnumerable<User> Users { get; set; }
        public User User { get; set; }
        public FilterViewModel FilterViewModel { get; set; }
        public SortViewModel SortViewModel { get; set; }
        public PageViewModel PageViewModel { get; set; }
        public FilterPostViewModel FilterPostViewModel { get; set; }
        public FilterUsersViewModel FilterUsersViewModel { get; set; }
        public IEnumerable<Post> Posts { get; set; }
    }

   
}