using System;
using System.Collections.Generic;
using System.Linq;
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
    public class GradeService : BaseService, IGradeService
    {
        private readonly UserManager<User> _userManager;

        public GradeService(ApplicationDbContext dbContext, IMapper mapper, ILogger logger,
            UserManager<User> userManager) : base(dbContext, mapper, logger)
        {
            _userManager = userManager;
        }

        public GradeVm AddGradeToStudent(AddGradeToStudentVm addGradeToStudentVm)
        {
            try
            {
                if (addGradeToStudentVm == null)
                    throw new ArgumentNullException("View model parameter is null");

                // Verify teacher exists
                var teacher = DbContext.Users.OfType<Teacher>()
                    .FirstOrDefault(t => t.Id == addGradeToStudentVm.TeacherId)
                    ?? throw new ArgumentException($"Teacher with id {addGradeToStudentVm.TeacherId} not found.");

                // Verify teacher teaches the subject
                var subject = DbContext.Subjects
                    .FirstOrDefault(s => s.Id == addGradeToStudentVm.SubjectId
                                         && s.TeacherId == addGradeToStudentVm.TeacherId)
                    ?? throw new ArgumentException(
                        $"Teacher {addGradeToStudentVm.TeacherId} does not teach subject {addGradeToStudentVm.SubjectId}.");

                // Verify student exists
                var student = DbContext.Users.OfType<Student>()
                    .FirstOrDefault(s => s.Id == addGradeToStudentVm.StudentId)
                    ?? throw new ArgumentException($"Student with id {addGradeToStudentVm.StudentId} not found.");

                var grade = new Grade
                {
                    DateOfIssue = DateTime.Now,
                    GradeValue = addGradeToStudentVm.GradeValue,
                    SubjectId = addGradeToStudentVm.SubjectId,
                    StudentId = addGradeToStudentVm.StudentId
                };

                DbContext.Grades.Add(grade);
                DbContext.SaveChanges();
                return Mapper.Map<GradeVm>(grade);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public GradesReportVm GetGradesReportForStudent(GetGradesReportVm getGradesVm)
        {
            try
            {
                if (getGradesVm == null)
                    throw new ArgumentNullException("View model parameter is null");

                // Find student
                var student = DbContext.Users.OfType<Student>()
                    .FirstOrDefault(s => s.Id == getGradesVm.StudentId)
                    ?? throw new ArgumentException($"Student with id {getGradesVm.StudentId} not found.");

                // Determine access rights using type checking (TPH discriminator)
                var getterUser = DbContext.Users.FirstOrDefault(u => u.Id == getGradesVm.GetterUserId);
                bool isAdmin = getterUser != null && _userManager.IsInRoleAsync(getterUser, "Admin").Result;

                bool isTeacher = DbContext.Users.OfType<Teacher>()
                    .Any(t => t.Id == getGradesVm.GetterUserId);

                bool isOwnStudent = DbContext.Users.OfType<Student>()
                    .Any(s => s.Id == getGradesVm.GetterUserId && s.Id == getGradesVm.StudentId);

                bool isParentOfStudent = DbContext.Users.OfType<Parent>()
                    .Any(p => p.Id == getGradesVm.GetterUserId
                              && DbContext.Users.OfType<Student>()
                                  .Any(s => s.Id == getGradesVm.StudentId && s.ParentId == p.Id));

                if (!isAdmin && !isTeacher && !isOwnStudent && !isParentOfStudent)
                    throw new UnauthorizedAccessException(
                        $"User {getGradesVm.GetterUserId} is not authorized to view grades of student {getGradesVm.StudentId}.");

                var grades = DbContext.Grades
                    .Include(g => g.Subject)
                    .Where(g => g.StudentId == student.Id).ToList();

                return new GradesReportVm
                {
                    StudentId = student.Id,
                    StudentFirstName = student.FirstName,
                    StudentLastName = student.LastName,
                    Grades = Mapper.Map<IList<GradeVm>>(grades)
                };
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}
