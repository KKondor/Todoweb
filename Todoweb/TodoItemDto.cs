namespace Todoweb
{
    public class TodoItemDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public Todo.Priority TodoPriority { get; set; } = Todo.Priority.Low_Priority;
        public string? Description { get; set; }
        public bool IsComplete { get; set; } = false;
        public DateTime CreateDate { get; } = DateTime.Now;
        public DateTime? DueDate { get; set; }

        public TodoItemDto() { }
        public TodoItemDto(Todo todoItem) =>
        (Id, Name, TodoPriority, Description, IsComplete, CreateDate, DueDate) = 
            (todoItem.Id, todoItem.Name, todoItem.TodoPriority, todoItem.Description, todoItem.IsComplete, todoItem.CreateDate, todoItem.DueDate);

    }
}
