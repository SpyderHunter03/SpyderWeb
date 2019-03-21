using System;
using System.Text;
using SpyderWeb.Events;
using SpyderWeb.Events.EventArguments;

namespace SpyderWeb.CoreModules
{
    public static class HelpModule
    {
        public static void MessageReceived(object sender, MessageReceivedArgs args)
        {
            if (args.Message.IndexOf($"{args.Prefix}help", StringComparison.InvariantCultureIgnoreCase) == 0)
                HelpMethod(sender, args);
        }

        private static void HelpMethod(object sender, MessageReceivedArgs args)
        {
            var content = new StringBuilder();
            content.AppendLine("**SpyderWeb**");
            content.AppendLine("A utility bot for Discord.Net\n");
            content.AppendLine("```");

            // foreach (var module in _commandService.Modules)
            // {
            //     if (_tagService.Module != null && module == _tagService.Module) continue;
            //     if (module.Commands.Count == 0) continue;

            //     content.AppendLine(module.Name);

            //     foreach (var command in module.Commands)
            //         content.AppendLine(command.Name.PadRight(27) + command.Summary);

            //     content.AppendLine();
            // }

            content.AppendLine("```");
            content.AppendLine("Commands suffixed with a '*' are restricted to elevated actors");

            SendMessage(sender, content.ToString());
        }

        private static void SendMessage(object sender, string message)
        {
            EventPublisher.OnSendMessageEvent(sender, new SendMessageArgs(message, sender));
        }
    }
}