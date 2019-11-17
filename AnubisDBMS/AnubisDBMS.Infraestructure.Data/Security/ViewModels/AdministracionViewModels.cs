using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnubisDBMS.Infraestructure.Data.Security.ViewModels
{
    public class RoleUserListViewModel
    {
        public RoleUserListViewModel()
        {
            Usuarios = new List<RoleUserListItemViewModel>();
        }
        public long Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public bool Bloqueado { get; set; }
        public List<RoleUserListItemViewModel> Usuarios { get; set; }
    }

    public class RoleUserListItemViewModel
    {
        public long Id { get; set; }
        public string Usuario { get; set; }
        public string Nombres { get; set; }
        public string Celular { get; set; }
        public string Apellidos { get; set; }
        public DateTime FechaRegistro { get; set; }
        public bool Bloqueado { get; set; }
    }

    public class RegisterNewUserViewModel
    {
        public RegisterNewUserViewModel()
        {
            Rol = "Usuario";
            RolSeleccionado = false;
        }

        public bool RolSeleccionado { get; set; }

        public string Rol { get; set; }

        [Required]
        [Display(Name = "Contraseña")]
        public string Contrasena { get; set; }

        [Required]
        [Display(Name = "Rol del Usuario")]
        public long IdRol { get; set; }

        [Required]
        public string Usuario { get; set; }
        public string Celular { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public string Nombres { get; set; }
        public string Apellidos { get; set; }
    }

    public class EditUserViewModel
    {
        [Required]
        public long Id { get; set; }

        [Required]
        [Display(Name = "Rol del Usuario")]
        public long IdRol { get; set; }

        [Required]
        public string Usuario { get; set; }
        public string Celular { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public string Nombres { get; set; }
        public string Apellidos { get; set; }
    }

    public class ManualPasswordResetViewModel
    {
        [Required]
        public long Id { get; set; }

        public string Usuario { get; set; }

        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "La {0} debe ser de al menos {2} caracteres", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Contrasena { get; set; }

        [Required]
        [Compare("Contrasena", ErrorMessage = "Las contraseñas deben coincidir")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirmar Contraseña")]
        public string ConfirmacionContrasena { get; set; }
    }
}
