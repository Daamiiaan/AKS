namespace SchoolRegister.Model.DataModels
{
    public class SubjectGroup
    {
        public virtual Subject Subject { get; set; } = default!;
        public int SubjectId { get; set; }
        public virtual Group Group { get; set; } = default!;
        public int GroupId { get; set; }

        public SubjectGroup()
        {
        }
    }
}
 