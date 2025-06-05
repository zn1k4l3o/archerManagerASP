using ArcherManager.Model;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ArcherManager.Web.Models
{
    public class ArcherDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public int ClubId { get; set; }
        public Club Club { get; set; }

    }
}
