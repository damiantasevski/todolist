using AutoMapper;
using ToDoList.Data.Entities;
using Task = ToDoList.Data.Entities.Task;

namespace ToDoList.Models
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<TasksListViewModel, TasksList>()
                .ForMember(dest => dest.Tasks, opt => opt.MapFrom(src => src.Tasks)).ReverseMap();

            CreateMap<TaskViewModel, Task>().ReverseMap();
        }
    }
}
