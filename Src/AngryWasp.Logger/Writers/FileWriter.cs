using System;
using System.IO;

namespace AngryWasp.Logger
{
    public class FileWriter : ILogWriter
    {
        StreamWriter output;

        public FileWriter(string logFilePath)
        {
            output = new StreamWriter(new FileStream(logFilePath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite));
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

        public void Flush()
        {
            output.Flush();
        }

        public void Close()
        {
            output.Close();
        }
    }
}
