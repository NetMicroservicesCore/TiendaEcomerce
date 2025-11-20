using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TiendaEcomerce.Data;
using TiendaEcomerce.Models;

namespace TiendaEcomerce.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly ApplicationDbContext _context;
        public AdminController(UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }
        #region Usuarios
        public async Task<IActionResult> Users()
        {
            var users = _userManager.Users.ToList();
            return View(users);
        }
        #endregion

        #region Metodo Oficial Crear Roles

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ApplicationRole roleData)
        {
            if (User.Identity!.IsAuthenticated)
            {
                if (!await _roleManager.RoleExistsAsync(roleData.Name!))
                {
                    await _roleManager.CreateAsync(new ApplicationRole { Name=roleData.Name!, Description= roleData.Description});
                }
                return RedirectToAction("Index","Home");
            }
            return RedirectToAction("Login");
        }



        #endregion


        #region Consultar Roles
        /// <summary>
        /// Este metodo se encarga de consukltar todos los roles del sistema
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<JsonResult> GetAsyncAllRoles()
        {
            List<ApplicationRole> roles = await _roleManager.Roles.ToListAsync<ApplicationRole>() ;
            return Json(roles);
        }
        #endregion
        [HttpGet]
        [Authorize]   //falta validar  por politicas de seguridad o roles de usuario
        public async Task<IActionResult> AddRoles() {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddRole(string userId, string Name)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();
            if (!await _roleManager.RoleExistsAsync(Name)) await 
                    _roleManager.CreateAsync(new ApplicationRole { Name = Name });
            await _userManager.AddToRoleAsync(user, Name);
            return RedirectToAction("Users");
        }

        [HttpPost]
        public async Task<IActionResult> RemoveRole(string userId, string role)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();
            await _userManager.RemoveFromRoleAsync(user, role);
            return RedirectToAction("Users");
        }
        public IActionResult Index()
        {
            string variable = "Hola";
            return View();
        }

        #region Implementacion de Vistas Parciales
        [HttpPost]
        public async Task<IActionResult> Edit(RoleViewModel model)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Index");

            // lógica de actualización...
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(RoleViewModel model)
        {
            // eliminar el rol
            return RedirectToAction("Index");
        }

        #endregion


        #region Asignar Permissiins a los Roles
        public async Task<IActionResult> Edit(string roleId)
        {
           /*
            var role = await _roleManager.FindByIdAsync(roleId);

            var permissions = await _context.Permissions.ToListAsync();
            var rolePermissions = await _context.RolePermissions
                        .Where(x => x.RoleId == roleId)
                        .Select(x => x.PermissionId)
                        .ToListAsync();

            var vm = new RolePermissionViewModel
            {
                RoleId = role.Id,
                RoleName = role.Name,
                Permissions = permissions.Select(p => new PermissionSelection
                {
                    PermissionId = p.Id,
                    Key = p.Key,
                    Description = p.Description,
                    Assigned = rolePermissions.Contains(p.Id)
                }).ToList()
            };*/

            return View(); //vm
        }
        #endregion




    }
}
