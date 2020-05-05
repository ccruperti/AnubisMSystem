using System;
using Microsoft.AspNet.Identity.Owin;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using AnubisDBMS.Infraestructure.Data.Security.Entities;
using AnubisDBMS.Infraestructure.Security.Managers;
using AnubisDBMS.Infraestructure.Data.Security.ViewModels;
using AnubisDBMS.Data.Localization.Entities;
using AnubisDBMS.Data;
using AnubisDBMS.Controllers;

namespace AnubisDBMS.Areas.Seguridad.Controllers
{


    public class AdministracionController : MainController
    {
        protected AnubisDBMSUserManager _userManager;
        protected AnubisDBMSRoleManager _roleManager;
        public AnubisDbContext db = new AnubisDbContext();

        public AdministracionController()
        {

        }
        public SelectList SelectListEmpresas(long? id = null)
        {

            List<Empresa> data = db.Empresas.Where(c => c.Activo).ToList();
            data.Add(new Empresa { IdEmpresa = 0, Nombre = "Seleccione Empresa" });
            return new SelectList(data.OrderBy(c => c.IdEmpresa), "IdEmpresa", "Nombre", id);

        }
        public AdministracionController(AnubisDBMSUserManager userManager, AnubisDBMSRoleManager roleManager)
        {
            UserManager = userManager;
            RoleManager = roleManager;
        }

        public AnubisDBMSUserManager UserManager
        {
            get => _userManager ?? HttpContext.GetOwinContext().GetUserManager<AnubisDBMSUserManager>();
            private set => _userManager = value;
        }

        public AnubisDBMSRoleManager RoleManager
        {
            get => _roleManager ?? HttpContext.GetOwinContext().GetUserManager<AnubisDBMSRoleManager>();
            private set => _roleManager = value;
        }

        // GET: Seguridad/Administracion
        public ActionResult Index(long? recentId)
        {
            var model = new List<RoleUserListViewModel>();

            var UserId = User.Identity.GetUserId<long>();

            var currentRoleId = UserManager.FindById(UserId);
            var roleid = currentRoleId?.Roles?.First();

            var currentRole = RoleManager.FindById(roleid.RoleId);
            var roles = RoleManager.AvailableEditRoles(currentRole.Prioridad).ToList();
          
            foreach (var rol in roles)
            {
                var rolModel = new RoleUserListViewModel
                {
                    Id = rol.Id,
                    Nombre = rol.Plural,
                    Bloqueado = rol.Sistema,
                    Descripcion = rol.Descripcion
                };
                var rolUsuarios = UserManager.GetUsersInRole(rol.Id, User.Identity.GetUserId<long>()); 
                if (!User.IsInRole("Developers"))
                {
                    rolUsuarios = rolUsuarios.Where(x => x.IdEmpresa == IdEmpresa).ToList();
                }
                rolModel.Usuarios = rolUsuarios.Select(c => new RoleUserListItemViewModel
                {
                    Id = c.Id,
                    Usuario = c.UserName,
                    Celular = c.Celular,
                    Nombres = c.Nombres,
                    Apellidos = c.Apellidos,
                    FechaRegistro = c.FechaRegistro
                }).ToList();
                model.Add(rolModel);
            }
            return View(model);
        }

        [ChildActionOnly]
        public PartialViewResult ListaUsuarioRol(RoleUserListViewModel model)
        {
            return PartialView(model);
        }

