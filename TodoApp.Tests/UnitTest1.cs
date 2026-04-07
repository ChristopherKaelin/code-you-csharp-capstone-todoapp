using Microsoft.AspNetCore.Mvc;
using TodoApp.Api.Controllers;
using TodoApp.Api.Models;

namespace TodoApp.Tests;

public class UnitTest1
{
    private List<TodoItem> GetTestTodos() => new List<TodoItem>
    {
        new TodoItem { Id = 1, Title = "Buy groceries", Priority = 2, IsComplete = false },
        new TodoItem { Id = 2, Title = "Walk the dog", Priority = 1, IsComplete = false },
        new TodoItem { Id = 3, Title = "Read a book", Priority = 3, IsComplete = true }
    };

    [Fact]
    public void GetTodos_ReturnsSortedList()
    {
        var controller = new TodoController(GetTestTodos());
        var result = controller.GetTodos();

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        var todos = Assert.IsType<List<TodoItem>>(ok.Value);
        Assert.Equal(3, todos.Count);
    }

    [Fact]
    public void GetTodoItem_ReturnsCorrectItem()
    {
        var controller = new TodoController(GetTestTodos());
        var result = controller.GetTodoItem(1);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        var todo = Assert.IsType<TodoItem>(ok.Value);
        Assert.Equal("Buy groceries", todo.Title);
    }

    [Fact]
    public void GetTodoItem_ReturnsNotFound_WhenMissing()
    {
        var controller = new TodoController(GetTestTodos());
        var result = controller.GetTodoItem(999);

        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public void CreateTodoItem_AddsItem()
    {
        var controller = new TodoController(GetTestTodos());
        var newItem = new TodoItem { Title = "New task", Priority = 1 };

        var result = controller.CreateTodoItem(newItem);

        var created = Assert.IsType<CreatedAtActionResult>(result.Result);
        var todo = Assert.IsType<TodoItem>(created.Value);
        Assert.Equal("New task", todo.Title);
        Assert.Equal(4, todo.Id);
    }

    [Fact]
    public void UpdateTodoItem_ReturnsNotFound_WhenMissing()
    {
        var controller = new TodoController(GetTestTodos());
        var result = controller.UpdateTodoItem(999, new TodoItem { Title = "Ghost" });

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void DeleteTodoItem_RemovesItem()
    {
        var todos = GetTestTodos();
        var controller = new TodoController(todos);
        var result = controller.DeleteTodoItem(1);

        Assert.IsType<NoContentResult>(result);
        Assert.Equal(2, todos.Count);
    }

    [Fact]
    public void DeleteTodoItem_ReturnsNotFound_WhenMissing()
    {
        var controller = new TodoController(GetTestTodos());
        var result = controller.DeleteTodoItem(999);

        Assert.IsType<NotFoundResult>(result);
    }
}