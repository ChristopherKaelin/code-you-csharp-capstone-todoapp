using System.Net.Http;
using System.Text;
using System.Text.Json;
using TodoApp.Api.Models;

var client = new HttpClient();
client.BaseAddress = new Uri("http://localhost:1221");

var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

while (true)
{
    Console.WriteLine("\n=== Todo App ===");
    Console.WriteLine("1. List all todos");
    Console.WriteLine("2. List overdue todos");
    Console.WriteLine("3. Get todo by ID");
    Console.WriteLine("4. Create todo");
    Console.WriteLine("5. Create sub todo");
    Console.WriteLine("6. Update todo");
    Console.WriteLine("7. Delete todo");
    Console.WriteLine("0. Exit");
    Console.Write("\nSelect an option: ");

    var choice = Console.ReadLine();

    switch (choice)
    {
        case "1":
            await ListTodos();
            break;
        case "2":
            await ListOverdueTodos();
            break;
        case "3":
            await ListTodoItem();
            break;
        case "4":
            await CreateTodo();
            break;
        case "5":
            await CreateSubTodo();
            break;
        case "6":
            await UpdateTodo();
            break;
        case "7":
            await DeleteTodo();
            break;
        case "0":
            return;
        default:
            Console.WriteLine("Invalid option.");
            break;
    }
}

//  List all todos
async Task ListTodos()
{
    var todoList = await RetrieveTodoList();

    if (todoList == null || todoList.Count == 0)
    {
        Console.WriteLine("No todos found.");
        return;
    }
    else
    {
        DisplayTodoList(todoList);
    }
}

//  List overdue todos
// async Task ListOverdueTodos()
// {
//     var todoList = await RetrieveTodoList();

//     var overdue = todoList?
//         .Where(t => t.DueDate.HasValue && t.DueDate.Value < DateOnly.FromDateTime(DateTime.Today) && !t.IsComplete)
//         .ToList();

//     Console.WriteLine($"Overdue count: {overdue?.Count}");

//     if (overdue == null || overdue.Count == 0)
//     {
//         Console.WriteLine("No overdue todos found.");
//         return;
//     }
//     else
//     {
//         DisplayTodoList(overdue);
//     }
// }

async Task ListOverdueTodos()
{
    var todoList = await RetrieveTodoList();

    var overdue = todoList?
        .Where(t => t.DueDate.HasValue && t.DueDate.Value < DateOnly.FromDateTime(DateTime.Today) && !t.IsComplete)
        .ToList();

    if (overdue == null || overdue.Count == 0)
    {
        Console.WriteLine("No overdue todos found.");
        return;
    }

    foreach (var todo in overdue)
    {
        if (todo.ParentId.HasValue)
        {
            var parent = todoList?.FirstOrDefault(t => t.Id == todo.ParentId);
            if (parent != null)
                Console.WriteLine($"ID: {parent.Id} | {parent.Title} | Status: {(parent.IsComplete ? "Complete" : "Incomplete")}");
        }

        string indent = todo.ParentId.HasValue ? "  -> " : "";
        Console.WriteLine($"{indent}ID: {todo.Id} | {todo.Title} | Status: {(todo.IsComplete ? "Complete" : "Incomplete")} | Due: {todo.DueDate}");
    }
}

//  Get todo by ID
async Task ListTodoItem()
{
    await ListTodos();
    var id = PromptForId("Enter todo ID");
    if (id == null)
        return;

    var response = await client.GetAsync($"/api/todo/{id}");

    if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
    {
        Console.WriteLine("Todo not found.");
        return;
    }

    var json = await response.Content.ReadAsStringAsync();
    var todo = JsonSerializer.Deserialize<TodoItem>(json, options);

    Console.WriteLine($"Todo Item: {todo!.Id} - {todo.Title}");
    Console.WriteLine($"  Description: {todo.Description}");
    Console.WriteLine($"  Due: {todo.DueDate?.ToString("yyyy-MM-dd") ?? "None"}");
    Console.WriteLine($"  Priority: {todo.Priority}");
    Console.WriteLine($"  Status: {(todo.IsComplete ? "Complete" : "Incomplete")}");

    var allTodos = await RetrieveTodoList();
    var children = allTodos?.Where(t => t.ParentId == id).ToList();

    if (children != null && children.Count > 0)
    {
        Console.WriteLine("\n  Sub-tasks:");
        DisplaySubTodoList(children);
    }
}

//  Create todo
async Task CreateTodo()
{
    var todo = await PromptForTodoDetails();

    var json = JsonSerializer.Serialize(todo);
    var content = new StringContent(json, Encoding.UTF8, "application/json");
    var response = await client.PostAsync("/api/todo", content);

    if (response.IsSuccessStatusCode)
        Console.WriteLine("Todo created successfully.");
    else
        Console.WriteLine("Failed to create todo.");
}

