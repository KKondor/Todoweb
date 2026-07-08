using System.ComponentModel.DataAnnotations;
using static Todoweb.Model.API.Todo;

namespace Todoweb.Model.API
{
    public class TodoPatchDto
    {
        [MaxLength(100)]
        public string? Name { get; set; }
        [Range(0,2)]
        public Priority? TodoPriority { get; set; } = Priority.Low_Priority;
        [MaxLength(1000)]
        public string? Description { get; set; }
        public bool? IsComplete { get; set; } = false;
        public DateTime? DueDate { get; set; }
    }
}
