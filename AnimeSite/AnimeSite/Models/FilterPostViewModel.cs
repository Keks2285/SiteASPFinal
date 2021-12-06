using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnimeSite.Models
{
    public class FilterPostViewModel
    {
        public int? SelectID { get; set; }
        public FilterPostViewModel(int? id)
        {
            
            SelectID = id;
        }
    }
}
