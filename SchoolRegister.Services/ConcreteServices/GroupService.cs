using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchoolRegister.DAL.EF;
using SchoolRegister.Model.DataModels;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;

namespace SchoolRegister.Services.ConcreteServices
{
    public class GroupService : BaseService, IGroupService
    {
        private readonly UserManager<User> _userManager;

        public GroupService(ApplicationDbContext dbContext, IMapper mapper, ILogger logger,
            UserManager<User> userManager) : base(dbContext, mapper, logger)
        {
            _userManager = userManager;
        }

        public GroupVm GetGroup(Expression<Func<Group, bool>> filterPredicate)
        {
            try
            {
                if (filterPredicate == null)
                    throw new ArgumentNullException("FilterPredicate is null");

                var group = DbContext.Groups
                    .Include(g => g.Students)
                    .Include(g => g.SubjectGroups).ThenInclude(sg => sg.Subject)
                    .FirstOrDefault(filterPredicate);
                return Mapper.Map<GroupVm>(group);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public IEnumerable<GroupVm> GetGroups(Expression<Func<Group, bool>> filterPredicate = null)
        {
            try
            {
                var groups = DbContext.Groups
                    .Include(g => g.Students)
                    .Include(g => g.SubjectGroups).ThenInclude(sg => sg.Subject)
                    .AsQueryable();
                if (filterPredicate != null)
                    groups = groups.Where(filterPredicate);

                return Mapper.Map<IEnumerable<GroupVm>>(groups.ToList());
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public GroupVm AddOrUpdateGroup(AddOrUpdateGroupVm addOrUpdateGroupVm)
        {
            try
            {
                if (addOrUpdateGroupVm == null)
                    throw new ArgumentNullException("View model parameter is null");

                if (addOrUpdateGroupVm.Id.HasValue && addOrUpdateGroupVm.Id > 0)
                {
                    var existing = DbContext.Groups.FirstOrDefault(g => g.Id == addOrUpdateGroupVm.Id.Value)
                        ?? throw new ArgumentException($"Group with id {addOrUpdateGroupVm.Id} not found.");
                    existing.Name = addOrUpdateGroupVm.Name;
                    DbContext.SaveChanges();
                    return Mapper.Map<GroupVm>(existing);
                }
                else
                {
                    var newGroup = new Group { Name = addOrUpdateGroupVm.Name };
                    DbContext.Groups.Add(newGroup);
                    DbContext.SaveChanges();
                    return Mapper.Map<GroupVm>(newGroup);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public StudentVm AttachStudentToGroup(AttachDetachStudentToGroupVm attachStudentToGroupVm)
        {
            try
            {
                if (attachStudentToGroupVm == null)
                    throw new ArgumentNullException("View model parameter is null");

                var group = DbContext.Groups.FirstOrDefault(g => g.Id == attachStudentToGroupVm.GroupId)
                    ?? throw new ArgumentException($"Group {attachStudentToGroupVm.GroupId} not found.");
                var student = DbContext.Users.OfType<Student>()
                    .FirstOrDefault(s => s.Id == attachStudentToGroupVm.StudentId)
                    ?? throw new ArgumentException($"Student {attachStudentToGroupVm.StudentId} not found.");

                student.GroupId = group.Id;
                DbContext.SaveChanges();
                return Mapper.Map<StudentVm>(student);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public StudentVm DetachStudentFromGroup(AttachDetachStudentToGroupVm detachStudentToGroupVm)
        {
            try
            {
                if (detachStudentToGroupVm == null)
                    throw new ArgumentNullException("View model parameter is null");

                var student = DbContext.Users.OfType<Student>()
                    .FirstOrDefault(s => s.Id == detachStudentToGroupVm.StudentId)
                    ?? throw new ArgumentException($"Student {detachStudentToGroupVm.StudentId} not found.");

                student.GroupId = null;
                DbContext.SaveChanges();
                return Mapper.Map<StudentVm>(student);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public GroupVm AttachSubjectToGroup(AttachDetachSubjectGroupVm attachSubjectGroupVm)
        {
            try
            {
                if (attachSubjectGroupVm == null)
                    throw new ArgumentNullException("View model parameter is null");

                var group = DbContext.Groups.FirstOrDefault(g => g.Id == attachSubjectGroupVm.GroupId)
                    ?? throw new ArgumentException($"Group {attachSubjectGroupVm.GroupId} not found.");
                _ = DbContext.Subjects.FirstOrDefault(s => s.Id == attachSubjectGroupVm.SubjectId)
                    ?? throw new ArgumentException($"Subject {attachSubjectGroupVm.SubjectId} not found.");

                bool exists = DbContext.SubjectGroups.Any(sg =>
                    sg.GroupId == attachSubjectGroupVm.GroupId && sg.SubjectId == attachSubjectGroupVm.SubjectId);
                if (!exists)
                {
                    DbContext.SubjectGroups.Add(new SubjectGroup
                    {
                        GroupId = attachSubjectGroupVm.GroupId,
                        SubjectId = attachSubjectGroupVm.SubjectId
                    });
                    DbContext.SaveChanges();
                }

                return Mapper.Map<GroupVm>(group);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public GroupVm DetachSubjectFromGroup(AttachDetachSubjectGroupVm detachSubjectGroupVm)
        {
            try
            {
                if (detachSubjectGroupVm == null)
                    throw new ArgumentNullException("View model parameter is null");

                var group = DbContext.Groups.FirstOrDefault(g => g.Id == detachSubjectGroupVm.GroupId)
                    ?? throw new ArgumentException($"Group {detachSubjectGroupVm.GroupId} not found.");
                var sg = DbContext.SubjectGroups.FirstOrDefault(x =>
                    x.GroupId == detachSubjectGroupVm.GroupId && x.SubjectId == detachSubjectGroupVm.SubjectId)
                    ?? throw new ArgumentException(
                        $"Subject {detachSubjectGroupVm.SubjectId} is not in group {detachSubjectGroupVm.GroupId}.");

                DbContext.SubjectGroups.Remove(sg);
                DbContext.SaveChanges();
                return Mapper.Map<GroupVm>(group);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public SubjectVm AttachTeacherToSubject(AttachDetachSubjectToTeacherVm attachSubjectToTeacherVm)
        {
            try
            {
                if (attachSubjectToTeacherVm == null)
                    throw new ArgumentNullException("View model parameter is null");

                _ = DbContext.Users.OfType<Teacher>()
                    .FirstOrDefault(t => t.Id == attachSubjectToTeacherVm.TeacherId)
                    ?? throw new ArgumentException($"Teacher {attachSubjectToTeacherVm.TeacherId} not found.");

                var subject = DbContext.Subjects.FirstOrDefault(s => s.Id == attachSubjectToTeacherVm.SubjectId)
                    ?? throw new ArgumentException($"Subject {attachSubjectToTeacherVm.SubjectId} not found.");

                subject.TeacherId = attachSubjectToTeacherVm.TeacherId;
                DbContext.SaveChanges();
                return Mapper.Map<SubjectVm>(subject);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public SubjectVm DetachTeacherFromSubject(AttachDetachSubjectToTeacherVm attachSubjectToTeacherVm)
        {
            try
            {
                if (attachSubjectToTeacherVm == null)
                    throw new ArgumentNullException("View model parameter is null");

                var subject = DbContext.Subjects.FirstOrDefault(s =>
                    s.Id == attachSubjectToTeacherVm.SubjectId
                    && s.TeacherId == attachSubjectToTeacherVm.TeacherId)
                    ?? throw new ArgumentException(
                        $"Subject {attachSubjectToTeacherVm.SubjectId} is not assigned to teacher {attachSubjectToTeacherVm.TeacherId}.");

                subject.TeacherId = null;
                subject.Teacher = null; // clear cached navigation property
                DbContext.SaveChanges();
                return Mapper.Map<SubjectVm>(subject);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}
