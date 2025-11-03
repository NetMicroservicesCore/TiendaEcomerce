using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TiendaEcomerce.Models;

namespace TiendaEcomerce.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;


        public AdminController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Users()
        {
            var users = _userManager.Users.ToList();
            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> AddRoles() {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddRole(string userId, string Name)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();
            if (!await _roleManager.RoleExistsAsync(Name)) await _roleManager.CreateAsync(new IdentityRole(Name));
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

        #region Create Roles
        [HttpPost]
        public async Task<IActionResult> Create(string Name)
        {
            if (User.Identity!.IsAuthenticated)
            {
                if (!await _roleManager.RoleExistsAsync(Name))
                {
                    await _roleManager.CreateAsync(new IdentityRole(Name));
                }
                return RedirectToAction("Index");
            }
            return RedirectToAction("Login");
        }
        #endregion

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


    }
}
