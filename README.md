# EventFlow with EventSource
A basic example of using [Microsoft.Diagnostics.EventFlow](https://github.com/Azure/diagnostics-eventflow ) with [EventSource](https://docs.microsoft.com/en-us/dotnet/api/system.diagnostics.tracing.eventsource ) for logging purposes.



### Event Source:

```c#
// this file defices the possible logging events that can be used from other parts of the code

[EventSource(Name = "MyEventSource")]
sealed class MyCompanyEventSource : EventSource
{
    public static MyCompanyEventSource Log = new MyCompanyEventSource();

    public static class Keywords
    {
        public const EventKeywords Page = (EventKeywords)1;
        public const EventKeywords DataBase = (EventKeywords)2;
        public const EventKeywords Diagnostic = (EventKeywords)4;
        public const EventKeywords Perf = (EventKeywords)8;
    }

    public static class Tasks
    {
        public const EventTask Page = (EventTask)1;
        public const EventTask DBQuery = (EventTask)2;
    }

    [Event(1002, Message = "Starting up.", Keywords = Keywords.Perf, Level = EventLevel.Informational)]
    public void Startup() { WriteEvent(1002); }
	...
    [Event(1006, Opcode = EventOpcode.Stop, Task = Tasks.DBQuery, Keywords = Keywords.DataBase, Level = EventLevel.Informational)]
    public void DBQueryStop() { WriteEvent(1006); }
    ...
    [Event(1008, Message = "Closing application.", Keywords = Keywords.Perf, Level = EventLevel.Informational)]
    public void Closing() { WriteEvent(1008); }
    
}

```



### config.json file:

```json
// inputs and outputs for the logging. there might be multiple ones if needed. for example one might log information, another errors.

{
    "inputs": [
        {
            "type": "EventSource",
            "sources": [
                {
                    "providerName": "MyEventSource",
                    "level": "Informational"//,
                }
            ]
        }
    ],
    "outputs": [
        { "type": "StdOutput" }
    ],
    "schemaVersion": "2016-08-11"
}
```



### using from Program:

```c#
// a basic example of using Log.Startup() and Log.Closing()
// the initialization of the logging happens through calling CreatePipeline() method

internal class Program
{
    static void Main()
    {
        using (var pipeline = DiagnosticPipelineFactory.CreatePipeline("eventFlowConfig.json"))
        {
            MyCompanyEventSource.Log.Startup(); // defined in EventSource file

            PerformSomething();

            MyCompanyEventSource.Log.Closing(); // defined in EventSource file
        }

        Console.WriteLine("Any key to exit");
        Console.ReadKey();
    }

    private static void PerformSomething()
    {
        Console.WriteLine("Some action here.");
    }
}
```

