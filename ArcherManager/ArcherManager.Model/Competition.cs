using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArcherManager.Model
{
    public class Competition : IValidatableObject
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public DateOnly CompetitionStart { get; set; }
        [Required]
        public DateOnly CompetitionEnd { get; set; }
        [ForeignKey(nameof(Organiser))]
        public int OrganiserId { get; set; }
        public Club? Organiser { get; set; }
        [Required]
        public CompetitionType CompetitionType { get; set; }
        public virtual ICollection<Score>? Competitors { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (CompetitionEnd < CompetitionStart)
            {
                yield return new ValidationResult(
                    "Competition end date must be the same or after the start date.",
                    new[] { nameof(CompetitionEnd), nameof(CompetitionStart) }
                );
            }
        }
    }
}
