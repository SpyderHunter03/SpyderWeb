using System;

namespace SpyderWeb.Events.EventArguments
{
    public class SendMessageArgs : EventArgs
    {
        public string Message { get; set; }
        public object MessageContext { get; set; }

        public SendMessageArgs(string message, object messageContext)
        {
            Message = message;
            MessageContext = messageContext;
        }
    }
}