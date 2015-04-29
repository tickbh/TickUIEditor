using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDEditor.Utils
{
    using EventList = List<EventHandler<object>>;
    class EventManager
    {
        static Dictionary<String, EventList> eventDict = new Dictionary<String, EventList>();

        public static void RegisterAudience(String raiser, EventHandler<object> eventHandler)
        {
            UnRegiseter(raiser, eventHandler.Target);
            EventList list;
            bool success = eventDict.TryGetValue(raiser, out list);
            if (!success)
            {
                list = new EventList();
                list.Add(eventHandler);
                eventDict.Add(raiser, list);
            }
            else
            {
                list.Add(eventHandler);
            }
        }

        public static void UnRegiseter(String raiser, object listen)
        {
            EventList list;
            bool success = eventDict.TryGetValue(raiser, out list);
            if (!success)
            {
                return;
            }
            foreach (EventHandler<object> args in list)
            {
                if (args.Target == listen)
                {
                    list.Remove(args);
                    break;
                }
            }

        }

        public static void RaiserEvent(String raiser, object obj, object arg)
        {
            EventList list;
            bool success = eventDict.TryGetValue(raiser, out list);
            if (!success)
            {
                return;
            }
            foreach (EventHandler<object> args in list)
            {
                args(obj, arg);
            }
        }
    }
}
