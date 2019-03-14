using System;

namespace SpyderWeb.Events.EventArguments
{
    public class MessageReceivedArgs : EventArgs
    {
        public string Message { get; set; }
        public object MessageContext { get; set; }

        public MessageReceivedArgs(string message, object messageContext)
        {
            Message = message;
            MessageContext = messageContext;
        }
    }
}