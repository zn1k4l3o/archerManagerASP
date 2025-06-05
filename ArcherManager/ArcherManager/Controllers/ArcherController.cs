using ArcherManager.DAL;
using ArcherManager.Model;
using ArcherManager.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ArcherManager.Web.Controllers
{
    public class ArcherController(ArcherManagerDbContext _dbContext) : Controller
    {
        public Dictionary<string, int> AgeClass = new Dictionary<string, int>() {
            { "U13MW", 13 },
            { "U15MW", 15 },
            { "U18MW", 18 },
            { "U21MW", 21 },
            { "MW", 50 },
            { "MMW", 50 }
        };

        public IActionResult Index()
        {
            List<Archer> archers = _dbContext.Archers.Include(a => a.Club).ToList();
            ViewBag.AllArchers = archers;
            FillAgeClass();
            return View();
        }

        [HttpPost]
        public IActionResult IndexAjax(ArcherFilterModel filter = null)
        {
            List<Archer> archers = _dbContext.Archers.Include(c => c.Club).ToList();

            filter ??= new ArcherFilterModel();

            var archerQuery = _dbContext.Archers.Include(p => p.Club).AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.FullName))
                archerQuery = archerQuery.Where(p => (p.Name + " " + p.Surname).ToLower().Contains(filter.FullName.ToLower()));

            if (!string.IsNullOrWhiteSpace(filter.ClubName))
                archerQuery = archerQuery.Where(p => p.Club.Name.ToLower().Contains(filter.ClubName.ToLower()));

            int minAge = 0;
            int maxAge = 1000;
            if (filter.AgeClass == "MMW")
            {
                minAge = AgeClass["MMW"];
            }
            else if (filter.AgeClass != "MW" && AgeClass.ContainsKey(filter.AgeClass))
            {
                maxAge = AgeClass[filter.AgeClass];
            }
            int currentYear = DateTime.Now.Year;

            if (AgeClass.ContainsKey(filter.AgeClass))
                archerQuery = archerQuery.Where(p => currentYear - p.DateOfBirth.Year >= minAge &&
                 currentYear - p.DateOfBirth.Year <= maxAge);

            var model = archerQuery.ToList();

            return PartialView("_ArcherTable", model);
        }

        public void FillAgeClass()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem() { Text = "-", Value = "-" });
            foreach(string key in AgeClass.Keys)
            {
                list.Add(new SelectListItem
                {
                    Text = key.ToString(),
                    Value = key.ToString()
                });
            }
            ViewBag.AllClasses = list;
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

        [Authorize]
        public IActionResult Create()
        {
            this.FillClubsDropdown();
            return View();
        }

        [HttpPost]
        [Authorize]
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

        [ActionName(nameof(Edit))]
        [Authorize]
        public IActionResult Edit(int id)
        {
            this.FillClubsDropdown();
            Archer model = _dbContext.Archers.FirstOrDefault(a => a.ID == id);
            return View(model);
        }

        [HttpPost]
        [ActionName(nameof(Edit))]
        [Authorize]
        public async Task<IActionResult> EditPost(int id)
        {
            var archer = _dbContext.Archers.Single(c => c.ID == id);
            var ok = await this.TryUpdateModelAsync(archer);

            if (ok && this.ModelState.IsValid)
            {
                _dbContext.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            this.FillClubsDropdown();
            return View();
        }

    }
}
