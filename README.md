# AngryWasp.Logger

Provides basic logging functionality.

## Building

Requires .NET Core

`dotnet restore && dotnet build -c Release`

## How-To

``` cs
//Create a log instance that writes to the console
Log.CreateInstance(true);
Log.Instance.Write(Log_Severity.Info, "Info message");
try {
    //do something stupid
} catch (Exception ex) {
    Log.Instance.WriteFatalException(ex, "Exception, stacktrace and this message will be written to the log and the program closed");
    //Can also use Log.Instance.WriteNonFatalException for the same functionality, but without quitting
}
```