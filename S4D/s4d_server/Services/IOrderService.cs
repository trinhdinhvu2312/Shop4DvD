using s4dServer.DTOs.Request;
using s4dServer.DTOs.Response;
using s4dServer.Models;

namespace s4dServer.Services
{
    public interface IOrderService
    {
        public Task<PagingModel<OrderResponseDTO>> SearchAllOrders(int page, int pageSize, int userId);

        public Task<ServiceResponse<OrderResponseDTO>> AddOrder(OrderRequestDTO orderRequestDTO);

        public Task<ServiceResponse<OrderResponseDTO>> UpdateOrder(OrderRequestDTO orderRequestDTO);

        public Task<ServiceResponse<OrderInfoResponseDTO>> GetOrderInfoByOrderId(int orderId);

        public Task<ServiceResponse<OrderResponseDTO>> GetOrderById(int orderId);

        public Task<int> GetAllOrdersTotal(string? searchValue);

        public Task<int> RemoveOrder(int orderId);
    }
}