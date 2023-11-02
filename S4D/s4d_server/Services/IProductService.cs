using s4dServer.DTOs.Request;
using s4dServer.DTOs.Response;
using s4dServer.Models;

namespace s4dServer.Services
{
    public interface IProductService
    {
        public Task<PagingModel<ProductResponseDTO>> GetAllProducts(int page, int pageSize, string? productName, string? category);

        public Task<ServiceResponse<ProductResponseDTO>> AddItem(ProductRequestDTO newItem);

        public Task<ServiceResponse<ProductResponseDTO>> UpdateItem(ProductRequestDTO updatedItem);

        public Task<List<ProductResponseDTO>> GetListProduct();

        public List<ProductResponseDTO> GetProductByCategoryId(int categoryId);

        public Task<List<ProductResponseDTO>> SearchProduct(int categoryId, string? search);

        public ProductResponseDTO GetProductById(int id);

        public Task<ServiceResponse<bool>> DeleteProduct(int productId);
    }
}