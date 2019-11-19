using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnubisDBMS.Data.ViewModels
{
    public class HomeVm
    {
        public string Estado { get; set; }
        public string EstiloCSS { get; set; }
    }

    public class SharedVM
    {
        public bool Visible { get; set; }
        public HomeVm HomeVM { get; set; }
    
    }
}
