namespace TelegramBot_Scheduler
{
    public static class FileService
    {
        private static readonly object _fileLock = new();

        public static string WriteToToDoFile(string content) {
            lock (_fileLock) {
                using FileStream fileStream = new($"Files/todo.txt", FileMode.Append, FileAccess.Write);
                using StreamWriter streamWriter = new(fileStream);
                
                streamWriter.Write("\n"+content);
            }
            return "Saved to file successfully!";
        }
        
        public static void WriteToDayEventFile<T>(T content) {
            lock (_fileLock) {
                using FileStream fileStream = new($"Files/{GetFileName()}.txt", FileMode.Truncate, FileAccess.Write);
                using StreamWriter streamWriter = new(fileStream);

                if (content is IEnumerable<string> list) {
                    foreach (var item in list)
                    {
                        streamWriter.Write(item + "\n");
                    }
                }
                else {
                    streamWriter.Write(content);
                }
            }
        }
        private static string GetFileName() => DateTime.Now.DayOfWeek.ToString().ToLower();

        public static List<string> Read() {
            lock (_fileLock) {
                using FileStream fileStream = new($"Files/{GetFileName()}.txt", FileMode.Open, FileAccess.Read);
                using StreamReader streamReader = new(fileStream);

                List<string> entries = new();
                string? line = string.Empty;

                while ((line = streamReader.ReadLine()) != null)
                {
                    entries.Add(line);
                }
                
                return entries;
            }
        }
    }
}
