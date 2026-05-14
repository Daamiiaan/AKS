namespace SchoolRegister.ViewModels.VM
{
    public class SubjectVm
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public int? TeacherId { get; set; }
        public string? TeacherName { get; set; }
    }
}
