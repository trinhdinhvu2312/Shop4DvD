using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using s4dServer.DTOs.Request;
using s4dServer.DTOs.Response;
using s4dServer.Services;

namespace s4dServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderDetailController : ControllerBase
    {
        private readonly IOrderDetailService orderDetailService;

        public OrderDetailController(IOrderDetailService orderDetailService)
        {
            this.orderDetailService = orderDetailService;
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<ServiceResponse<IEnumerable<OrderDetailResponseDTO>>>> CreateOrderDetail(IEnumerable<ProductOrderRequestDTO> productOrderRequestDTOs)
        {
            var response = await orderDetailService.CreateOrderDetail(productOrderRequestDTOs);
            return StatusCode(response.Status ? StatusCodes.Status200OK : StatusCodes.Status500InternalServerError, response);
        }

        [HttpDelete("[action]")]
        public async Task<ActionResult<ServiceResponse<bool>>> RemoveOrderDetail(OrderDetailRequestDTO orderDetailRequestDTO)
        {
            var response = await orderDetailService.RemoveOrderDetail(orderDetailRequestDTO);
            return StatusCode(response.Status ? StatusCodes.Status200OK : StatusCodes.Status500InternalServerError, response);
        }

        [HttpPut("[action]")]
        public async Task<ActionResult<ServiceResponse<bool>>> EditOrderDetail(OrderDetailRequestDTO orderDetailRequestDTO)
        {
            var response = await orderDetailService.EditOrderDetail(orderDetailRequestDTO);
            return StatusCode(response.Status ? StatusCodes.Status200OK : StatusCodes.Status500InternalServerError, response);
        }
    }
}