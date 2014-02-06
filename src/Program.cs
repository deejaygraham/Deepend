using System;

namespace Deepend
{
    class Program
    {
        static int Main(string[] args)
        {
            try
            {
                var factory = new SupportedCommandFactory();

                ISupportedCommand command = factory.Parse(args);

                command.Execute();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return -1;
            }

            return 0;
        }
    }
}
