using System;
using System.IO;

namespace AngryWasp.Logger
{
    public class ConsoleWriter : ILogWriter
    {
        private TextWriter output;

        public ConsoleWriter()
        {
            output = Console.Out;
        }

        public void Flush()
        {
            output.Flush();
        }

        public void Close()
        {
            output.Close();
        }

		public void WriteText(string value)
		{
			output.WriteLine(value);
			output.Flush();
		}

        public void Write(Log_Severity severity, string value)
        {
            switch (severity)
            {
                case Log_Severity.Fatal:
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    break;
                case Log_Severity.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case Log_Severity.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case Log_Severity.Info:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case Log_Severity.None:
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    break;
            }

			output.WriteLine(value);
			output.Flush();

            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
