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
            FillCompetitionType();
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
                FillClubsDropdown();
                FillCompetitionType();
                return View();
            }
        }

        public void FillCompetitionType()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (CompetitionType type in Enum.GetValues(typeof(CompetitionType)))
            {
                list.Add(new SelectListItem
                {
                    Text = type.ToString(),
                    Value = type.ToString()
                });
            }
            ViewBag.AllCompetitionTypes = list;
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
            foreach (BowType bow in Enum.GetValues(typeof(BowType)))
            {
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

        [Route("natjecanja/detaljno/{id}")]
        public IActionResult Details(int id)
        {
            FillArchersDropdown();
            Competition competition = LoadCompetition(id);
            FillBowTypeDropdown();
            return View(new AddArcherEntryModel
            {
                CompetitionId = competition.Id,
                BowType = BowType.Barebow
            });
        }

        [HttpPost]
        [Route("natjecanja/detaljno/{id}")]
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
            return View(new AddArcherEntryModel
            {
                CompetitionId = competition.Id,
                BowType = BowType.Barebow
            });
        }

        [Route("natjecanja/rezultati/{id}")]
        public IActionResult Results(int id)
        {
            var competitors = LoadCompetition(id)
                .Competitors.ToList();
            return View(new InputScoreModel
            {
                CompetitionId = id,
                Competitors = competitors,
            });
        }

        [HttpPost]
        [Route("natjecanja/rezultati/{id}")]
        public async Task<IActionResult> Results(InputScoreModel model)
        {
            LoadCompetition(model.CompetitionId);
            Competition competition = await _dbContext.Competitions
                .Include(c => c.Competitors)
                .ThenInclude(s => s.Archer)
                .FirstOrDefaultAsync(c => c.Id == model.CompetitionId);
            List<Score> scores = competition.Competitors.ToList();
            for (int i = 0; i < competition.Competitors.Count; i++)
            {
                scores[i].ScoreAmount = model.Competitors[i].ScoreAmount;

            }
            await _dbContext.SaveChangesAsync();
            return View(new InputScoreModel
            {
                CompetitionId = model.CompetitionId,
                Competitors = competition.Competitors.ToList(),
            });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteScore([FromQuery] int scoreId, [FromQuery] int competitionId)
        {
            var competition = await _dbContext.Competitions
                .Include(c => c.Competitors)
                .ThenInclude(s => s.Archer)
                .FirstOrDefaultAsync(c => c.Id == competitionId);
            Score score = competition.Competitors.First(s => s.Id == scoreId);
            competition.Competitors.Remove(score);
            await _dbContext.SaveChangesAsync();

            return Ok(new { redirectUrl = Url.Action(nameof(Details), new { id = competitionId }) });
        }
    }
}
