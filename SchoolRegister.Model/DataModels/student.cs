using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolRegister.Model.DataModels
{
    public class Student : User
    {
        public virtual Group? Group { get; set; }
        [ForeignKey("Group")]
        public int? GroupId { get; set; }
        public virtual IList<Grade> Grades { get; set; } = default!;
        public virtual Parent? Parent { get; set; }
        [ForeignKey("Parent")]
        public int? ParentId { get; set; }

        [NotMapped]
        public double AverageGrade
        {
            get
            {
                if (Grades == null || !Grades.Any())
                    return 0;

                return Grades.Average(g => (double)g.GradeValue);
            }
        }

        [NotMapped]
        public IDictionary<string, double> AverageGradePerSubject
        {
            get
            {
                if (Grades == null || !Grades.Any())
                    return new Dictionary<string, double>();

                return Grades
                    .Where(g => g.Subject != null)
                    .GroupBy(g => g.Subject.Name)
                    .ToDictionary(
                        group => group.Key,
                        group => group.Average(g => (double)g.GradeValue)
                    );
            }
        }

        [NotMapped]
        public IDictionary<string, List<GradeScale>> GradesPerSubject
        {
            get
            {
                if (Grades == null || !Grades.Any())
                    return new Dictionary<string, List<GradeScale>>();

                return Grades
                    .Where(g => g.Subject != null)
                    .GroupBy(g => g.Subject.Name)
                    .ToDictionary(
                        group => group.Key,
                        group => group.Select(g => g.GradeValue).ToList()
                    );
            }
        }

        public Student()
        {
            Grades = new List<Grade>();
        }
    }
}