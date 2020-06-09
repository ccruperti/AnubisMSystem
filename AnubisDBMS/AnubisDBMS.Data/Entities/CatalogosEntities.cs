using AnubisDBMS.Data.Localization.Entities;
using AnubisDBMS.Infraestructure.Data.Helpers;
using AnubisDBMS.Infraestructure.Data.Security.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnubisDBMS.Data.Entities
{

    public class Equipo : CrudEntities
    {
        public long IdEquipo { get; set; }
        public string Alias { get; set; }
        public string SerieEquipo { get; set; }
        public string CodigoQR { get; set; }
        public string IdUsuario { get; set; }
        public bool AplicaMonitoreo { get; set; }
        public long? IdEmpresa { get; set; }
        public virtual Empresa Empresa { get; set; }
        //public virtual AnubisDBMSUser Usuarios { get; set; }
       //public IEnumerable<User> Usuarios { get; set; }
        //public virtual ICollection<EquipoSensor> EquiposSensor { get; set; }

    }

    public class Sensor : CrudEntities
    {
        public long IdSensor { get; set; }
        public long IdTipoSensor { get; set; }
        public DateTime? FechaConsulta { get; set; } 
        public double? Max { get; set; }
        public double? Min { get; set; }
        public long? IdEmpresa { get; set; }
        public virtual Empresa Empresa { get; set; }
        // public virtual ICollection<EquipoSensor> EquiposSensor { get; set; }
        public virtual TipoSensor TipoSensor { get; set; }
    }

    public class EquipoSensor : CrudEntities
    {
        public long IdEquipoSensor { get; set; }
        public long? IdEquipo { get; set; }
        public long? IdSensor { get; set; }
        public int NumeroPuerto { get; set; }
        public virtual Equipo Equipos { get; set; }
        public virtual Sensor Sensores { get; set; }
        public long? IdEmpresa { get; set; }
        public virtual Empresa Empresa { get; set; }
    }

    public class TipoSensor : CrudEntities
    {
        public long IdTipoSensor { get; set; }
        public string NombreTipoSensor { get; set; }
        public double Min_TipoSensor { get; set; }
        public double Max_TipoSensor { get; set; }
        public string UnidadSensor { get; set; }
        public long? IdEmpresa { get; set; }
        public virtual Empresa Empresa { get; set; }
    }

    public class Frecuencia : CrudEntities
    {
        public long IdFrecuencia { get; set; }
        public string NombreFrecuencia { get; set; }
        public int DiasFrecuencia { get; set; }
        public long? IdEmpresa { get; set; }
        public virtual Empresa Empresa { get; set; }
    }

    public class Estados : CrudEntities
    {
        public long IdEstado { get; set; }
        public string NombreEstado { get; set; }
        public string EstiloCss { get; set; }
        public string TipoEstado { get; set; }
        public long? IdEmpresa { get; set; }
        public virtual Empresa Empresa { get; set; }
    }

    public class Tecnicos : CrudEntities
    {
        public long IdTecnico { get; set; }
        public string NombreTecnico { get; set; }
        public string CelularTecnico { get; set; }
        public string CorreoTecnico { get; set; }
        public string Cedula { get; set; }
        public long? IdEmpresa { get; set; }
        public virtual Empresa Empresa { get; set; }
    }


    public class Mantenimiento : CrudEntities
    {
        public long IdManteniemiento { get; set; }
        public long? IdEquipo { get; set; }
        public long IdEstado { get; set; }
        public long IdTecnico { get; set; }
        public string IdUsuario { get; set; }
        public long IdFrecuencia { get; set; }
        public string Notas { get; set; }
        public bool ServicioActivo { get; set; }
        public bool Notificiaciones { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaMantenimiento { get; set; }
        public DateTime? FechaFinMantenimiento { get; set; }
        public long? IdEmpresa { get; set; }
        public virtual Empresa Empresa { get; set; }
        public virtual Equipo Equipo { get; set; }
        public virtual Estados Estados { get; set; }
        public virtual Tecnicos Tecnicos { get; set; }
        //public virtual AnubisDBMSUser Usuarios { get; set; }
        public virtual Frecuencia Frecuencias { get; set; }
    }

   
    public class DataSensores : CrudEntities
    {
        
        public long IdDataSensor { get; set; }
        public string SerieSensor { get; set; }
        public string TipoSensor { get; set; }
        public double Medida { get; set; }
        public string UnidadMedida { get; set; } 
        public bool Chequeado { get; set; }
        public bool Error { get; set; }
        public bool EncimaNormal { get; set; }
        public bool DebajoNormal { get; set; } 
        public bool AlertaRecibida { get; set; }
        public bool Notificado { get; set; }
        public long? IdEmpresa { get; set; }
        public virtual Empresa Empresa { get; set; }

    }


}
