using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UdemyObservability.ConsoleApp
{
    internal class ServiceHelper
    {

        internal async Task Work1()
        {
            using var activity = ActivitySourceProvider.Source.StartActivity();
            var serviceOne = new ServiceOne();
            activity.SetTag("work 1 tag", "work 1 tag value");
            activity.AddEvent(new ActivityEvent("work 1 event"));
            Console.WriteLine($"google response length:{await serviceOne.MakeRequestToGoogle()}");
            Console.WriteLine("Work1 tamamlandı.");
        }
        internal async Task Work2()
        {

            using var activity = ActivitySourceProvider.SourceFile.StartActivity();
            activity.SetTag("work 2 tag", "work 2 tag value");
            activity.AddEvent(new ActivityEvent("work 2 event"));
        
        }

    }
}
