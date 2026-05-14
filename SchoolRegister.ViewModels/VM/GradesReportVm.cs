namespace SchoolRegister.ViewModels.VM
{
    public class GradesReportVm
    {
        public int StudentId { get; set; }
        public string StudentFirstName { get; set; } = default!;
        public string StudentLastName { get; set; } = default!;
        public IList<GradeVm> Grades { get; set; } = new List<GradeVm>();
    }
}
