namespace TodoApp.Api.Models;

public class TodoItem
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsComplete { get; set; }
    public DateOnly? DueDate { get; set; }
    public int Priority { get; set; }
    public int? ParentId { get; set; }
}

