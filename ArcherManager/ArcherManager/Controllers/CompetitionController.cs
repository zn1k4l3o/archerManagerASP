using ArcherManager.DAL;
using ArcherManager.Model;
using ArcherManager.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ArcherManager.Web.Controllers
{
    public class CompetitionController(ArcherManagerDbContext _dbContext) : Controller
    {
        public IActionResult Index()
        {
            List<Competition> competitions = _dbContext.Competitions.Include(c => c.Organiser).ToList();
            ViewBag.Competitions = competitions;    
            return View();
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
            FillClubsDropdown();
            return View();
        }

        [HttpPost]
        public IActionResult Create(Competition formCompetition)
        {
            if (ModelState.IsValid)
            {
                formCompetition.Competitors = new List<Score>();
                _dbContext.Competitions.Add(formCompetition);
                _dbContext.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            else
            {
                this.FillClubsDropdown();
                return View();
            }
        }

        public void FillArchersDropdown()
        {
            ViewBag.AllArchers = _dbContext.Archers
                .Select(c => new SelectListItem
                {
                    Text = c.Name + " " + c.Surname,
                    Value = c.ID.ToString()
                }).ToList();
        }

        public void FillBowTypeDropdown()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            foreach(BowType bow in Enum.GetValues(typeof(BowType))) {
                list.Add(new SelectListItem 
                { 
                    Text = bow.ToString(),
                    Value = bow.ToString() 
                });
            }
            ViewBag.AllBowTypes = list;
        }

        public Competition LoadCompetition(int id)
        {
            Competition competition = _dbContext.Competitions
                .Include(c => c.Organiser)
                .Include(c => c.Competitors)
                    .ThenInclude(c => c.Archer)
                .FirstOrDefault(c => c.Id == id);
            ViewBag.Competition = competition;
            return competition;
        }

        public IActionResult Details(int id) 
        {
            FillArchersDropdown();
            Competition competition = LoadCompetition(id);
            FillBowTypeDropdown();
            return View(new AddArcherEntryModel {
                CompetitionId=competition.Id,
            BowType=BowType.Barebow
            });
        }

        [HttpPost]
        public async Task<IActionResult> Details(AddArcherEntryModel model)
        {
            Competition currentCompetition = await _dbContext.Competitions
                .Include(c => c.Competitors)
                .FirstOrDefaultAsync(c => c.Id == model.CompetitionId);
            currentCompetition.Competitors.Add(new Score()
            {
                ArcherId = model.ArcherId,
                ScoreAmount = 0,
                BowType = model.BowType,
                CompetitionType = currentCompetition.CompetitionType
            });
            await _dbContext.SaveChangesAsync();
            FillArchersDropdown();
            Competition competition = LoadCompetition(model.CompetitionId);
            FillBowTypeDropdown();
            return View(new AddArcherEntryModel {
                CompetitionId = competition.Id,
                BowType = BowType.Barebow
            });
        }

    }
}
