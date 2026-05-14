namespace SchoolRegister.ViewModels.VM
{
    public class StudentVm
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public int? GroupId { get; set; }
        public string? GroupName { get; set; }
        public int? ParentId { get; set; }
        public string? ParentName { get; set; }
    }
}
