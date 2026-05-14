namespace SchoolRegister.ViewModels.VM
{
    public class TeachersGroupsVm
    {
        public int TeacherId { get; set; }
        public string TeacherName { get; set; } = default!;
        public IList<GroupVm> Groups { get; set; } = new List<GroupVm>();
    }
}
