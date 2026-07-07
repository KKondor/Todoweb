namespace Todoweb
{
    public class Todo
    {
        public enum Priority
        {
            Low_Priority,
            Normal_Priority,
            Urgent_Priority
        }
        public int Id { get; set; }
        public string? Name { get; set; }
        public Priority TodoPriority { get; set; }
        public string? Description { get; set; }
        public bool IsComplete { get; set; }
        public DateTime CreateDate { get; }
        public DateTime DueDate { get; set; }

    }
}
