using AnubisDBMS.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnubisDBMS.Data.ViewModels
{
    public class GestionEquiposViewModels
    {
        public GestionEquiposViewModels()
        {
            Sensores = new List<EquipoSensor>();
        }
        public long? IdSensor { get; set; }
        public long? IdEquipo { get; set; }

        public long? IdEquipoSensor { get; set; }
        public string NumSerieEquipo { get; set; }
        public string NumIdentificacionSensor { get; set; }
        public List<EquipoSensor> Sensores { get; set; }
    }
    public class ListaEquipos
    {
        public ListaEquipos()
        {
            EquiposDb = new List<Equipo>();
        }
    
        public List<Equipo> EquiposDb { get; set; }
        public DateTime? Desde { get; set; }
        public DateTime? Hasta { get; set; }

    }
}
