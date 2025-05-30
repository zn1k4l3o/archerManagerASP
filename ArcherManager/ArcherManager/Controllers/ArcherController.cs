using ArcherManager.DAL;
using ArcherManager.Model;
using ArcherManager.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ArcherManager.Web.Controllers
{
    public class ArcherController(ArcherManagerDbContext _dbContext) : Controller
    {

        public  IActionResult Index()
        {
            List<Archer> archers = _dbContext.Archers.Include(a => a.Club).ToList();
            return View(archers);
        }

        private void FillClubsDropdown()
        {
            ViewBag.AllClubs = _dbContext.Clubs
                .Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                }).ToList();
        }

        public IActionResult Create()
        {
            this.FillClubsDropdown();
            return View();
        }

        [HttpPost]
        public IActionResult Create(Archer formArcher)
        {
            if (ModelState.IsValid)
            {
                Club club = _dbContext.Clubs.FirstOrDefault(club => club.Id == formArcher.ClubId);
                formArcher.Club = club;
                _dbContext.Archers.Add(formArcher);
                _dbContext.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            else
            {
                this.FillClubsDropdown();
                return View();
            }
        }


    }
}
