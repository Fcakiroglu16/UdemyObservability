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
            var result = await _orderService.CreateAsync(request);

            #region Third-party api istek örneği

            //var httpClient = new HttpClient();

            //var response = await httpClient.GetAsync("https://jsonplaceholder.typicode.com/todos/1");

            //var content = await response.Content.ReadAsStringAsync(); 
            #endregion


            return new ObjectResult(result) { StatusCode = result.StatusCode };
            #region Exception örneği için hazırlandı.

            //var a = 10;
            //var b = 0;
            //var c = a / b; 
            #endregion



        }
    }
}
