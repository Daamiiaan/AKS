namespace SchoolRegister.Model.DataModels
{
    public class Teacher : User
    {
        public virtual IList<Subject> Subjects { get; set; } = default!;
        public string? Title { get; set; }

        public Teacher()
        {
            Subjects = new List<Subject>();
        }
    }
}
 