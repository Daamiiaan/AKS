namespace SchoolRegister.ViewModels.VM
{
    public class GroupVm
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public IList<StudentVm> Students { get; set; } = new List<StudentVm>();
        public IList<SubjectVm> Subjects { get; set; } = new List<SubjectVm>();
    }
}
