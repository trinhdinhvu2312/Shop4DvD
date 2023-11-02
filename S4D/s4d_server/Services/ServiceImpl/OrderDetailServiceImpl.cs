using s4dServer.DTOs.Request;
using s4dServer.DTOs.Response;
using s4dServer.Models;
using s4dServer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace s4dServer.Services.ServiceImpl
{
    public class OrderDetailServiceImpl : IOrderDetailService
    {
        private readonly S4DContext _context;

        public OrderDetailServiceImpl(S4DContext context)
        {
            _context = context;
        }

        public async Task<ServiceResponse<IEnumerable<OrderDetailResponseDTO>>> CreateOrderDetail(IEnumerable<ProductOrderRequestDTO> productOrderRequestDTOs)
        {
            try
            {
                // Tạo danh sách chi tiết đơn hàng từ danh sách yêu cầu đơn hàng
                var orderDetails = new List<OrderDetail>();

                foreach (var productOrderRequestDTO in productOrderRequestDTOs)
                {
                    var product = await _context.Products.FindAsync(productOrderRequestDTO.ProductId);

                    if (product == null)
                    {
                        return new ServiceResponse<IEnumerable<OrderDetailResponseDTO>>
                        {
                            Data = null,
                            Message = $"Product with ID {productOrderRequestDTO.ProductId} not found",
                            Status = false
                        };
                    }

                    var orderDetail = new OrderDetail
                    {
                        OrderID = productOrderRequestDTO.OrderId,
                        Product = product,
                        Quantity = productOrderRequestDTO.Quantity,
                        Subtotal = CalculateSubtotal(product, productOrderRequestDTO.Quantity)
                    };

                    orderDetails.Add(orderDetail);
                }

                // Thêm danh sách chi tiết đơn hàng vào cơ sở dữ liệu
                _context.OrderDetails.AddRange(orderDetails);
                await _context.SaveChangesAsync();

                // Chuyển đổi danh sách chi tiết đơn hàng thành DTO và trả về kết quả thành công
                var responseDTOs = orderDetails.Select(od => new OrderDetailResponseDTO
                {
                    OrderDetailID = od.OrderDetailID,
                    OrderID = od.OrderID,
                    Product = new ProductResponseDTO
                    {
                        ProductID = od.Product.ProductID,
                        ProductName = od.Product.ProductName,
                        // Các thông tin khác của sản phẩm có thể được thêm vào đây
                    },
                    Quantity = od.Quantity,
                    Subtotal = od.Subtotal
                });

                return new ServiceResponse<IEnumerable<OrderDetailResponseDTO>>
                {
                    Data = responseDTOs,
                    Message = "Order details created successfully",
                    Status = true
                };
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ và trả về kết quả không thành công
                return new ServiceResponse<IEnumerable<OrderDetailResponseDTO>>
                {
                    Data = null,
                    Message = $"Failed to create order details: {ex.Message}",
                    Status = false
                };
            }
        }

        private decimal CalculateSubtotal(Product product, int quantity)
        {
            decimal subtotal = product.Price * quantity;

            // Kiểm tra nếu sản phẩm có Promotion và Promotion đang có hiệu lực
            var promotionProduct = _context.PromotionProducts.FirstOrDefault(pp => pp.ProductID == product.ProductID);
            if (promotionProduct != null && promotionProduct.Promotion != null && IsPromotionValid(promotionProduct.Promotion))
            {
                subtotal -= subtotal * promotionProduct.Promotion.DiscountPercentage;
            }

            return subtotal;
        }

        private bool IsPromotionValid(Promotion promotion)
        {
            // Kiểm tra xem Promotion có trong khoảng thời gian hiệu lực hay không
            DateTime currentDate = DateTime.Now.Date;

            if (promotion.StartDate.HasValue && promotion.EndDate.HasValue)
            {
                if (currentDate >= promotion.StartDate.Value && currentDate <= promotion.EndDate.Value)
                {
                    return true;
                }
            }

            return false;
        }

        public async Task<ServiceResponse<bool>> RemoveOrderDetail(OrderDetailRequestDTO orderDetailRequestDTO)
        {
            try
            {
                // Tìm chi tiết đơn hàng cần xóa
                var orderDetail = await _context.OrderDetails.FindAsync(orderDetailRequestDTO.OrderDetailId);

                if (orderDetail == null)
                {
                    return new ServiceResponse<bool>
                    {
                        Data = false,
                        Message = $"Order detail with ID {orderDetailRequestDTO.OrderDetailId} not found",
                        Status = false
                    };
                }

                // Xóa chi tiết đơn hàng khỏi cơ sở dữ liệu
                _context.OrderDetails.Remove(orderDetail);
                await _context.SaveChangesAsync();

                // Trả về kết quả thành công
                return new ServiceResponse<bool>
                {
                    Data = true,
                    Message = "Order detail removed successfully",
                    Status = true
                };
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ và trả về kết quả không thành công
                return new ServiceResponse<bool>
                {
                    Data = false,
                    Message = $"Failed to remove order detail: {ex.Message}",
                    Status = false
                };
            }
        }

        public async Task<ServiceResponse<bool>> EditOrderDetail(OrderDetailRequestDTO orderDetailRequestDTO)
        {
            try
            {
                // Tìm chi tiết đơn hàng cần chỉnh sửa
                var orderDetail = await _context.OrderDetails.FindAsync(orderDetailRequestDTO.OrderDetailId);

                if (orderDetail == null)
                {
                    return new ServiceResponse<bool>
                    {
                        Data = false,
                        Message = $"Order detail with ID {orderDetailRequestDTO.OrderDetailId} not found",
                        Status = false
                    };
                }

                // Cập nhật thông tin chi tiết đơn hàng
                orderDetail.Quantity = orderDetailRequestDTO.Quantity;
                orderDetail.Subtotal = orderDetail.Product.Price * orderDetailRequestDTO.Quantity;

                _context.OrderDetails.Update(orderDetail);
                await _context.SaveChangesAsync();

                // Trả về kết quả thành công
                return new ServiceResponse<bool>
                {
                    Data = true,
                    Message = "Order detail updated successfully",
                    Status = true
                };
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ và trả về kết quả không thành công
                return new ServiceResponse<bool>
                {
                    Data = false,
                    Message = $"Failed to edit order detail: {ex.Message}",
                    Status = false
                };
            }
        }
    }
}