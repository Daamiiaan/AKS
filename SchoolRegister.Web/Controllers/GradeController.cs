using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using SchoolRegister.Model.DataModels;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;

namespace SchoolRegister.Web.Controllers;

[Authorize(Roles = "Teacher, Admin, Student, Parent")]
public class GradeController : BaseController
{
    private readonly IGradeService _gradeService;
    private readonly IStudentService _studentService;
    private readonly ISubjectService _subjectService;
    private readonly UserManager<User> _userManager;

    public GradeController(
        IGradeService gradeService,
        IStudentService studentService,
        ISubjectService subjectService,
        UserManager<User> userManager,
        IStringLocalizer localizer,
        ILogger logger,
        IMapper mapper) : base(logger, mapper, localizer)
    {
        _gradeService = gradeService;
        _studentService = studentService;
        _subjectService = subjectService;
        _userManager = userManager;
    }

    // Formularz wystawienia oceny (GET) – tylko Teacher
    [HttpGet]
    [Authorize(Roles = "Teacher")]
    public IActionResult AddGrade(int studentId)
    {
        var teacher = _userManager.GetUserAsync(User).Result as Teacher;
        if (teacher == null) return Forbid();

        var student = _studentService.GetStudent(s => s.Id == studentId);
        if (student == null) return NotFound();

        // Tylko przedmioty prowadzone przez tego nauczyciela
        var subjects = _subjectService.GetSubjects(s => s.TeacherId == teacher.Id);
        ViewBag.SubjectsSelectList = new SelectList(subjects.Select(s => new
        {
            Text = s.Name,
            Value = s.Id
        }), "Value", "Text");

        // Wartości ocen
        ViewBag.GradeValues = new SelectList(Enum.GetValues(typeof(GradeScale))
            .Cast<GradeScale>()
            .Select(g => new { Text = g.ToString(), Value = (int)g }),
            "Value", "Text");

        ViewBag.StudentName = $"{student.FirstName} {student.LastName}";

        return View(new AddGradeToStudentVm
        {
            StudentId = studentId,
            TeacherId = teacher.Id
        });
    }

    // Zapis oceny (POST) – tylko Teacher
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Teacher")]
    public IActionResult AddGrade(AddGradeToStudentVm vm)
    {
        if (ModelState.IsValid)
        {
            _gradeService.AddGradeToStudent(vm);
            return RedirectToAction("Details", "Student", new { studentId = vm.StudentId });
        }
        return View(vm);
    }
}
