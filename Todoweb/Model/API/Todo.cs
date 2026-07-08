using System.ComponentModel.DataAnnotations;

namespace Todoweb.Model.API
{
    public class Todo
    {
        public enum Priority
        {
            Low_Priority,
            Normal_Priority,
            Urgent_Priority
        }
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public Priority TodoPriority { get; set; } = Priority.Low_Priority;
        public string? Description { get; set; }
        public bool IsComplete { get; set; } = false;
        public DateTime CreateDate { get;} = DateTime.Now;
        public DateTime? DueDate { get; set; }
    }
}
