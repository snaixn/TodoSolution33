using System.Text.Json;
using System.Text.Json.Serialization;

namespace Todo.Core
{
    public class TodoList
    {
        private readonly List<TodoItem> _items = new();

        public IReadOnlyList<TodoItem> Items => _items.AsReadOnly();
        public int Count => _items.Count;

        public TodoItem Add(string title)
        {
            var item = new TodoItem(title);
            _items.Add(item);
            return item;
        }

        public bool Remove(Guid id) => _items.RemoveAll(i => i.Id == id) > 0;

        public IEnumerable<TodoItem> Find(string substring) =>
            _items.Where(i => i.Title.Contains(substring ?? string.Empty, StringComparison.OrdinalIgnoreCase));

        //сохранение и загрузка
        public void Save(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException("Path is required", nameof(path));

            var options = new JsonSerializerOptions { WriteIndented = true };
            var json = JsonSerializer.Serialize(_items, options);
            File.WriteAllText(path, json);
        }

        public static TodoList Load(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException("Path is required", nameof(path));

            if (!File.Exists(path))
                throw new FileNotFoundException("File not found", path);

            var json = File.ReadAllText(path);
            var options = new JsonSerializerOptions();
            var items = JsonSerializer.Deserialize<List<TodoItem>>(json, options) ?? new List<TodoItem>();

            var list = new TodoList();
            list._items.AddRange(items);
            return list;
        }
    }
}