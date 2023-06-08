using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using OrderService.Hubs;
using OrderService.Store;

namespace OrderService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IHubContext<OrderHub> _hubContext;

        public OrderController(IHubContext<OrderHub> hubContext)
        {
            _hubContext = hubContext;
        }

        [HttpPost]
        public IActionResult AddOrder([FromBody] OrderDTO order)
        {
            if (order.Details == null) return new BadRequestObjectResult("Order Details Cannot be Empty");
            var newOrder = new Order(OrderStore.GetCurrentSequence(), order.Details);
            OrderStore.AddOrder(newOrder);
            _hubContext.Clients.Group("ChatimeSummarecon").SendAsync("ReceiveOrder", newOrder);
            return new OkObjectResult("Success");
        }

        [HttpGet]
        public IActionResult GetOrdersByRange(int from, int to)
        {
            return new OkObjectResult(OrderStore.GetOrdersBySequence(from, to));
        }
    }

    public class OrderDTO
    {
        public int? Id { get; set; }

        public string? Details { get; set; }

        public string? Status { get; set; }
    }
}