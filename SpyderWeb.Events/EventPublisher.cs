using SpyderWeb.Events.EventArguments;
using System;

namespace SpyderWeb.Events
{
    public static class EventPublisher
    {
        // public static event EventHandler<CreateTagArgs> CreateTagEvent;
        // public static void OnCreateTagEvent(object sender, CreateTagArgs args)
        // {
        //     var handler = CreateTagEvent;
        //     handler?.Invoke(sender, args);
        // }

        // public static event EventHandler<UpdateTagArgs> UpdateTagEvent;
        // public static void OnUpdateTagEvent(object sender, UpdateTagArgs args)
        // {
        //     var handler = UpdateTagEvent;
        //     handler?.Invoke(sender, args);
        // }

        // public static event EventHandler<DeleteTagArgs> DeleteTagEvent;
        // public static void OnDeleteTagEvent(object sender, DeleteTagArgs args)
        // {
        //     var handler = DeleteTagEvent;
        //     handler?.Invoke(sender, args);
        // }

        public static event EventHandler<MessageReceivedArgs> MessageReceivedEvent;
        public static void OnMessageReceivedEvent(object sender, MessageReceivedArgs args)
        {
            var handler = MessageReceivedEvent;
            handler?.Invoke(sender, args);
        }

        public static event EventHandler<SendMessageArgs> SendMessageEvent;
        public static void OnSendMessageEvent(object sender, SendMessageArgs args)
        {
            var handler = SendMessageEvent;
            handler?.Invoke(sender, args);
        }
    }
}
