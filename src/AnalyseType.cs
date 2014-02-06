using System.Collections.Generic;
using System.Text;

namespace Deepend
{
    public class AnalyseType : ISupportedCommand
    {
        public AnalyseType()
        {
        }

        private string TypeName { get; set; }

        public void Initialise(IEnumerable<string> arguments)
        {
            //this.TypeName = tn;
        }


        public void Execute()
        {
        }

        public string Name { get { return "type"; } }

        public string Syntax
        {
            get
            {
                StringBuilder builder = new StringBuilder();

                builder.AppendLine("<type> <assembly name>");

                return builder.ToString();
            }
        }
    }
}
