namespace SpyderWeb.CoreModules
{
    [System.AttributeUsage(System.AttributeTargets.Class |
                           System.AttributeTargets.Struct)]
    public class CommandAttribute : System.Attribute
    {
        private string _command;

        public CommandAttribute(string command)
        {
            _command = command;
        }
    }
}