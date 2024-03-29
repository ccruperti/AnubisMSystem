﻿using AnubisDBMS.Data.Entities;
using AnubisDBMS.Data.Localization.Entities;
using AnubisDBMS.Infraestructure.Data.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnubisDBMS.Data.ViewModels
{
    public class Catalogos_viewModels
    {
        public class EquipoVM : CrudEntities
        {
            public EquipoVM()
            {
                Lista = new List<Equipo>();
            }
            public long IdEquipo { get; set; }
            public string Alias { get; set; }
            public string SerieEquipo { get; set; }
            public string CodigoQR { get; set; }
            public string IdUsuario { get; set; }
            public List<Equipo> Lista { get; set; }
        }





        public class SensorVM : CrudEntities
        {
            public SensorVM()
            {
                Lista = new List<SensorVM>();
            }
            public long IdSensor { get; set; }
            public long? IdTipoSensor { get; set; }
            public string TipoSensor { get; set; } 
            public double? Max { get; set; } 

            public double? Min { get; set; }
            public string NombreTipoSensor { get; set; }
            public string UnidadSensor { get; set; }
            public List<SensorVM> Lista { get; set; }
        }


        public class TipoSensorVM : CrudEntities
        {
            public TipoSensorVM()
            {
                Lista = new List<TipoSensor>();
            }
            public long IdTipoSensor { get; set; }
            public string NombreTipoSensor { get; set; }
            public string UnidadSensor { get; set; }
            [Display(Name ="Escala Máxima")]
            public double Max_TipoSensor { get; set; }
            [Display(Name = "Escala Mínima")]

            public double Min_TipoSensor { get; set; }
            public List<TipoSensor> Lista { get; set; }
        }

        public class TecnicoVM : CrudEntities
        {
            public TecnicoVM()
            {
                Lista = new List<Tecnicos>();
            }
            public long IdTecnico { get; set; }
            public string NombreTecnico { get; set; }
            public string CelularTecnico { get; set; }
            public string CorreoTecnico { get; set; }
            public string Cedula { get; set; }
            public List<Tecnicos> Lista { get; set; }
        }
        public class ListaPerfilesEmpresas
        {
            public ListaPerfilesEmpresas()
            {
                ListaPerfiles = new List<PerfilVM>();
            }
            public List<PerfilVM> ListaPerfiles { get; set; }
        }
        public class PerfilVM {
            public PerfilVM()
                {
                PrimeraNotificacion = 0;
                SegundaNotificacion = 0;
                TerceraNotificacion = 0; 
        }

            public string correo { get; set; }
            public string Ruc { get; set; }
            public string RazonSocial { get; set; }
            public int PrimeraNotificacion { get; set; }
            public int SegundaNotificacion { get; set; }
            public int TerceraNotificacion { get; set; }
            public long IdEmpresa { get; set; }
            public string NombreEmpresa { get; set; }
            public bool IsServicioActivo { get; set; }

            }
        public class NombreEmpresaSeleccionadaVm
        {
            public string NombreEmpresa { get; set; }
        }
        public class EmpresaVM : CrudEntities
        {
            public EmpresaVM()
            {
                Lista = new List<Empresa>();
            }
            public long IdEmpresa { get; set; }
            public string Nombre { get; set; }
            public string RUC { get; set; }
            public string RazonSocial { get; set; }
            public bool ServicioActivo { get; set; }
            public string EmailNotificacion { get; set; }
            public int PrimeraNotificacion { get; set; }
            public int SegundaNotificacion { get; set; }
            public int TerceraNotificacion { get; set; }
            public List<Empresa> Lista { get; set; }
        }
    }
}
