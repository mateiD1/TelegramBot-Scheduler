using System.Text;

namespace TelegramBot_Scheduler
{
    public struct Event(int id, string name, string description, TimeOnly time)
    {
        public int ID { get; } = id;
        public string Name { get; private set; } = name;
        public string Description { get; private set; } = description;
        public TimeOnly Time { get; private set; } = time;

        public override string ToString() {
            return $"{ID}|{Name}|{Description}|{Time}";
        }

        public static Event FormattedStringToEvent(string eventInfo) {
            string[] info = eventInfo.Split('|');

            return new Event(Convert.ToInt32(info[0]),
                info[1],
                info[2],
                TimeOnly.Parse(info[3]));
        }

        public static string GetEvents(int count) {
            TimeOnly currTime = TimeOnly.FromDateTime(DateTime.Now);
            List<string> unparsedEvents = FileService.Read();

            Event[] events = new Event[count];
            int lastIndex = 0;

            for (int i = 0; i < unparsedEvents.Count; i++) {
                Event e = Event.FormattedStringToEvent(unparsedEvents[i]);

                if (e.Time > currTime) {
                    events[lastIndex] = e;
                    lastIndex++;
                }

                if (lastIndex == count)
                    break;
            }

            // return all events as one string
            StringBuilder eventsAsText = new();
            foreach (var item in events) {
                string eventAsString = item.ToString();
                
                // meaning we reached the point of 'nulls'
                if (eventAsString[0..eventAsString.IndexOf('|')] == "0")
                    break;

                eventsAsText.Append(item.ToString() + "\n");
            }

            return eventsAsText.ToString();
        }

        public static string ChangeEvent(string[] args) {
            string eventID = args[1];

            List<string> events = FileService.Read();

            for (int i = 0; i < events.Count; i++) {
                if (events[i][0..events[i].IndexOf('|')] == eventID) {
                    Event newEvent = Event.FormattedStringToEvent(events[i]);
                    newEvent.Name = args[2];
                    
                    StringBuilder newEventDescription = new();
                    for (int j = 3; j < args.Length; j++) {
                        newEventDescription.Append(args[j]+" ");
                    }
                    newEvent.Description = newEventDescription.ToString();

                    events[i] = newEvent.ToString();
                    break;
                }
            }

            FileService.Write(events);

            return "Updated Event successfully!";
        }

        public static string Redate(int id, TimeOnly time) {
            List<string> events = FileService.Read();
            bool found = false;

            for (int i=0; i<events.Count; i++) {
                if (Convert.ToInt32(events[i][0..events[i].IndexOf('|')]) == id) {
                    found = true;

                    Event currEvent = Event.FormattedStringToEvent(events[i]);
                    currEvent.Time = time;
                    events[i] = currEvent.ToString();
                    
                    if (currEvent.Time < TimeOnly.FromDateTime(DateTime.Now)) {
                        Program._knownEvents.Remove(currEvent.ID);
                    }
                    
                    // check if we have to redate coming events so they do not overlap
                    if (i < events.Count-1) {
                        Event nextEvent = Event.FormattedStringToEvent(events[i+1]);

                        if (nextEvent.Time < currEvent.Time)
                            updateComingEvents(i, currEvent.Time-nextEvent.Time);
                    }
                    break;
                }
            }

            if (!found)
                return "ID not found";

            FileService.Write(events);
            return "Redated succesfully";
        }

        private static void updateComingEvents(int i, TimeSpan difference) {
            List<string> events = FileService.Read();

            for (int j=i; j<events.Count; j++) {
                Event currEvent = Event.FormattedStringToEvent(events[j]);
                currEvent.Time.Add(difference);

                if (currEvent.Time < TimeOnly.FromDateTime(DateTime.Now)) {
                    Program._knownEvents.Remove(currEvent.ID);
                }
            }
        }
    }
}