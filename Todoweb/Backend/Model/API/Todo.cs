using System.ComponentModel.DataAnnotations;

namespace Todoweb.Backend.Model.API
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
        [MaxLength(100)]
        public string? Name { get; set; }
        [Range(0,2)]
        public Priority TodoPriority { get; set; } = Priority.Low_Priority;
        [MaxLength(1000)]
        public string? Description { get; set; }
        public bool IsComplete { get; set; } = false;
        public DateTime CreateDate { get; private set; } = DateTime.Now;
        public DateTime? DueDate { get; set; }
    }
}
