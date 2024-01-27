using API.Data;

namespace API.Services
{
    public interface ITodoService
    {
        Task<IEnumerable<Todo>> Get(Guid[] ids, Filters filters);

        Task<Todo> Post(Todo model);

        Task<IEnumerable<Todo>> AddRange(IEnumerable<Todo> list);

        Task<Todo> Update(Guid id, Todo model);

        Task<bool> Delete(Todo model);
    }
}