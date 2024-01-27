using API.Data;
using Microsoft.EntityFrameworkCore;

namespace API.Services
{
    public class TodoService : ITodoService
    {
        private readonly TodoContext _todoContext;

        public TodoService(TodoContext todoContext)
        {
            _todoContext = todoContext;
        }

        public async Task<IEnumerable<Todo>> Get(Guid[] ids, Filters filters)
        {
            var todos = _todoContext.Todos.AsQueryable();

            if (filters == null)
                filters = new Filters();

            if (filters.Tasks != null && filters.Tasks.Any())
                todos = todos.Where(x => filters.Tasks.Contains(x.Task));

            if (filters.Done != null && filters.Done.Any())
                todos = todos.Where(x => filters.Done.Contains(x.Done));

            if (ids != null && ids.Any())
                todos = todos.Where(x => ids.Contains(x.Id));

            //await Task.Delay(2000);

            return await todos.ToListAsync();
        }

        public async Task<Todo> Post(Todo model)
        {
            await _todoContext.Todos.AddAsync(model);
            await _todoContext.SaveChangesAsync();
            return model;
        }

        public async Task<IEnumerable<Todo>> AddRange(IEnumerable<Todo> todos)
        {
            await _todoContext.Todos.AddRangeAsync(todos);
            await _todoContext.SaveChangesAsync();
            return todos;
        }

        public async Task<Todo> Update(Guid id, Todo model)
        {
            var dataForChanges = await _todoContext.Todos.SingleAsync(x => x.Id == id);

            dataForChanges.Task = model.Task;
            dataForChanges.Done = model.Done;

            _todoContext.Todos.Update(dataForChanges);
            await _todoContext.SaveChangesAsync();
            return dataForChanges;
        }

        public async Task<bool> Delete(Todo model)
        {
            _todoContext.Todos.Remove(model);
            await _todoContext.SaveChangesAsync();

            return true;
        }
    }
    public class Filters
    {
        public string[]? Tasks { get; set; }
        public bool[]? Done { get; set; }
    }
}
