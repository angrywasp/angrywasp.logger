using System;
using AngryWasp.Logger;
using System.IO;

namespace Logger.Sample
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Log log = Log.CreateInstance(true);

            //Can now access the log via the variable log
            //or the static property Log.Instance

            //Logger will close an application if you write a fatal error
            //CrashOnError will enable to same behaviour if an error is logged
            //This can be enabled/disabled at any time. Default value is false
            log.CrashOnError = false;

            //to write to Logger simply call Write
            log.Write("Hello world");

            //we can also supply a severity value
            log.Write(Log_Severity.Error, "This is an error: {0}", "Some random error message");

            //it is trivial to implement your own writer by implementing ILogWriter interface
            //the sample writer MemoryWriter is a variation of FileWriter to write to a memory stream rather than a file stream
            log.AddWriter("memory", new MemoryWriter(), false);

            log.Write("This log message will go to all three writers!");

            //writers can also be removed. Just pass the name of the writer to RemoveWriter
            log.RemoveWriter("memory");

            //call shutdown when closing your app to flush all the buffers
            log.Shutdown();

            Console.ReadKey();
        }
    }

    class MemoryWriter : ILogWriter
    {
        StreamWriter output;

        public MemoryWriter()
        {
            output = new StreamWriter(new MemoryStream());
        }

        public void Write(string value)
        {
            output.Write(value);
            output.Flush();
        }

        public void WriteLine(string value)
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
