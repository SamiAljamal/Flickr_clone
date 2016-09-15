using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Flickr_Clone.Models;
using Flickr_Clone.ModelViews;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Security.Claims;


// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Flickr_Clone.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private IHostingEnvironment _environment;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ApplicationDbContext db, IHostingEnvironment environment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _db = db;
            _environment = environment;
            if (_db.Roles.FirstOrDefault(r => r.Name == "admin") == null)
            {
                db.Roles.Add(new IdentityRole() { Name = "admin" });
            }
            if (_db.Roles.FirstOrDefault(r => r.Name == "user") == null)
            {
                db.Roles.Add(new IdentityRole() { Name = "user" });
            }
            db.SaveChanges();
        }

        public async Task<IActionResult> Index()
        {

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var currentUser = await _userManager.FindByIdAsync(userId);
            return View(_db.Images.Where(x => x.UserId == currentUser.Id).ToList());
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            var user = new ApplicationUser { UserName = model.Email };
            IdentityResult result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                IdentityUserRole<string> userrole = new IdentityUserRole<string>();
                var role = _db.Roles.FirstOrDefault(r => r.Name == "user");
                userrole.RoleId = role.Id;
                userrole.UserId = user.Id;
                _db.UserRoles.Add(userrole);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: true, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> LogOff()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index");
        }            



        [HttpPost]
        public async Task<IActionResult> Index(ICollection<IFormFile> files)
        {
            var uploads = Path.Combine(_environment.WebRootPath, "uploads");
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value; //find the user who uploaded the file
            var currentUser = await _userManager.FindByIdAsync(userId);
            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    using (var fileStream = new FileStream(Path.Combine(uploads, file.FileName), FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                    Image newImage = new Image();
                    newImage.Path = "/uploads/" + file.FileName;
                    newImage.UserId = currentUser.Id;
                    _db.Images.Add(newImage);
                    _db.SaveChanges();
                }
            }
            return View(_db.Images.Where(x => x.UserId == currentUser.Id).ToList());
        }
    }
}
