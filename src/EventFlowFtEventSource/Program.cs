using System;
using Microsoft.Diagnostics.EventFlow;

namespace EventFlowFtEventSource
{
    internal class Program
    {
        static void Main()
        {
            using (var pipeline = DiagnosticPipelineFactory.CreatePipeline("eventFlowConfig.json"))
            {
                MyCompanyEventSource.Log.Startup();

                PerformSomething();

                MyCompanyEventSource.Log.Closing();
            }

            Console.WriteLine("Any key to exit");
            Console.ReadKey();
        }

        private static void PerformSomething()
        {
            Console.WriteLine("Some action here.");
        }
    }
}
