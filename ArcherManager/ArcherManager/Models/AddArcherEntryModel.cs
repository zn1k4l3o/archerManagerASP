using ArcherManager.Model;

namespace ArcherManager.Web.Models
{
    public class AddArcherEntryModel
    {
        public int CompetitionId { get; set; }
        public int ArcherId { get; set; }
        public BowType BowType { get; set; }
    }
}
