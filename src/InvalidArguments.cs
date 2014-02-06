using System;
using System.Collections.Generic;
using System.Text;

namespace Deepend
{
    public class InvalidArguments : ISupportedCommand
    {
        public InvalidArguments(Exception ex, IEnumerable<ISupportedCommand> cmds)
        {
            this.Ex = ex;
            this.Commands = new List<ISupportedCommand>(cmds);
        }
        
        private Exception Ex { get; set; }

        private List<ISupportedCommand> Commands { get; set; }

        public void Initialise(IEnumerable<string> ignore)
        {
        }

        public void Execute()
        {
            Console.WriteLine(this.Syntax);
        }

        public string Name
        {
            get { return "help"; }
        }

        public string Syntax
        {
            get
            {
                StringBuilder builder = new StringBuilder();

                if (this.Ex != null && !String.IsNullOrEmpty(this.Ex.Message))
                {
                    builder.AppendLine(this.Ex.Message);
                }

                builder.AppendLine();
                builder.AppendLine("Supported commands:");
                builder.AppendLine();

                foreach (var command in this.Commands)
                {
                    builder.AppendFormat("\t{0} {1}", command.Name, command.Syntax);
                    builder.AppendLine();
                }

                return builder.ToString();
            }
        }
    }
}
