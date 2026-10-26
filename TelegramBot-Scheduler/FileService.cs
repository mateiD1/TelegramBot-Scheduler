namespace TelegramBot_Scheduler
{
    public static class FileService
    {
        public static string WriteToToDoFile(string content) 
        {
            File.AppendAllText("Files/todo.txt", ("\n"+content));
            return "Saved to file successfully!";
        }

        public static string ReadFromToDoFile()
        {
            return File.ReadAllText("Files/todo.txt");
        }
        
        public static void Write(List<string> list) {
            using FileStream fileStream = new($"Files/{GetFileName()}.txt", FileMode.Truncate, FileAccess.Write);
            using StreamWriter streamWriter = new(fileStream);
            
            // when reading all events and changing the list
            foreach (var item in list)
            {
                streamWriter.Write(item + "\n");
            }
        }
        private static string GetFileName() => DateTime.Now.DayOfWeek.ToString().ToLower();

        public static List<string> Read() 
        {
            return File.ReadAllLines($"Files/{GetFileName()}.txt").ToList();
        }
    }
}
