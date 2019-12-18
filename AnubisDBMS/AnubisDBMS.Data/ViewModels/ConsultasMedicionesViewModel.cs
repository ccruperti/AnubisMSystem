using AnubisDBMS.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnubisDBMS.Data.ViewModels
{
    public class ConsultasMedicionesViewModel
    {
        public ConsultasMedicionesViewModel()
        {
            Lecturas = new List<DataSensores>();
        }
        public List<DataSensores> Lecturas { get; set; }
    }
}
