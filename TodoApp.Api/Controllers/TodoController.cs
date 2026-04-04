using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using TodoApp.Api.Models;

namespace TodoApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodoController : ControllerBase
    {
        private List<TodoItem> Todos { get; } = new List<TodoItem>();

        public TodoController()
        {
            string jsonFile = System.IO.File.ReadAllText("./Resources/todos.json");
            var todoData = JsonSerializer.Deserialize<List<TodoItem>>(
                jsonFile,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (todoData != null)
            {
                Todos = todoData;
            }
        }

        // GET: api/todos
        [HttpGet]
        public ActionResult<List<TodoItem>> GetTodos()
        {
            var sortedTodos = Todos
                .OrderBy(t => t.DueDate == null)
                .ThenBy(t => t.DueDate)
                .ThenBy(t => t.Priority)
                .ToList();

            return Ok(sortedTodos);
        }

        // GET: api/todos/{id}
        [HttpGet("{id}")]
        public ActionResult<TodoItem> GetTodoItem(int id)
        {
            var todoItem = Todos.FirstOrDefault(t => t.Id == id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return Ok(todoItem);
        }

        // POST: api/todos
        [HttpPost]
        public ActionResult<TodoItem> CreateTodoItem(TodoItem todoItem)
        {
            todoItem.Id = Todos.Max(t => t.Id) + 1;
            Todos.Add(todoItem);
            SaveTodos();
            return CreatedAtAction(nameof(GetTodoItem), new { id = todoItem.Id }, todoItem);
        }

        // PUT: api/todos/{id}
        [HttpPut("{id}")]
        public ActionResult UpdateTodoItem(int id, TodoItem todoItem)
        {
            var existing = Todos.FirstOrDefault(t => t.Id == id);

            if (existing == null)
            {
                return NotFound();
            }

            existing.Title = todoItem.Title;
            existing.Description = todoItem.Description;
            existing.DueDate = todoItem.DueDate;
            existing.Priority = todoItem.Priority;
            existing.IsComplete = todoItem.IsComplete;

            SaveTodos();
            return NoContent();
        }

        // DELETE: api/todos/{id}
        [HttpDelete("{id}")]
        public ActionResult DeleteTodoItem(int id)
        {
            var existing = Todos.FirstOrDefault(t => t.Id == id);

            if (existing == null)
            {
                return NotFound();
            }

            Todos.Remove(existing);
            SaveTodos();
            return NoContent();
        }

        private void SaveTodos()
        {
            System.IO.File.WriteAllText("./Resources/todos.json", JsonSerializer.Serialize(Todos));
        }

    }
}
