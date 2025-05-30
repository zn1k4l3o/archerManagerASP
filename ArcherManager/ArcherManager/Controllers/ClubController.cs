using ArcherManager.DAL;
using ArcherManager.Model;
using ArcherManager.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace ArcherManager.Web.Controllers
{
    public class ClubController(ArcherManagerDbContext _dbContext) : Controller
    {
        public IActionResult Index()
        {
            List<Club> clubs = _dbContext.Clubs.ToList();
            ViewBag.AllClubs = clubs;
            return View();
        }

        public IActionResult Create() 
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Club formClub)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Add(formClub);
                _dbContext.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View();
            }

        }
    }
}
