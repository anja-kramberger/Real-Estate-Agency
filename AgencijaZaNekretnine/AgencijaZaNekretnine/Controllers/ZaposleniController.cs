using AgencijaZaNekretnine.Data;
using AgencijaZaNekretnine.Models;
using AgencijaZaNekretnine.Models.EFRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using Syncfusion.EJ2.Linq;
using System.Data;
using System.Net;
using X.PagedList;

namespace AgencijaZaNekretnine.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ZaposleniController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        public ZaposleniController(UserManager<IdentityUser> userManager, ApplicationDbContext dbContext, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _context = dbContext;
            _roleManager = roleManager;

        }
        public async Task<IActionResult> Index(string searchString, string currentFilter, string? Role, string currentRole, int? page)
        {
            var pageNumber = page ?? 1;
            IEnumerable<UserBO> zaposleni  = _context.UserBOs.Where(u => u.isApproved == true).ToList();
            var uloge = _roleManager.Roles.ToList();
            ViewBag.Role = uloge;

            foreach(var item in zaposleni)
            {
                item.PhoneNumber = string.Join(",", _userManager.GetRolesAsync(item).Result.ToArray());
            }

            string? rola = Role;
            

            if (searchString != null || rola != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
                rola = currentRole;
                
            }

            ViewBag.CurrentFilter = searchString;
            ViewBag.CurrentRole = rola;

            


            if (!String.IsNullOrEmpty(searchString) || !String.IsNullOrEmpty(rola))
            {
                if (!String.IsNullOrEmpty(rola))   ///!String.IsNullOrEmpty(Request.Form["Role"].ToString())
                {
                    string? idR = rola;
                    var role = _context.Roles.Find(idR);
                    string? nazivRole = role.Name.ToString();
                    var usersWithPermission = _userManager.GetUsersInRoleAsync(nazivRole).Result;
                    var idsWithPermission = usersWithPermission.Select(s => s.Id);
                    var users = _context.UserBOs.Where(u => idsWithPermission.Contains(u.Id) && u.isApproved == true).ToList();


                    zaposleni = users;

                }
                if (!String.IsNullOrEmpty(searchString))
                {
                    zaposleni = zaposleni.Where(s => s.FirstName.Contains(searchString) || s.LastName.Contains(searchString));

                }

            }

            
            var lista = zaposleni.ToPagedList(pageNumber, 6);


            return View(lista);
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }
        

        [HttpPost]
        public async Task<IActionResult> Create(UserBO user)
        {
            
            if (ModelState.IsValid)
            {

                IdentityResult result = await _userManager.CreateAsync(user);

                if (result.Succeeded)
                    return RedirectToAction("Index");
                else
                {
                    foreach (IdentityError error in result.Errors)
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(user);
        }
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            UserBO userBO = _context.UserBOs.FirstOrDefault(u => u.Id == user.Id);
            ViewBag.Employed = userBO.Employed;
            
            ViewBag.Emaill = user.Email;
            return View(user);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(UserBO user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }

            UserBO userBO = _context.UserBOs.FirstOrDefault(u => u.Id == user.Id);
            userBO.Phone = user.Phone;
            userBO.FirstName = user.FirstName;
            userBO.LastName = user.LastName;
            userBO.Employed = user.Employed;
            
            var result = await _userManager.UpdateAsync(userBO);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }

            foreach(var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return View(userBO);

        }
        
        public IActionResult Confirmation()
        {
            IEnumerable<UserBO> zaposleni = _context.UserBOs.Where(u => u.isApproved == false).ToList();

            foreach (var item in zaposleni)
            {
                item.PhoneNumber = string.Join(",", _userManager.GetRolesAsync(item).Result.ToArray());
            }
            return View(zaposleni);
        }

        public async Task<IActionResult> Confirm(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            UserBO userBO = _context.UserBOs.FirstOrDefault(u => u.Id == user.Id);
            userBO.isApproved = true;
            userBO.Employed = true;
            _context.SaveChanges();
            return RedirectToAction("Confirmation");
        }
        public async Task<IActionResult> Delete(string id)
        {
            if (ModelState.IsValid)
            {

                var user = await _userManager.FindByIdAsync(id);
                //var logins = user.Logins;
                var rolesForUser = await _userManager.GetRolesAsync(user);

                using (var transaction = _context.Database.BeginTransaction())
                {

                    if (rolesForUser.Count() > 0)
                    {
                        foreach (var item in rolesForUser.ToList())
                        {
                            var result = await _userManager.RemoveFromRoleAsync(user, item);
                        }
                    }

                    await _userManager.DeleteAsync(user);
                    transaction.Commit();
                }

                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index");
            }
            
        }
    }
}
