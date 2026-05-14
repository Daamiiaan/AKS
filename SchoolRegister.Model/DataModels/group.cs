using System.ComponentModel.DataAnnotations;

namespace SchoolRegister.Model.DataModels
{
    public class Group
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = default!;
        public virtual IList<Student> Students { get; set; } = default!;
        public virtual IList<SubjectGroup> SubjectGroups { get; set; } = default!;

        public Group()
        {
            Students = new List<Student>();
            SubjectGroups = new List<SubjectGroup>();
        }
    }
}