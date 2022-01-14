// https://docs.microsoft.com/en-us/dotnet/api/system.diagnostics.tracing.eventsource

using System.Diagnostics.Tracing;

namespace EventFlowFtEventSource
{
    [EventSource(Name = "MyEventSource")]
    sealed class MyCompanyEventSource : EventSource
    {
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

        [Event(1001, Message = "Application Failure: {0}", Level = EventLevel.Error, Keywords = Keywords.Diagnostic)]
        public void Failure(string message) { WriteEvent(1001, message); }

        [Event(1002, Message = "Starting up.", Keywords = Keywords.Perf, Level = EventLevel.Informational)]
        public void Startup() { WriteEvent(1002); }


        [Event(1003, Message = "loading page {1} activityID={0}", Opcode = EventOpcode.Start,
            Task = Tasks.Page, Keywords = Keywords.Page, Level = EventLevel.Informational)]
        public void PageStart(int ID, string url) { if (IsEnabled()) WriteEvent(1003, ID, url); }

        [Event(1004, Opcode = EventOpcode.Stop, Task = Tasks.Page, Keywords = Keywords.Page, Level = EventLevel.Informational)]
        public void PageStop(int ID) { if (IsEnabled()) WriteEvent(1004, ID); }

        [Event(1005, Opcode = EventOpcode.Start, Task = Tasks.DBQuery, Keywords = Keywords.DataBase, Level = EventLevel.Informational)]
        public void DBQueryStart(string sqlQuery) { WriteEvent(1005, sqlQuery); }

        [Event(1006, Opcode = EventOpcode.Stop, Task = Tasks.DBQuery, Keywords = Keywords.DataBase, Level = EventLevel.Informational)]
        public void DBQueryStop() { WriteEvent(1006); }

        [Event(1007, Level = EventLevel.Verbose, Keywords = Keywords.DataBase)]
        public void Mark(int ID) { if (IsEnabled()) WriteEvent(1007, ID); }

        [Event(1008, Message = "Closing application.", Keywords = Keywords.Perf, Level = EventLevel.Informational)]
        public void Closing() { WriteEvent(1008); }

        public static MyCompanyEventSource Log = new MyCompanyEventSource();
    }
}
