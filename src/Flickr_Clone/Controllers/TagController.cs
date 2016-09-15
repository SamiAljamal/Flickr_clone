using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Flickr_Clone.Models;
using Microsoft.EntityFrameworkCore;

namespace Flickr_Clone.Controllers
{
    public class TagController : Controller
    {
        private ApplicationDbContext _db;

        public TagController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            List<Tag> tags = _db.Tags.ToList();
            return View(tags);
        }

        public IActionResult Details(int id)
        {
            Tag tag = _db.Tags.Include(t => t.ImageTags)
                    .ThenInclude(it => it.Image)
                .FirstOrDefault(t => t.TagId == id);
            return View(tag);
        }
    }
}
