using System;
using System.IO;

namespace AngryWasp.Logger
{
    public class ConsoleWriter : ILogWriter
    {
        private TextWriter output;
        private ConsoleColor color = ConsoleColor.White;

        public void SetColor(ConsoleColor color)
        {
            this.color = color;
            Console.ForegroundColor = color;
        }

        public ConsoleWriter()
        {
            output = Console.Out;
        }

        public void Flush() => output.Flush();

        public void Close() => output.Close();

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
                default: //Info and None
                    Console.ForegroundColor = color;
                    break;
            }

			output.WriteLine(value);
			output.Flush();

            Console.ForegroundColor = color;
        }
    }
}
