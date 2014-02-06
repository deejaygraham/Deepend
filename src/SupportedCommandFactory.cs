using System;
using System.Collections.Generic;
using System.Linq;

namespace Deepend
{
    public class SupportedCommandFactory
    {
        public ISupportedCommand Parse(IEnumerable<string> arguments)
        {
            var allCommands = new List<ISupportedCommand>() 
                    { 
                        new AnalyseFullAssembly(), 
                        new AnalyseNamespace(), 
                        new AnalyseType() 
                    };

            ISupportedCommand selectedCommand = null;
            
            try
            {
                if (arguments.Count() == 0)
                    throw new Exception();

                string typedCommand = arguments.First();

                foreach(var command in allCommands)
                {
                    if (String.Compare(typedCommand, command.Name, true) == 0)
                    {
                        selectedCommand = command;
                        break;
                    }
                }

                int skipCommands = 1;

                if (selectedCommand == null)
                {
                    selectedCommand = allCommands.First();
                    skipCommands = 0;
                }

                if (selectedCommand != null)
                    selectedCommand.Initialise(arguments.Skip(skipCommands));
            }
            catch(Exception ex)
            {
                selectedCommand = new InvalidArguments(
                    ex,
                    allCommands
                    );
            }

            return selectedCommand;
        }
    }
}
