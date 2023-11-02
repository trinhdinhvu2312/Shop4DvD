using s4dServer.DTOs.Request;
using s4dServer.DTOs.Response;
using s4dServer.Models;

namespace s4dServer.Services
{
    public interface IOrderDetailService
    {
        public Task<ServiceResponse<IEnumerable<OrderDetailResponseDTO>>> CreateOrderDetail(IEnumerable<ProductOrderRequestDTO> productOrderRequestDTOs);

        public Task<ServiceResponse<bool>> RemoveOrderDetail(OrderDetailRequestDTO orderDetailRequestDTO);

        public Task<ServiceResponse<bool>> EditOrderDetail(OrderDetailRequestDTO orderDetailRequestDTO);
    }
}