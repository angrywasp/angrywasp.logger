using System.Collections.Generic;
using System;

namespace AngryWasp.Logger
{
    public enum Log_Severity
    {
        None,
        Info,
        Warning,
        Error,
        Fatal
    }

    public interface ILogWriter
    {
        void Write(Log_Severity severity, string value);

        void Flush();

        void Close();

		void SetColor(ConsoleColor color);
    }

    public class Log
    {
        public const string LineBreakDouble = "=========================================================================";
        public const string LineBreakSingle = "-------------------------------------------------------------------------";

        private Dictionary<string, ILogWriter> writers = new Dictionary<string, ILogWriter>();

        public bool CrashOnError { get; set; } = false;

        private static Log instance = null;

        public static Log Instance => instance;

        public static Log CreateInstance(bool consoleOut = false, string outputFile = null)
        {
            if (instance != null)
                return instance;
                
            instance = new Log(consoleOut, outputFile);
            return instance;
        } 

        private Log(bool consoleOut = false, string outputFile = null)
        {
            if (instance != null)
                throw new Exception("Log is already initialized");

            if(consoleOut)
                AddWriter("console", new ConsoleWriter(), false);

            if(!String.IsNullOrEmpty(outputFile))
                AddWriter("file", new FileWriter(outputFile), false);
        }

        //todo: this will reset the console color. 
        //need to fix that
        public bool SupressConsoleOutput
        {
            set
            {
                if (value)
                    RemoveWriter("console");
                else
                    AddWriter("console", new ConsoleWriter(), false);
            }
        }

        public void AddWriter(string name, ILogWriter writer, bool writeLogHeader)
        {
            if (writers.ContainsKey(name))
            {
                writers[name] = writer;
                return;
            }

            if(writeLogHeader)
                WriteLogHeader(writer);

            writers.Add(name, writer);
        }

        public bool RemoveWriter(string name)
        {
            if(!writers.ContainsKey(name))
                return false;

            writers.Remove(name);

            return true;
        }

        public void SetColor(ConsoleColor color)
        {
            foreach (var w in writers.Values)
                w.SetColor(color);
        }

        private void WriteLogHeader(ILogWriter writer)
        {
            Write(LineBreakDouble, Log_Severity.None);
            Write("Logger Started at: " + DateTime.Now.ToString(), Log_Severity.None);
            Write(LineBreakDouble, Log_Severity.None);
        }

        private string Write(string message, Log_Severity severity = Log_Severity.Info)
        {
            if (severity != Log_Severity.None)
                message = $"{severity.ToString()}: {message}";

            foreach(ILogWriter writer in writers.Values)
                writer.Write(severity, message);

            switch(severity)
            {
                case Log_Severity.Error:
                    {
                        if(CrashOnError)
                        {
                            Shutdown();
                            Environment.Exit(0);
                        }
                    }
                    break;

                case Log_Severity.Fatal:
                    {
                        Shutdown();
                        Environment.Exit(0);
                    }
                    break;
            }
                    
            return message;
        }

        /// <summary>
        /// Write an info message to the log
        /// </summary>
        /// <param name="message">Message to write to the log</param>
        public string Write(string message) =>
            Write(message, Log_Severity.Info);

        /// <summary>
        /// Write a message to the log. Requires user to state message severity. For general info message Write(string, params object[]) can be used
        /// </summary>
        /// <param name="severity">The severity of the error message</param>
        /// <param name="message">Message to write to the log</param>
        public string Write(Log_Severity severity, string message) =>
            Write(message, severity);

        /// <summary>
        /// Writes an exception to the log and marks it as an error.
        /// This allows the Application to keep running on an exception (subject to CrashOnError)
        /// </summary>
        /// <returns>The non fatal exception.</returns>
        /// <param name="ex">The exception that will be written to the log</param>
        /// <param name="additionalMessage">Additional message. Use for a description of the exception</param>
        public string WriteNonFatalException(Exception ex, string additionalMessage = null) =>
            WriteException(Log_Severity.Warning, ex, additionalMessage);

        /// <summary>
        /// Writes an exception to the log and closes the application
        /// </summary>
        /// <returns>The fatal exception.</returns>
        /// <param name="ex">The exception that will be written to the log</param>
        /// <param name="additionalMessage">Additional message. Use for a description of the exception</param>
        public Exception WriteFatalException(Exception ex, string additionalMessage = null) =>
            new Exception(WriteException(Log_Severity.Fatal, ex, additionalMessage));

        /// <summary>
        /// Flushes all ILogWriter buffers and closes all writers
        /// Do not attempt to write to the log after calling Shutdown
        /// </summary>
        public void Shutdown()
        {
            foreach(ILogWriter writer in writers.Values)
            {
                writer.Flush();
                writer.Close();
            }
        }

        private string WriteException(Log_Severity severity, Exception ex, string additionalMessage = null)
        {
			if (additionalMessage == null)
			{
				string s = Write(severity, $"{ex.Message}\r\nStack:\r\n{ex.StackTrace}");
				if (ex.InnerException != null)
					s += WriteException(severity, ex.InnerException);

				return s;
			}
			else
			{
				string s = Write(severity, $"{additionalMessage}\r\nException:\r\n{ex.Message}\r\nStack:\r\n{ex.StackTrace}");
				if (ex.InnerException != null)
					s += WriteException(severity, ex.InnerException, additionalMessage);

				return s;
			}
        }
    }
}