//  Create sub todo
async Task CreateSubTodo()
{
    await ListTodos();
    var parentId = PromptForId("Enter parent todo ID");
    if (parentId == null)
        return;

    var getResponse = await client.GetAsync($"/api/todo/{parentId}");

    if (getResponse.StatusCode == System.Net.HttpStatusCode.NotFound)
    {
        Console.WriteLine("Parent todo not found.");
        return;
    }

    var todo = await PromptForTodoDetails();
    todo.ParentId = parentId;

    var json = JsonSerializer.Serialize(todo);
    var content = new StringContent(json, Encoding.UTF8, "application/json");
    var response = await client.PostAsync("/api/todo", content);

    if (response.IsSuccessStatusCode)
        Console.WriteLine("Todo created successfully.");
    else
        Console.WriteLine("Failed to create todo.");
}

//  Update todo
async Task UpdateTodo()
{
    await ListTodos();
    var id = PromptForId("Enter todo ID to update");
    if (id == null)
        return;

    var getResponse = await client.GetAsync($"/api/todo/{id}");

    if (getResponse.StatusCode == System.Net.HttpStatusCode.NotFound)
    {
        Console.WriteLine("Todo not found.");
        return;
    }

    var existingJson = await getResponse.Content.ReadAsStringAsync();
    var todo = JsonSerializer.Deserialize<TodoItem>(existingJson, options)!;

    Console.Write($"Title ({todo.Title}): ");
    var title = Console.ReadLine();
    if (!string.IsNullOrWhiteSpace(title)) todo.Title = title;

    Console.Write($"Due date ({todo.DueDate?.ToString("yyyy-MM-dd") ?? "None"}): ");
    var dueDateInput = Console.ReadLine();
    if (!string.IsNullOrWhiteSpace(dueDateInput)) todo.DueDate = DateOnly.Parse(dueDateInput);

    Console.Write($"Priority ({todo.Priority}): ");
    var priorityInput = Console.ReadLine();
    if (!string.IsNullOrWhiteSpace(priorityInput)) todo.Priority = int.Parse(priorityInput);

    Console.Write($"Complete ({todo.IsComplete}): ");
    var completeInput = Console.ReadLine();
    if (!string.IsNullOrWhiteSpace(completeInput)) todo.IsComplete = bool.Parse(completeInput);

    var json = JsonSerializer.Serialize(todo);
    var content = new StringContent(json, Encoding.UTF8, "application/json");
    var response = await client.PutAsync($"/api/todo/{id}", content);

    if (response.IsSuccessStatusCode)
        Console.WriteLine("Todo updated successfully.");
    else
        Console.WriteLine("Failed to update todo.");
}

//  Delete todo
async Task DeleteTodo()
{
    await ListTodos();
    var id = PromptForId("Enter todo ID to delete");
    if (id == null)
        return;

    var response = await client.DeleteAsync($"/api/todo/{id}");

    if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
    {
        Console.WriteLine("Todo not found.");
        return;
    }

    if (response.IsSuccessStatusCode)
        Console.WriteLine("Todo deleted successfully.");
    else
        Console.WriteLine("Failed to delete todo.");
}

//  Retrieve complete list of todo items
async Task<List<TodoItem>?> RetrieveTodoList()
{
    var response = await client.GetAsync("/api/todo");
    var json = await response.Content.ReadAsStringAsync();
    var todos = JsonSerializer.Deserialize<List<TodoItem>>(json, options);

    return todos;
}

//  Display list of todo items
void DisplayTodoList(List<TodoItem> todos)
{
    var parents = todos.Where(t => t.ParentId == null).ToList();

    foreach (var todo in parents)
    {
        bool hasChildren = todos.Any(t => t.ParentId == todo.Id);
        string childIndicator = hasChildren ? "[+]" : "";
        Console.WriteLine($"ID: {todo.Id} {childIndicator} | {todo.Title} | Status: {(todo.IsComplete ? "Complete" : "Incomplete")}");
    }
}

//  Display list of sub todo items
void DisplaySubTodoList(List<TodoItem> todos)
{
    foreach (var todo in todos)
    {
        Console.WriteLine($"    ID: {todo.Id} | {todo.Title} | Status: {(todo.IsComplete ? "Complete" : "Incomplete")} | Due: {todo.DueDate}");
    }
}

//  Prompts the user for todo details and returns a new TodoItem.
async Task<TodoItem> PromptForTodoDetails()
{
    string? title;
    do
    {
        Console.Write("Title: ");
        title = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(title))
            Console.WriteLine("Title is required.");
    } while (string.IsNullOrWhiteSpace(title));

    Console.Write("Description: ");
    var description = Console.ReadLine();

    Console.Write("Due date (yyyy-MM-dd) or leave blank: ");
    var dueDateInput = Console.ReadLine();
    DateOnly? dueDate = string.IsNullOrWhiteSpace(dueDateInput) ? null : DateOnly.Parse(dueDateInput);

    Console.Write("Priority (1-5): ");
    var priority = int.Parse(Console.ReadLine()!);

    return new TodoItem
    {
        Title = title!,
        Description = description,
        DueDate = dueDate,
        Priority = priority,
        IsComplete = false
    };

}

//  Prompts the user for Todo item Id and returns an integer
int? PromptForId(string prompt)
{
    while (true)
    {
        Console.Write($"\n{prompt} (0 to cancel): ");
        var input = Console.ReadLine();

        if (!int.TryParse(input, out int id))
        {
            Console.WriteLine("Invalid input, please enter a number.");
            continue;
        }

        if (id == 0) return null;

        return id;
    }
}