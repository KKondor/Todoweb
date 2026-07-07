using static Todoweb.Model.API.Todo;

namespace Todoweb.Model.API
{
    public class TodoPatchDto
    {
        public string? Name { get; set; }
        public Priority? TodoPriority { get; set; } = Priority.Low_Priority;
        public string? Description { get; set; }
        public bool? IsComplete { get; set; } = false;
        public DateTime? DueDate { get; set; }
    }
}
