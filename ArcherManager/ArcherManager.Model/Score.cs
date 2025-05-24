using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcherManager.Model
{

    public class Score
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int ArcherId { get; set; }
        public Archer Archer { get; set; }
        [Required]
        public int ScoreAmount { get; set; }
        [Required]
        public BowType BowType { get; set; }
        [Required]
        public CompetitionType CompetitionType { get;set; }
        [Required]
        public DateOnly Date {  get; set; }
    }
}
