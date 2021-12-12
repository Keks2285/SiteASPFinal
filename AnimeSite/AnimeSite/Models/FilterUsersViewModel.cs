using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnimeSite.Models
{
    public class FilterUsersViewModel
    {
        public string? SelectLogin { get; set; }
        public FilterUsersViewModel(string? login)
        {

            SelectLogin = login;
        }
    }
}
