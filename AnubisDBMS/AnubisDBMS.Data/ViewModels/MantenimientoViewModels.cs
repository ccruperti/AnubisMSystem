using AnubisDBMS.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnubisDBMS.Data.ViewModels
{
    public class MantenimientoVM
    {
        public MantenimientoVM()
        {
        Lista= new List<Mantenimiento>();
        }
        public long IdManteniemiento { get; set; }
        public long IdEquipoSensor { get; set; }
        public long IdEstado { get; set; }
        public long IdTecnico { get; set; }
        public string IdUsuario { get; set; }
        public long IdFrecuencia { get; set; }
        public string Notas { get; set; }
        public bool ServicioActivo { get; set; }
        public bool Notificiaciones { get; set; }
        public string Descripcion { get; set; }

        List<Mantenimiento> Lista = new List<Mantenimiento>();
    }
}
 
