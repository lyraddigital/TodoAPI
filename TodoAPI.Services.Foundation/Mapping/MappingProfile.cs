using AutoMapper;
using TodoAPI.Repository.Core.Models;
using TodoAPI.Services.Core.Models;

namespace TodoAPI.Services.Foundation.Mapping
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<UpdatedTodoDTO, Todo>();
            CreateMap<CreatedTodoDTO, TodoDTO>()
                .ForMember(s => s.Id, m => m.Ignore());
            CreateMap<CreatedTodoDTO, Todo>()
                .ForMember(s => s.Id, m => m.Ignore());
            CreateMap<Todo, TodoDTO>();
        }
    }
}