        [HttpGet]
        public async Task<ActionResult> RegistrarUsuario(int? idRol)
        {
            var model = new RegisterNewUserViewModel();
         
            if (idRol != null)
            {
                var role = await RoleManager.FindByIdAsync(idRol ?? 0);
                if (role != null)
                {
                    model.Rol = role.Name;
                    model.RolSeleccionado = true;
                    model.IdRol = idRol.Value;
                }
            }
            var currentRoleId = UserManager.FindById(User.Identity.GetUserId<long>()).Roles.First();
            var currentRole = RoleManager.FindById(currentRoleId.RoleId);
            ViewBag.IdRol = new SelectList(RoleManager.AvailableEditRoles(currentRole.Prioridad), "Id", "Name", model.IdRol);
            if(!User.IsInRole("Developers"))
            {
            model.IdEmpresa = db.Users.FirstOrDefault(x=>x.IdEmpresa==IdEmpresa)?.IdEmpresa??0;
            }
            ViewBag.IdEmpresa = SelectListEmpresas(model.IdEmpresa); 
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RegistrarUsuario(RegisterNewUserViewModel model)
        {
                if (ModelState.IsValid)
            {
                var role =  RoleManager.FindById(model.IdRol);

                var nuevoUsuario = new AnubisDBMSUser
                {
                    UserName = model.Usuario, 
                    Nombres = model.Nombres,
                    Celular = model.Celular,
                    Apellidos = model.Apellidos,
                    TipoUsuario = role.Name,
                    IdEmpresa = model.IdEmpresa
                };
                   
                var creacion = await UserManager.CreateAsync(nuevoUsuario, model.Contrasena);
                if (creacion.Succeeded)
                {
                    if(model.IdEmpresa != 0)
                    {
                      nuevoUsuario.IdEmpresa=model.IdEmpresa; 
                      db.SaveChanges();
                    }
                    var rol = await RoleManager.FindByIdAsync(model.IdRol);
                    await UserManager.AddToRoleAsync(nuevoUsuario.Id, rol.Name);
                    return RedirectToAction("Index", new {recentId = nuevoUsuario.Id});
                }
                else
                {
                    ModelState.AddModelError("Usuario", creacion.Errors.First());
                }
            }
            var currentRoleId = UserManager.FindById(User.Identity.GetUserId<long>()).Roles.First();
            var currentRole = RoleManager.FindById(currentRoleId.RoleId);
            ViewBag.IdRol = new SelectList(RoleManager.AvailableEditRoles(currentRole.Prioridad), "Id", "Name", model.IdRol);
            ViewBag.IdEmpresa = SelectListEmpresas(model.IdEmpresa);

            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> ReseteoManualClave(long id)
        {
            var usuario = await UserManager.FindByIdAsync(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            var viewModel = new ManualPasswordResetViewModel
            {
                Id = usuario.Id,
                Usuario = usuario.UserName,
               
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ReseteoManualClave(ManualPasswordResetViewModel viewModel)
        {
            var usuario = await UserManager.FindByIdAsync(viewModel.Id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            if (ModelState.IsValid)
            {
                var changePass = await UserManager.ManualResetPasswordAsync(usuario.Id, viewModel.Contrasena);
                if (changePass.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("Contrasena", changePass.Errors.First());
            }
            viewModel.Email = usuario.Email;
            viewModel.Usuario = usuario.UserName;
            return View(viewModel);
        }

        [HttpGet]
        public async Task<ActionResult> EditarUsuario(long id)
        {
            var usuario = await UserManager.FindByIdAsync(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            var viewModel = new EditUserViewModel
            {
                Id = usuario.Id,
                Usuario = usuario.UserName,
                Celular = usuario.Celular,
                Nombres = usuario.Nombres,
                Apellidos = usuario.Apellidos,
                IdEmpresa = usuario.IdEmpresa ?? 0,
                Email = usuario.Email
            };
            var currentRoleId = UserManager.FindById(User.Identity.GetUserId<long>()).Roles.First();
            var currentRole = RoleManager.FindById(currentRoleId.RoleId);
            ViewBag.IdRol = new SelectList(RoleManager.AvailableEditRoles(currentRole.Prioridad), "Id", "Name", viewModel.IdRol);
            ViewBag.IdEmpresa = SelectListEmpresas(usuario.IdEmpresa);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditarUsuario(EditUserViewModel viewModel)
        {
            var usuario = await UserManager.FindByIdAsync(viewModel.Id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            if (usuario.UserName != viewModel.Usuario)
            {
                var newUserCheck = await UserManager.FindByNameAsync(viewModel.Usuario);
                if (newUserCheck != null)
                {
                    ModelState.AddModelError("Usuario", "Este nombre de usuario no esta disponible");
                }
            }
            if (ModelState.IsValid)
            {
                if (usuario.Roles.First().RoleId != viewModel.IdRol)
                {
                    var currentRole = await RoleManager.FindByIdAsync(usuario.Roles.First().RoleId);
                    var newRole = await RoleManager.FindByIdAsync(viewModel.IdRol);
                    await UserManager.RemoveFromRoleAsync(usuario.Id, currentRole.Name);
                    await UserManager.AddToRoleAsync(usuario.Id, newRole.Name);
                }
                if (usuario.UserName != viewModel.Usuario)
                {
                    usuario.UserName = viewModel.Usuario;
                }
                usuario.Nombres = viewModel.Nombres;
                usuario.Apellidos = viewModel.Apellidos;
                usuario.FechaModificacion = DateTime.Now;
                usuario.Celular = viewModel.Celular;
                usuario.IdEmpresa = viewModel.IdEmpresa;
                usuario.Email = viewModel.Email;
                var userUpdate = await UserManager.UpdateAsync(usuario);
                await db.SaveChangesAsync();
                if (userUpdate.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("", userUpdate.Errors.First());
            }
            ViewBag.IdRol = new SelectList(RoleManager.Roles, "Id", "Name", viewModel.IdRol);
            ViewBag.IdEmpresa = SelectListEmpresas(viewModel.IdEmpresa);

            return View(viewModel);
        }
    }
}