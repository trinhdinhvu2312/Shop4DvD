using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using s4dServer.DTOs.Request;
using s4dServer.DTOs.Response;
using s4dServer.Models;
using s4dServer.Services;

namespace s4dServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService orderService;

        public OrderController(IOrderService orderService)
        {
            this.orderService = orderService;
        }

        [AllowAnonymous]
        [HttpGet("{orderId}")]
        public async Task<ActionResult<ServiceResponse<OrderResponseDTO>>> GetOrderById(int orderId)
        {
            var response = await orderService.GetOrderById(orderId);
            if (!response.Status)
            {
                return NotFound(new ProblemDetails
                {
                    Status = 404,
                    Title = "Order not found."
                });
            }

            return response;
        }

        [AllowAnonymous]
        [HttpGet("info/{orderId}")]
        public async Task<ActionResult<ServiceResponse<OrderInfoResponseDTO>>> GetOrderInfoByOrderId(int orderId)
        {
            var response = await orderService.GetOrderInfoByOrderId(orderId);
            if (!response.Status)
            {
                return NotFound(new ProblemDetails
                {
                    Status = 404,
                    Title = "Order not found."
                });
            }

            return response;
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<OrderResponseDTO>>> AddOrder([FromBody] OrderRequestDTO orderRequestDTO)
        {
            var response = await orderService.AddOrder(orderRequestDTO);
            if (!response.Status)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Status = 500,
                    Title = "An error occurred while adding the order.",
                    Detail = response.Message
                });
            }

            return response;
        }

        [HttpPut("{orderId}")]
        public async Task<ActionResult<ServiceResponse<OrderResponseDTO>>> UpdateOrder(int orderId, [FromBody] OrderRequestDTO orderRequestDTO)
        {
            var response = await orderService.UpdateOrder(orderRequestDTO);
            if (!response.Status)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Status = 500,
                    Title = "An error occurred while updating the order.",
                    Detail = response.Message
                });
            }

            return response;
        }

        [HttpDelete("{orderId}")]
        public async Task<ActionResult<ServiceResponse<bool>>> RemoveOrder(int orderId)
        {
            var result = await orderService.RemoveOrder(orderId);
            if (result == 0)
            {
                return NotFound(new ProblemDetails
                {
                    Status = 404,
                    Title = "Order not found."
                });
            }

            return new ServiceResponse<bool>
            {
                Data = true,
                Status = true,
                Message = "Order removed successfully."
            };
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<ServiceResponse<PagingModel<OrderResponseDTO>>>> GetAllOrders(int page, int pageSize, int userId)
        {
            var response = new ServiceResponse<PagingModel<OrderResponseDTO>>();

            try
            {
                var orders = await orderService.SearchAllOrders(page, pageSize, userId);
                response.Data = orders;
                response.Status = true;
                response.ErrorCode = 200;
                response.Message = "Orders retrieved successfully.";
                return response;
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Status = 500,
                    Title = "An error occurred while retrieving orders.",
                    Detail = e.Message
                });
            }
        }
    }
}