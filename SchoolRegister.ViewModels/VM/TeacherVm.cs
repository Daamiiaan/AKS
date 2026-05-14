namespace SchoolRegister.ViewModels.VM
{
    public class TeacherVm
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string? Title { get; set; }
        public IList<SubjectVm> Subjects { get; set; } = new List<SubjectVm>();
    }
}
