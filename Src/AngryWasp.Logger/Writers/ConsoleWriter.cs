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
			output.WriteLine(value);
			output.Flush();
        }
    }
}
