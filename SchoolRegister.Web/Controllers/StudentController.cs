using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using SchoolRegister.Model.DataModels;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;

namespace SchoolRegister.Web.Controllers;

[Authorize(Roles = "Admin, Teacher, Student, Parent")]
public class StudentController : BaseController
{
    private readonly IStudentService _studentService;
    private readonly IGradeService _gradeService;
    private readonly UserManager<User> _userManager;

    public StudentController(
        IStudentService studentService,
        IGradeService gradeService,
        UserManager<User> userManager,
        IStringLocalizer localizer,
        ILogger logger,
        IMapper mapper) : base(logger, mapper, localizer)
    {
        _studentService = studentService;
        _gradeService = gradeService;
        _userManager = userManager;
    }

    // Lista studentów – Admin i Teacher widzą wszystkich, Parent widzi swoje dzieci
    [Authorize(Roles = "Admin, Teacher, Parent")]
    public IActionResult Index()
    {
        var user = _userManager.GetUserAsync(User).Result;

        if (_userManager.IsInRoleAsync(user, "Parent").Result && user is Parent parent)
        {
            var children = _studentService.GetStudents(s => s.ParentId == parent.Id);
            return View(children);
        }

        return View(_studentService.GetStudents());
    }

    // Szczegóły studenta + jego oceny
    public IActionResult Details(int studentId)
    {
        var currentUser = _userManager.GetUserAsync(User).Result;

        var studentVm = _studentService.GetStudent(s => s.Id == studentId);
        if (studentVm == null)
            return NotFound();

        var gradesReport = _gradeService.GetGradesReportForStudent(new GetGradesReportVm
        {
            StudentId = studentId,
            GetterUserId = currentUser.Id
        });

        ViewBag.Student = studentVm;
        return View(gradesReport);
    }
}
