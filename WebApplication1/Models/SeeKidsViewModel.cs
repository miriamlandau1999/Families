using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.data;

namespace WebApplication1.Models
{
    public class SeeKidsViewModel
    {
        public Family Family { get; set; }
        public IEnumerable<Kid> Kids { get; set; }
        public Kid KidAdded { get; set; }
    }
}