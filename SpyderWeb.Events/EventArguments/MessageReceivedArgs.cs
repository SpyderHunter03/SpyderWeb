using System;

namespace SpyderWeb.Events.EventArguments
{
    public class MessageReceivedArgs : EventArgs
    {
        public string Prefix { get; set; }
        public string Message { get; set; }
        public object MessageContext { get; set; }

        public MessageReceivedArgs(string prefix, string message, object messageContext)
        {
            Prefix = prefix;
            Message = message;
            MessageContext = messageContext;
        }
    }
}