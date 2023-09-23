using Common.Shared.DTOs;
using System.Diagnostics;
using System.Net;

namespace Stock.API.Services
{
    public class StockService
    {
        private readonly PaymentService _paymentService;
        private readonly ILogger<StockService> _logger;
        public StockService(PaymentService paymentService, ILogger<StockService> logger)
        {
            _paymentService = paymentService;
            _logger = logger;
        }

        private Dictionary<int, int> GetProductStockList()
        {

            Dictionary<int, int> productStockList = new();
            productStockList.Add(1, 10);
            productStockList.Add(2, 20);
            productStockList.Add(3, 30);

            return productStockList;

        }


        public async Task<ResponseDto<StockCheckAndPaymentProcessResponseDto>> CheckAndPaymentProcess(StockCheckAndPaymentProcessRequestDto request)
        {




            var userId = Activity.Current?.GetBaggageItem("userId");

           

            var productStockList = GetProductStockList();

            var stockStatus = new List<(int productId, bool hasStockExist)>();

            foreach (var orderItem in request.OrderItems)
            {
                var hasExistStock = productStockList.Any(x => x.Key == orderItem.ProductId && x.Value >= orderItem.Count);

                stockStatus.Add((orderItem.ProductId, hasExistStock));

            }

            if (stockStatus.Any(x => x.hasStockExist == false))
            {

                return ResponseDto<StockCheckAndPaymentProcessResponseDto>.Fail(HttpStatusCode.BadRequest.GetHashCode(), "stock yetersiz");

            }

          
            _logger.LogInformation("Stock ayrıldı.orderCode:{@orderCode}", request.OrderCode);
            var (isSuccess, failMessage) = await _paymentService.CreatePaymentProcess(new PaymentCreateRequestDto()
            {
                OrderCode = request.OrderCode,
                TotalPrice = request.OrderItems.Sum(x => x.UnitPrice)
            });

            if (isSuccess)
            {
                return ResponseDto<StockCheckAndPaymentProcessResponseDto>.Success(HttpStatusCode.OK.GetHashCode(), new() { Description = "ödeme süreci tamamlandı." });
            }


            return ResponseDto<StockCheckAndPaymentProcessResponseDto>.Fail(HttpStatusCode.BadRequest.GetHashCode(), failMessage!);


        }




    }
}
