using Telegram.Bot;

namespace TelegramBot_Scheduler
{
    internal class Program
    {
        public static TelegramBotClient bot = new(Info.API_KEY);
        public static HashSet<int> _knownEvents = new();

        static async Task Main(string[] args)
        {
            bot.OnMessage += Answer.OnMessage;
            Console.CancelKeyPress += (_, _) =>
            {
                Environment.Exit(0);
            };

            Console.WriteLine($"Bot is running, ctrl+c to cancel...");

            DateTime start = DateTime.Now;

            while (true) {
                if (DateTime.Now - start > TimeSpan.FromMinutes(1)) {
                    start = DateTime.Now;

                    string eventResult = Event.GetEvents(1);
                    if (string.IsNullOrEmpty(eventResult)) {
                        Console.WriteLine("No events for today anymore");
                        break;
                    }

                    Event nextEvent = Event.FormattedStringToEvent(eventResult);

                    if (nextEvent.Time - TimeOnly.FromDateTime(start) < TimeSpan.FromMinutes(15)
                        && !_knownEvents.Contains(nextEvent.ID)) {
                        await bot.SendMessage(Info.CHAT_ID, $"Next event: {nextEvent.ID}|{nextEvent.Name}\n" +
                        $"{nextEvent.Description}\n" +
                        $"Only {(nextEvent.Time - TimeOnly.FromDateTime(start)).Minutes}min left.\n" +
                        $"Get ready!");

                        _knownEvents.Add(nextEvent.ID);
                    }
                }
            }
        }
    }
}
