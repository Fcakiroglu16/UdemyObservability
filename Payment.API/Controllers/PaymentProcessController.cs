using Common.Shared.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace Payment.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PaymentProcessController : ControllerBase
    {


        [HttpPost]
        public IActionResult Create(PaymentCreateRequestDto request)
        {
            
            if (HttpContext.Request.Headers.TryGetValue("traceparent", out StringValues values))
            {
                Console.WriteLine($"traceParent:{values.First()}");
            };


            const decimal balance = 1000;

            if(request.TotalPrice>balance)
            {

                return BadRequest(ResponseDto<PaymentCreateResponseDto>.Fail(400, "yetersiz bakiye"));

            }
            return Ok(ResponseDto<PaymentCreateResponseDto>.Success(200,new PaymentCreateResponseDto()
            {
                Description= "kart işlemi başarıyla gerçekleşmiştir"
            }));


        }

    }
}
