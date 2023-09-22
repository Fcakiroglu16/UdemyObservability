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

        private readonly ILogger<PaymentProcessController> _logger;

        public PaymentProcessController(ILogger<PaymentProcessController> logger)
        {
            _logger = logger;
        }

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
                _logger.LogWarning("yetersiz bakiye.orderCode={@orderCode}", request.OrderCode);
                return BadRequest(ResponseDto<PaymentCreateResponseDto>.Fail(400, "yetersiz bakiye"));

            }

            _logger.LogInformation("kart işlemi başarıyla gerçekleşmiştir. orderCode={@orderCode}", request.OrderCode);
            return Ok(ResponseDto<PaymentCreateResponseDto>.Success(200,new PaymentCreateResponseDto()
            {
                Description= "kart işlemi başarıyla gerçekleşmiştir"
            }));


        }

    }
}
