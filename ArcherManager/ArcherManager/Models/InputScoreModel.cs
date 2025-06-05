using ArcherManager.Model;

namespace ArcherManager.Web.Models
{
    public class InputScoreModel
    {
        public int CompetitionId { get; set; }
        public List<Score> Competitors { get; set; }
    }
}
