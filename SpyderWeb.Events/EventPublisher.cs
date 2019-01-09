using SpyderWeb.Events.EventArguments;
using System;

namespace SpyderWeb.Events
{
    public static class EventPublisher
    {
        public static event EventHandler<CreateTagArgs> CreateTagEvent;
        public static event EventHandler<UpdateTagArgs> UpdateTagEvent;
        public static event EventHandler<DeleteTagArgs> DeleteTagEvent;

        public static void OnCreateTagEvent(object sender, CreateTagArgs args)
        {
            var handler = CreateTagEvent;
            handler?.Invoke(sender, args);
        }

        public static void OnUpdateTagEvent(object sender, UpdateTagArgs args)
        {
            var handler = UpdateTagEvent;
            handler?.Invoke(sender, args);
        }

        public static void OnDeleteTagEvent(object sender, DeleteTagArgs args)
        {
            var handler = DeleteTagEvent;
            handler?.Invoke(sender, args);
        }
    }
}
