using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Order.API.OrderServices;

namespace Order.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {

        private readonly OrderService _orderService;
        public OrderController(OrderService orderService)
        {
            _orderService = orderService;
        }
        [HttpPost]
        public async Task<IActionResult> Create(OrderCreateRequestDto request)
        {

         
            return Ok(await _orderService.CreateAsync(request));
            #region Exception örneği için hazırlandı.

            //var a = 10;
            //var b = 0;
            //var c = a / b; 
            #endregion



        }
    }
}
