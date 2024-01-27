using API.Data;
using API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodoItemsController : ControllerBase
    {
        private readonly IMemoryCache _cache;
        private readonly ITodoService _todoService;

        public TodoItemsController(ITodoService todoService, ILogger<TodoItemsController> logger, IMemoryCache memoryCache)
        {
            _todoService = todoService;
            _cache = memoryCache;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            //failfast validation
            if (id == Guid.Empty) return BadRequest();
            var data = (await _todoService.Get([id], null)).FirstOrDefault();
            if (data == null)
                return NotFound();
            return Ok(data);
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAll([FromQuery] Filters filters)
        {
            var CurrentDateTime = DateTime.Now;
            var cacheEntry = await _cache.GetOrCreate("TodoCacheKey", async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(10);
                entry.SetPriority(CacheItemPriority.High);

                return await _todoService.Get(null, filters);
            });
            return Ok(cacheEntry);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Todo todo)
        {
            await _todoService.Post(todo);
            return CreatedAtAction(nameof(Post), todo);

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] Todo todo)
        {
            //failfast validation
            if (todo is null || id == Guid.Empty) return BadRequest();
            await _todoService.Update(id, todo);
            return Ok(todo);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var data = (await _todoService.Get(new[] { id }, null)).FirstOrDefault();
            if (data == null)
                return NotFound();
            _cache.Remove("TodoCacheKey");
            await _todoService.Delete(data);
            return NoContent();
        }
    }
}
