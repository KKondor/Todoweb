using System.ComponentModel.DataAnnotations;

namespace Todoweb.Backend.Model.API
{
    public class TodoItemDto
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(100)]
        public string? Name { get; set; }
        [Range(0,2)]
        public Todo.Priority TodoPriority { get; set; } = Todo.Priority.Low_Priority;
        [MaxLength(1000)]
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
