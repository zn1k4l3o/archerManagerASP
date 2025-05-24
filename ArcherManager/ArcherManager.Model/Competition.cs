using System.ComponentModel.DataAnnotations;

namespace ArcherManager.Model
{
    public class Competition
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]  
        public Club Organiser { get; set; }
        public virtual ICollection<User> Members { get; set; }
    }
}
