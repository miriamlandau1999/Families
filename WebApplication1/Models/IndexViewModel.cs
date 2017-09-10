using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.data;
namespace WebApplication1.Models
{
    public class IndexViewModel
    {
        public IEnumerable<FamilyWithNumberOfKids> FamiliesWithNumberOfKids { get; set; }
        public Activity Activity { get; set; }
        public Family Family { get; set; }
    }
}