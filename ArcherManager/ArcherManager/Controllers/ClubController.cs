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

        [HttpGet]
        [ActionName(nameof(Edit))]
        public IActionResult Edit(int id)
        {
            var model = _dbContext.Clubs.FirstOrDefault(c => c.Id == id);
            return View(model);
        }

        [HttpPost]
        [ActionName(nameof(Edit))]
        public async Task<IActionResult> EditPost(int id)
        {
            var club = _dbContext.Clubs.Single(c => c.Id == id);
            var ok = await this.TryUpdateModelAsync(club);

            if (ok && this.ModelState.IsValid)
            {
                _dbContext.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            return View();
        }


    }
}
