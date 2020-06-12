using AnubisDBMS.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnubisDBMS.Data.ViewModels
{
    public class MantenimientoVM
    {
        public MantenimientoVM()
        {
            Lista = new List<Mantenimiento>();
            EquiposSensores = new List<EquipoSensor>();
        }
        public long? IdManteniemiento { get; set; }
        public long IdEstado { get; set; }
        [Required]
        [Display(Name = "Tecnico")]
        public long IdTecnico { get; set; } 
        public string IdUsuario { get; set; }
        [Required]
        [Display(Name = "Frecuencia")]
        public long IdFrecuencia { get; set; }
        public string Notas { get; set; }
        public bool ServicioActivo { get; set; }
        [DisplayName("Notificaciones")]
        public bool Notificaciones { get; set; }
        [Required]
        public string Descripcion { get; set; }
        [Required]
        public DateTime FechaMant { get; set; }
        public long IdEquipo { get; set; }


        public string QR { get; set; }
        public string AliasEquipo { get; set; }
        public bool AplicaMantenimiento { get; set; }
        public List<EquipoSensor> EquiposSensores { get; set; }
        public List<Mantenimiento> Lista = new List<Mantenimiento>();
    }

    public class Alerta
        {
        public string Equipo { get; set; }
        public string Sensor { get; set; }
        public string UnidadMedida { get; set; }
        public double Min { get; set; }
        public double Max { get; set; }
        public double medida { get; set; }
        public string encimadebajo { get; set; }

        }
}
 
