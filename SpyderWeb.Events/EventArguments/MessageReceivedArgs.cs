using System;

namespace SpyderWeb.Events.EventArguments
{
    public class MessageReceivedArgs : EventArgs
    {
        public int Sender { get; set; }
        public string Prefix { get; set; }
        public string Message { get; set; }
        public object MessageContext { get; set; }

        public MessageReceivedArgs(int sender, string prefix, string message, object messageContext)
        {
            Sender = sender;
            Prefix = prefix;
            Message = message;
            MessageContext = messageContext;
        }
    }
}