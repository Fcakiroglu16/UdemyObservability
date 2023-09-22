using Common.Shared.DTOs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Shared
{
    public static class ExceptionMiddleware
    {

        public static void UseExceptionMiddleware(this WebApplication app)
        {

            app.UseExceptionHandler(config =>
            {



                config.Run(async context =>
                {

                    var exceptionFeature = context.Features.Get<IExceptionHandlerFeature>();

                    context.Response.StatusCode = 500;

                    var response = ResponseDto<string>.Fail(500, exceptionFeature!.Error.Message);

                    await context.Response.WriteAsJsonAsync(response);


                });







            });







        }
    }
}
