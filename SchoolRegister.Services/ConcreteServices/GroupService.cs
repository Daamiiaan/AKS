using AutoMapper;
using Microsoft.Extensions.Logging;
using SchoolRegister.DAL.EF;
using SchoolRegister.Model.DataModels;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;

namespace SchoolRegister.Services.ConcreteServices
{
    public class GroupService : BaseService, IGroupService
    {
        public GroupService(ApplicationDbContext dbContext, IMapper mapper, ILogger logger)
            : base(dbContext, mapper, logger) { }

        public GroupVm? GetGroup(int id)
        {
            var group = DbContext.Groups.FirstOrDefault(g => g.Id == id);
            return group == null ? null : Mapper.Map<GroupVm>(group);
        }

        public IEnumerable<GroupVm> GetGroups(Func<GroupVm, bool>? filterPredicate = null)
        {
            var groups = DbContext.Groups.ToList();
            var groupVms = Mapper.Map<IEnumerable<GroupVm>>(groups);
            return filterPredicate == null ? groupVms : groupVms.Where(filterPredicate);
        }

        public GroupVm AddOrUpdateGroup(AddOrUpdateGroupVm vm)
        {
            if (vm.Id.HasValue)
            {
                var existing = DbContext.Groups.FirstOrDefault(g => g.Id == vm.Id.Value)
                    ?? throw new ArgumentException($"Group with id {vm.Id} not found.");
                existing.Name = vm.Name;
                DbContext.SaveChanges();
                return Mapper.Map<GroupVm>(existing);
            }
            else
            {
                var newGroup = new Group { Name = vm.Name };
                DbContext.Groups.Add(newGroup);
                DbContext.SaveChanges();
                return Mapper.Map<GroupVm>(newGroup);
            }
        }

        public GroupVm AttachStudentToGroup(AttachDetachStudentToGroupVm vm)
        {
            var group = DbContext.Groups.FirstOrDefault(g => g.Id == vm.GroupId)
                ?? throw new ArgumentException($"Group with id {vm.GroupId} not found.");
            var student = DbContext.Users.OfType<Student>().FirstOrDefault(s => s.Id == vm.StudentId)
                ?? throw new ArgumentException($"Student with id {vm.StudentId} not found.");

            student.GroupId = group.Id;
            DbContext.SaveChanges();
            return Mapper.Map<GroupVm>(group);
        }

        public GroupVm DetachStudentFromGroup(AttachDetachStudentToGroupVm vm)
        {
            var group = DbContext.Groups.FirstOrDefault(g => g.Id == vm.GroupId)
                ?? throw new ArgumentException($"Group with id {vm.GroupId} not found.");
            var student = DbContext.Users.OfType<Student>().FirstOrDefault(s => s.Id == vm.StudentId && s.GroupId == vm.GroupId)
                ?? throw new ArgumentException($"Student with id {vm.StudentId} is not in group {vm.GroupId}.");

            student.GroupId = null;
            DbContext.SaveChanges();
            return Mapper.Map<GroupVm>(group);
        }

        public GroupVm AttachSubjectToGroup(AttachDetachSubjectGroupVm vm)
        {
            var group = DbContext.Groups.FirstOrDefault(g => g.Id == vm.GroupId)
                ?? throw new ArgumentException($"Group with id {vm.GroupId} not found.");
            var subject = DbContext.Subjects.FirstOrDefault(s => s.Id == vm.SubjectId)
                ?? throw new ArgumentException($"Subject with id {vm.SubjectId} not found.");

            bool exists = DbContext.SubjectGroups.Any(sg => sg.GroupId == vm.GroupId && sg.SubjectId == vm.SubjectId);
            if (!exists)
            {
                DbContext.SubjectGroups.Add(new SubjectGroup { GroupId = vm.GroupId, SubjectId = vm.SubjectId });
                DbContext.SaveChanges();
            }
            return Mapper.Map<GroupVm>(group);
        }

        public GroupVm DetachSubjectFromGroup(AttachDetachSubjectGroupVm vm)
        {
            var group = DbContext.Groups.FirstOrDefault(g => g.Id == vm.GroupId)
                ?? throw new ArgumentException($"Group with id {vm.GroupId} not found.");
            var sg = DbContext.SubjectGroups.FirstOrDefault(sg => sg.GroupId == vm.GroupId && sg.SubjectId == vm.SubjectId)
                ?? throw new ArgumentException($"Subject {vm.SubjectId} is not attached to group {vm.GroupId}.");

            DbContext.SubjectGroups.Remove(sg);
            DbContext.SaveChanges();
            return Mapper.Map<GroupVm>(group);
        }
    }
}
