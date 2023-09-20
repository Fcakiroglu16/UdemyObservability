using Common.Shared.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenTelemetry.Shared;
using Stock.API.Services;
using System.Diagnostics;

namespace Stock.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class StockController : ControllerBase
    {

        private readonly StockService _stockService;

        public StockController(StockService stockService)
        {
            _stockService = stockService;
        }

        [HttpPost]
        public async Task<IActionResult> CheckAndPaymentStart(StockCheckAndPaymentProcessRequestDto request)
        {
            //örnek için header datası alındı ve debug modda incelendi.
            var header = HttpContext.Request.Headers;



            var result = await  _stockService.CheckAndPaymentProcess(request);
        
            return new ObjectResult(result) {  StatusCode=result.StatusCode };





        }
    }
}
