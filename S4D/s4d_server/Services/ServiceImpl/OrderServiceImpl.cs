using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.SignalR;
using s4dServer.DTOs.Request;
using s4dServer.DTOs.Response;
using s4dServer.Models;
using s4dServer.Services;

namespace s4dServices.ServiceImpl
{
    public class OrderServiceImpl : IOrderService
    {
        private readonly S4DContext _context;
        private readonly IMapper _mapper;

        public OrderServiceImpl(S4DContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<OrderResponseDTO>> AddOrder(OrderRequestDTO orderRequestDTO)
        {
            // Map the OrderRequestDTO to an Order entity
            var order = _mapper.Map<Order>(orderRequestDTO);

            // Add the order to the context
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // Map the created order entity back to OrderResponseDTO
            var responseDTO = _mapper.Map<OrderResponseDTO>(order);

            return new ServiceResponse<OrderResponseDTO>
            {
                Data = responseDTO,
                Message = "Order added successfully",
                Status = true
            };
        }

        public async Task<ServiceResponse<OrderResponseDTO>> UpdateOrder(OrderRequestDTO orderRequestDTO)
        {
            // Find the order by ID
            var order = await _context.Orders.FindAsync(orderRequestDTO.OrderId);

            if (order == null)
            {
                return new ServiceResponse<OrderResponseDTO>
                {
                    Data = null,
                    Message = "Order not found",
                    Status = false
                };
            }

            // Map the updated data from OrderRequestDTO to the existing order entity
            _mapper.Map(orderRequestDTO, order);

            // Update the order in the context
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();

            // Map the updated order entity back to OrderResponseDTO
            var responseDTO = _mapper.Map<OrderResponseDTO>(order);

            return new ServiceResponse<OrderResponseDTO>
            {
                Data = responseDTO,
                Message = "Order updated successfully",
                Status = true
            };
        }

        public async Task<ServiceResponse<OrderInfoResponseDTO>> GetOrderInfoByOrderId(int orderId)
        {
            // Find the order by ID
            var order = await _context.Orders.FindAsync(orderId);

            if (order == null)
            {
                return new ServiceResponse<OrderInfoResponseDTO>
                {
                    Data = null,
                    Message = "Order not found",
                    Status = false
                };
            }

            // Map the order entity to OrderInfoResponseDTO
            var responseDTO = _mapper.Map<OrderInfoResponseDTO>(order);

            return new ServiceResponse<OrderInfoResponseDTO>
            {
                Data = responseDTO,
                Message = "Order information retrieved successfully",
                Status = true
            };
        }

        public async Task<ServiceResponse<OrderResponseDTO>> GetOrderById(int orderId)
        {
            // Find the order by ID
            var order = await _context.Orders.FindAsync(orderId);

            if (order == null)
            {
                return new ServiceResponse<OrderResponseDTO>
                {
                    Data = null,
                    Message = "Order not found",
                    Status = false
                };
            }

            // Map the order entity to OrderResponseDTO
            var responseDTO = _mapper.Map<OrderResponseDTO>(order);

            return new ServiceResponse<OrderResponseDTO>
            {
                Data = responseDTO,
                Message = "Order retrieved successfully",
                Status = true
            };
        }

        public async Task<PagingModel<OrderResponseDTO>> SearchAllOrders(int page, int pageSize, int userId)
        {
            var pagingModel = new PagingModel<OrderResponseDTO>();

            try
            {
                // Get the orders based on the provided page, pageSize, and userId
                var orders = await _context.Orders
                    .Where(o => o.UserID == userId)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                // Map the orders to OrderResponseDTO
                var responseDTOs = _mapper.Map<List<OrderResponseDTO>>(orders);

                // Get the total count of orders for the given userId
                var totalCount = await _context.Orders.Where(o => o.UserID == userId).CountAsync();

                pagingModel.Data = responseDTOs;
                pagingModel.TotalCount = totalCount;
                pagingModel.TotalPage = (int)Math.Ceiling((double)totalCount / pageSize);
            }
            catch (Exception e)
            {
                // Handle any exceptions that occur during the process
                throw new Exception("Failed to retrieve orders", e);
            }

            return pagingModel;
        }

        public async Task<int> GetAllOrdersTotal(string? searchValue)
        {
            // Nếu searchValue được cung cấp, lấy tổng số đơn hàng phù hợp với searchValue. Ngược lại, lấy tổng số đơn hàng tất cả.
            if (!string.IsNullOrEmpty(searchValue))
            {
                // Lấy tổng số đơn hàng phù hợp với searchValue
                return await _context.Orders.Where(o => o.OrderID.ToString().Contains(searchValue)).CountAsync();
            }
            else
            {
                // Lấy tổng số đơn hàng tất cả
                return await _context.Orders.CountAsync();
            }
        }

        public async Task<int> RemoveOrder(int orderId)
        {
            // Find the order by ID
            var order = await _context.Orders.FindAsync(orderId);

            if (order == null)
            {
                return 0;
            }

            // Remove the order from the context
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return 1;
        }
    }
}