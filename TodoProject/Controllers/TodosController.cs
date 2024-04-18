using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoProject.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TodoProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodosController : ControllerBase
    {
        private readonly TodoContext _context;

        public TodosController(TodoContext context)
        {
            _context = context;
        }

        // GET: api/Todos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Todo>>> GetTodos()
        {
            return await _context.Todos.ToListAsync();
        }

        // GET: api/Todos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Todo>> GetTodo(int id)
        {
            var todo = await _context.Todos.FindAsync(id);

            if (todo == null)
            {
                return NotFound();
            }

            return todo;
        }

        // GET: api/Todos/ByPriority/high
        [HttpGet("ByPriority/{priorityId}")]
        public async Task<ActionResult<IEnumerable<Todo>>> GetTodosByPriority(string priorityId)
        {
            return await _context.Todos.Where(t => t.PriorityId == priorityId).ToListAsync();
        }

        // GET: api/Todos/ByCategory/work
        [HttpGet("ByCategory/{categoryId}")]
        public async Task<ActionResult<IEnumerable<Todo>>> GetTodosByCategory(string categoryId)
        {
            return await _context.Todos.Where(t => t.CategoryId == categoryId).ToListAsync();
        }

        // GET: api/Todos/ByStatus/open
        [HttpGet("ByStatus/{statusId}")]
        public async Task<ActionResult<IEnumerable<Todo>>> GetTodosByStatus(string statusId)
        {
            return await _context.Todos.Where(t => t.StatusId == statusId).ToListAsync();
        }

        // PUT: api/Todos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodo(int id, Todo todo)
        {
            // Check if the provided ID matches the ID of the todo object
            if (id != todo.Id)
            {
                return BadRequest("ID in URL does not match ID in request body.");
            }

            // Check if the todo with the provided ID exists in the database
            var existingTodo = await _context.Todos.FindAsync(id);
            if (existingTodo == null)
            {
                return NotFound("Todo not found.");
            }

            // Update properties of the existing todo with data from the request body
            existingTodo.Description = todo.Description;
            existingTodo.PriorityId = todo.PriorityId;
            existingTodo.CategoryId = todo.CategoryId;
            existingTodo.StatusId = todo.StatusId;

            // Save changes to the database
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // If the update fails due to concurrency conflict, handle it appropriately
                return Conflict("Concurrency conflict occurred.");
            }

            // Return a 204 No Content response indicating success
            return NoContent();
        }

        // POST: api/Todos
        [HttpPost]
        public async Task<ActionResult<Todo>> PostTodo(Todo todo)
        {
            // Check if the Category, Priority, and Status IDs exist in their respective tables
            var existingCategory = await _context.Categories.FindAsync(todo.CategoryId);
            var existingPriority = await _context.Priorities.FindAsync(todo.PriorityId);
            var existingStatus = await _context.Statuses.FindAsync(todo.StatusId);

            // If any of the referenced entities do not exist, return a 404 Not Found response
            if (existingCategory == null || existingPriority == null || existingStatus == null)
            {
                return NotFound("One or more referenced entities not found.");
            }

            // Assign the existing Category, Priority, and Status entities to the todo
            todo.Category = existingCategory;
            todo.Priority = existingPriority;
            todo.Status = existingStatus;

            // Add the todo to the context and save changes
            _context.Todos.Add(todo);
            await _context.SaveChangesAsync();

            // Return a 201 Created response with the created todo
            return CreatedAtAction(nameof(GetTodo), new { id = todo.Id }, todo);
        }

        // DELETE: api/Todos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodo(int id)
        {
            var todo = await _context.Todos.FindAsync(id);
            if (todo == null)
            {
                return NotFound();
            }

            _context.Todos.Remove(todo);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        // GET: api/Priorities
        [HttpGet("~/api/Priorities")]
        public async Task<ActionResult<IEnumerable<Priority>>> GetPriorities()
        {
            return await _context.Priorities.ToListAsync();
        }

        // GET: api/Categories
        [HttpGet("~/api/Categories")]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
            return await _context.Categories.ToListAsync();
        }

        // GET: api/Statuses
        [HttpGet("~/api/Statuses")]
        public async Task<ActionResult<IEnumerable<Status>>> GetStatuses()
        {
            return await _context.Statuses.ToListAsync();
        }


        private bool TodoExists(int id)
        {
            return _context.Todos.Any(e => e.Id == id);
        }
    }
}
