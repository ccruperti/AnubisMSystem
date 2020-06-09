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
            EquiposSensores = new List<EquipoSensor>();
        }
        public long IdSensor { get; set; }
        public long IdEquipo { get; set; }

        public long IdEquipoSensor { get; set; }
        public string NumSerieEquipo { get; set; }
        public int NumPuerto { get; set; }
        public string NumIdentificacionSensor { get; set; }
        public List<EquipoSensor> EquiposSensores { get; set; }
    }
    public class ListaEquipos
    {
        public ListaEquipos()
        {
            EquiposSensor = new List<EquipoSensorVM>();
        }

        public DateTime? Desde { get; set; }
        public DateTime? Hasta { get; set; }

        public List<EquipoSensorVM> EquiposSensor { get; set; }

    }

    public class EquipoSensorVM
    {
        public Equipo EquipoDb { get; set; }
        public int Sensores { get; set; }
        public int Mantenimeintos { get; set; }

    }
    public class MonitoreoSensoresVM
    {
        public MonitoreoSensoresVM()
        {
            DatosSensores = new List<DataSensoresVM>();
        }
        public long IdEquipo { get; set; }
        public string QR { get; set; }
        public string AliasEquipo { get; set; }
        public string TipoSensor { get; set; }
        public List<DataSensoresVM> DatosSensores { get; set; }
        public DateTime? Desde { get; set; }
        public DateTime? Hasta { get; set; }
    }

    public class DataSensoresVM
    {
       
        public string SerieSensor { get; set; }
        public string TipoSensor { get; set; }
        public double Lectura { get; set; }
        public string UnidadMedida { get; set; }
        public double? MinVal { get; set; }
        public double? MaxVal { get; set; }
        public double? LecMin { get; set; }
        public double? LecMax { get; set; }

    }

}
