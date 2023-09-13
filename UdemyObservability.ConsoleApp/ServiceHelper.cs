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

            throw new Exception("work 1 hata meydan geldi");

            Console.WriteLine($"google response length:{await serviceOne.MakeRequestToGoogle()}");
            Console.WriteLine("Work1 tamamlandı.");


            var serviceTwo = new ServiceTwo();

            var fileLength = serviceTwo.WriteToFile("Merhaba Dünya");


        }
    }
}
