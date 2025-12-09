using Xunit;
using Todo.Core;
using System.IO;

namespace Todo.Core.Tests
{
    public class TodoListPersistenceTests : IDisposable
    {
        private readonly string _tempFile;

        public TodoListPersistenceTests()
        {
            _tempFile = Path.GetTempFileName();
        }

        public void Dispose()
        {
            if (File.Exists(_tempFile))
                File.Delete(_tempFile);
        }

        [Fact]
        public void SaveAndLoad_ShouldPreserveItems()
        {
            // Arrange
            var list = new TodoList();
            list.Add("Buy milk");
            list.Add("Read book");
            var item = list.Add("Write code");
            item.MarkDone();

            // Act
            list.Save(_tempFile);
            var loaded = TodoList.Load(_tempFile);

            // Assert
            Assert.Equal(3, loaded.Count);
            var items = loaded.Items;
            Assert.Contains(items, i => i.Title == "Buy milk" && !i.IsDone);
            Assert.Contains(items, i => i.Title == "Write code" && i.IsDone);
        }

        [Fact]
        public void Load_NonExistentFile_ThrowsFileNotFoundException()
        {
            // Act & Assert
            Assert.Throws<FileNotFoundException>(() => TodoList.Load("nonexistent.json"));
        }

        [Fact]
        public void Save_NullPath_ThrowsArgumentException()
        {
            // Arrange
            var list = new TodoList();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => list.Save(null!));
        }
    }
}