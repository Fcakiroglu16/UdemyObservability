using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UdemyObservability.ConsoleApp
{
    internal class ServiceOne
    {
        static HttpClient httpClient = new HttpClient();
       internal async Task<int> MakeRequestToGoogle()
        {



            using var activity = ActivitySourceProvider.Source.StartActivity(kind:System.Diagnostics.ActivityKind.Producer,name: "CustomMakeRequestToGoogle");

            try
            {
                var eventTags = new ActivityTagsCollection();


                activity?.AddEvent(new("gooogle'a istek başladı", tags: eventTags));
                activity?.AddTag("request.schema", "https");
                activity?.AddTag("request.method", "get");
                var result = await httpClient.GetAsync("https://www.google.com");


                var responseContent = await result.Content.ReadAsStringAsync();

                activity?.AddTag("response.length", responseContent.Length);


                eventTags.Add("google body length", responseContent.Length);
                activity?.AddEvent(new("gooogle'a istek tamamlandı", tags: eventTags));


                var serviceTwo = new ServiceTwo();

                var fileLength = await serviceTwo.WriteToFile("Merhaba Dünya");



                return responseContent.Length;
            }
            catch (Exception ex)
            {

                activity?.SetStatus(ActivityStatusCode.Error,ex.Message);
                return -1;
            }

        

            
        }



    }
}
