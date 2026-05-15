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

[Authorize(Roles = "Admin")]
public class GroupController : BaseController
{
    private readonly IGroupService _groupService;
    private readonly IStudentService _studentService;
    private readonly ISubjectService _subjectService;
    private readonly UserManager<User> _userManager;

    public GroupController(
        IGroupService groupService,
        IStudentService studentService,
        ISubjectService subjectService,
        UserManager<User> userManager,
        IStringLocalizer localizer,
        ILogger logger,
        IMapper mapper) : base(logger, mapper, localizer)
    {
        _groupService = groupService;
        _studentService = studentService;
        _subjectService = subjectService;
        _userManager = userManager;
    }

    // Lista wszystkich grup
    public IActionResult Index()
    {
        var groups = _groupService.GetGroups();
        return View(groups);
    }

    // Formularz tworzenia/edycji grupy (GET)
    [HttpGet]
    public IActionResult AddOrEditGroup(int? id = null)
    {
        if (id.HasValue)
        {
            var groupVm = _groupService.GetGroup(x => x.Id == id);
            ViewBag.ActionType = "Edit";
            return View(Mapper.Map<AddOrUpdateGroupVm>(groupVm));
        }
        ViewBag.ActionType = "Add";
        return View();
    }

    // Zapis grupy (POST)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult AddOrEditGroup(AddOrUpdateGroupVm addOrUpdateGroupVm)
    {
        if (ModelState.IsValid)
        {
            _groupService.AddOrUpdateGroup(addOrUpdateGroupVm);
            return RedirectToAction("Index");
        }
        return View();
    }

    // Formularz dodawania studenta do grupy (GET)
    [HttpGet]
    public IActionResult AttachStudentToGroup(int groupId)
    {
        var group = _groupService.GetGroup(x => x.Id == groupId);
        // Studenci bez grupy lub z inną grupą
        var students = _studentService.GetStudents();
        ViewBag.StudentsSelectList = new SelectList(students.Select(s => new
        {
            Text = $"{s.FirstName} {s.LastName}",
            Value = s.Id
        }), "Value", "Text");
        ViewBag.GroupName = group?.Name;
        return View(new AttachDetachStudentToGroupVm { GroupId = groupId });
    }

    // Zapis dodania studenta do grupy (POST)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult AttachStudentToGroup(AttachDetachStudentToGroupVm vm)
    {
        if (ModelState.IsValid)
        {
            _groupService.AttachStudentToGroup(vm);
            return RedirectToAction("Index");
        }
        return View(vm);
    }

    // Formularz usuwania studenta z grupy (GET)
    [HttpGet]
    public IActionResult DetachStudentFromGroup(int groupId)
    {
        var group = _groupService.GetGroup(x => x.Id == groupId);
        var students = _studentService.GetStudents(s => s.GroupId == groupId);
        ViewBag.StudentsSelectList = new SelectList(students.Select(s => new
        {
            Text = $"{s.FirstName} {s.LastName}",
            Value = s.Id
        }), "Value", "Text");
        ViewBag.GroupName = group?.Name;
        return View(new AttachDetachStudentToGroupVm { GroupId = groupId });
    }

    // Zapis usunięcia studenta z grupy (POST)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult DetachStudentFromGroup(AttachDetachStudentToGroupVm vm)
    {
        if (ModelState.IsValid)
        {
            _groupService.DetachStudentFromGroup(vm);
            return RedirectToAction("Index");
        }
        return View(vm);
    }

    // Formularz przypisania przedmiotu do grupy (GET) - wywoływany z Subject/Index
    [HttpGet]
    public IActionResult AttachSubjectToGroup(int subjectId)
    {
        var groups = _groupService.GetGroups();
        ViewBag.GroupsSelectList = new SelectList(groups.Select(g => new
        {
            Text = g.Name,
            Value = g.Id
        }), "Value", "Text");
        ViewBag.SubjectId = subjectId;
        return View(new AttachDetachSubjectGroupVm { SubjectId = subjectId });
    }

    // Zapis przypisania przedmiotu do grupy (POST)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult AttachSubjectToGroup(AttachDetachSubjectGroupVm vm)
    {
        if (ModelState.IsValid)
        {
            _groupService.AttachSubjectToGroup(vm);
            return RedirectToAction("Index", "Subject");
        }
        return View(vm);
    }

    // Formularz odpięcia przedmiotu od grupy (GET) - wywoływany z Subject/Index
    [HttpGet]
    public IActionResult DetachSubjectToGroup(int subjectId)
    {
        var subject = _subjectService.GetSubject(x => x.Id == subjectId);
        var groupsForSubject = subject?.Groups ?? new List<GroupVm>();
        ViewBag.GroupsSelectList = new SelectList(groupsForSubject.Select(g => new
        {
            Text = g.Name,
            Value = g.Id
        }), "Value", "Text");
        ViewBag.SubjectId = subjectId;
        return View(new AttachDetachSubjectGroupVm { SubjectId = subjectId });
    }

    // Zapis odpięcia przedmiotu od grupy (POST)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult DetachSubjectToGroup(AttachDetachSubjectGroupVm vm)
    {
        if (ModelState.IsValid)
        {
            _groupService.DetachSubjectFromGroup(vm);
            return RedirectToAction("Index", "Subject");
        }
        return View(vm);
    }
}
