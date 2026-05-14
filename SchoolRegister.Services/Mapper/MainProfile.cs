using AutoMapper;
using SchoolRegister.Model.DataModels;
using SchoolRegister.ViewModels.VM;

namespace SchoolRegister.Services.Mapper
{
    public class MainProfile : Profile
    {
        public MainProfile()
        {
            // Subject mappings
            CreateMap<Subject, SubjectVm>()
                .ForMember(dest => dest.TeacherName, opt => opt.MapFrom(src =>
                    src.Teacher != null ? $"{src.Teacher.FirstName} {src.Teacher.LastName}" : null));

            CreateMap<AddOrUpdateSubjectVm, Subject>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id ?? 0));

            // Group mappings
            CreateMap<Group, GroupVm>()
                .ForMember(dest => dest.Students, opt => opt.MapFrom(src => src.Students))
                .ForMember(dest => dest.Subjects, opt => opt.MapFrom(src =>
                    src.SubjectGroups != null ? src.SubjectGroups.Select(sg => sg.Subject) : null));

            CreateMap<AddOrUpdateGroupVm, Group>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id ?? 0));

            // Student mappings
            CreateMap<Student, StudentVm>()
                .ForMember(dest => dest.GroupName, opt => opt.MapFrom(src =>
                    src.Group != null ? src.Group.Name : null))
                .ForMember(dest => dest.ParentName, opt => opt.MapFrom(src =>
                    src.Parent != null ? $"{src.Parent.FirstName} {src.Parent.LastName}" : null));

            // Teacher mappings
            CreateMap<Teacher, TeacherVm>()
                .ForMember(dest => dest.Subjects, opt => opt.MapFrom(src => src.Subjects));

            // Grade mappings
            CreateMap<Grade, GradeVm>()
                .ForMember(dest => dest.SubjectName, opt => opt.MapFrom(src =>
                    src.Subject != null ? src.Subject.Name : null));
        }
    }
}
