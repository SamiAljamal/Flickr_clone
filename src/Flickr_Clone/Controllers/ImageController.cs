using Microsoft.AspNetCore.Mvc;
using Flickr_Clone.Models;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Flickr_Clone.Controllers
{
    public class ImageController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public ImageController(UserManager<ApplicationUser> userManager, ApplicationDbContext db)
        {
            _db = db;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            List<Image> images = _db.Images.ToList();
            return View(images);
        }
        public async Task<IActionResult> Details(int Id)
        {
            Image img = _db.Images
                .Include(i => i.ImageTags)
                    .ThenInclude(it => it.Tag)
                .FirstOrDefault(i => i.Id == Id);
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var currentUser = await _userManager.FindByIdAsync(userId);
            ViewBag.ShowDelete = false;
            if (img.UserId == currentUser.Id)
            {
                ViewBag.ShowDelete = true;
            }
            return View(img);
        }
        [HttpPost]
        public IActionResult Details(string tag, string img)
        {
            var tagExist = _db.Tags.FirstOrDefault(t => tag == t.description); //check db to see if tag is in db
            int imgId= int.Parse(img);
            Tag newTag;
            if (tagExist == null)
            {
                newTag = new Tag();
                newTag.description = tag;
            }
            else
            {
                newTag = tagExist;
            }
            ImageTag newIT = new ImageTag();
            newIT.TagId= newTag.TagId;
            newIT.Tag = newTag;
            newIT.ImageId = imgId;
            if (tagExist == null)
            {
                _db.Tags.Add(newTag);
            }
            else
            {
                _db.Entry(newTag).State = EntityState.Modified;
            }
            _db.SaveChanges();
            _db.ImageTags.Add(newIT);
            _db.SaveChanges();

            return RedirectToAction("Details", new { Id = imgId });
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var currentUser = await _userManager.FindByIdAsync(userId);
            var thisImage = _db.Images.FirstOrDefault(images => images.Id == id);
            if (thisImage.UserId == currentUser.Id)
            {
                _db.Images.Remove(thisImage);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Details", new { Id = id });
        }
    }
}
