using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace TelegramBot_Scheduler
{
    public static class Answer
    {
        private static readonly Dictionary<string, Func<string[], string>> _commands = new() {
            ["commands"] = args => {
                if (args.Length != 1)
                    return "This command does not take arguments";

                return "'change' => change instance objective\n" +
                       "'list' => lists next x instances\n" +
                       "'redate' => redate an instance\n" +
                       "'save' => save something to remember later\n" +
                       "'todo' => shows the whole todo file";
            },
            ["redate"] = args => {
                if (args.Length != 3)
                    return "Follow command structure. Ex: redate 4 08:30";
                if (!int.TryParse(args[1], out int id))
                    return "Enter valid id";
                if (!TimeOnly.TryParse(args[2], out TimeOnly time))
                    return "Enter valid time";

                return Event.Redate(id, time);
            },
            ["list"] = args => {
                if (args.Length != 2)
                    return "Follow command structure. Ex: list 3";
                if (!int.TryParse(args[1], out int count))
                    return "Enter valid count";

                return Event.GetEvents(count);
            },
            ["change"] = args => {
                if (args.Length != 4)
                    return "Follow command structure. Ex: change 12 Reading Read new book";
                if (!int.TryParse(args[1], out _))
                    return "Enter valid id";
                if (args[2].Contains('|') || args[3].Contains('|'))
                    return "Enter valid arguments.";

                return Event.ChangeEvent(args);
            },
            ["save"] = args => {
                if (args.Length < 2)
                    return "Follow command structure. Ex: save Clean desk at 3 pm";

                StringBuilder sentence = new();
                for (int i = 1; i < args.Length; i++)
                {
                    sentence.Append(args[i]+" ");
                }
                
                return FileService.WriteToToDoFile(sentence.ToString());
            },
            ["todo"] = args =>
            {
                if (args.Length != 1)
                    return "This command does not take arguments";
                
                return FileService.ReadFromToDoFile();
            }
        };
        public static async Task OnMessage(Message message, UpdateType type) {
            // make sure only I, myself can use the bot
            if (message.Chat.Id != Info.CHAT_ID)
                return;
            
            if (string.IsNullOrEmpty(message.Text))
                return;
            
            string[] args = message.Text.Split(' ');
            
            if (!_commands.ContainsKey(args[0]))
                return;

            await Program.bot.SendMessage(Info.CHAT_ID, $"{_commands[args[0]](args)}");
        }
    }
}