using System.Collections.Generic;
using System.Linq;

namespace GameDebug
{
    public class DevConsole
    {
        // private readonly string _prefix;
        private readonly IEnumerable<IConsoleCommand> _commands;

        public DevConsole(IEnumerable<IConsoleCommand> commands)
        {
            // _prefix = prefix;
            _commands = commands;
        }

        public void ProcessCommand(string inputValue)
        {
            //if (!inputValue.StartsWith(_prefix))
            //   return;

            //inputValue = inputValue.Remove(0, _prefix.Length);

            string[] inputSplit = inputValue.Split(' ');

            string commandInput = inputSplit[0];
            string[] args = inputSplit.Skip(1).ToArray();

            ProcessCommand(commandInput, args);
        }

        public void ProcessCommand(string commandInput, string[] args)
        {
            foreach (var command in _commands)
            {
                if (!commandInput.Equals(command.CommandWord, System.StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                if (command.Process(args))
                {
                    return;
                }
                else
                {
                    DevConsoleBehaviour.Instance.Log("invalid arguments");
                    return;
                }

            }
            DevConsoleBehaviour.Instance.Log("invalid command");
        }
    }
}
