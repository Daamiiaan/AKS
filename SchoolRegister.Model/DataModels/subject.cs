using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolRegister.Model.DataModels
{
    public class Subject
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public virtual IList<SubjectGroup> SubjectGroups { get; set; } = default!;
        public virtual Teacher? Teacher { get; set; }
        [ForeignKey("Teacher")]
        public int? TeacherId { get; set; }
        public virtual IList<Grade> Grades { get; set; } = default!;

        public Subject()
        {
            SubjectGroups = new List<SubjectGroup>();
            Grades = new List<Grade>();
        }
    }
}