using System.Text.Json.Serialization;

namespace Todo.Core
{
    public class TodoItem
    {
        [JsonInclude]
        public Guid Id { get; private set; }

        [JsonInclude]
        public string Title { get; private set; }

        [JsonInclude]
        public bool IsDone { get; private set; }

        // System.Text.Json
        public TodoItem()
        {
            Id = Guid.NewGuid(); 
            Title = string.Empty;
        }

        public TodoItem(string title)
        {
            Title = title?.Trim() ?? throw new ArgumentNullException(nameof(title));
            Id = Guid.NewGuid();
        }

        public void MarkDone() => IsDone = true;
        public void MarkUndone() => IsDone = false;

        public void Rename(string newTitle)
        {
            if (string.IsNullOrWhiteSpace(newTitle))
                throw new ArgumentException("Title required", nameof(newTitle));
            Title = newTitle.Trim();
        }
    }
}