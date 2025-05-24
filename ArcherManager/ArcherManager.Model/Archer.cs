using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcherManager.Model
{
    public class Archer
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        public DateOnly DateOfBirth { get; set; }
        [Required]
        public int ClubId { get; set; }
        public Club Club { get; set; }

    }
}
