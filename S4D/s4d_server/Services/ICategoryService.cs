using s4dServer.DTOs.Request;
using s4dServer.DTOs.Response;
using s4dServer.Models;

namespace s4dServer.Services
{
    public interface ICategoryService
    {
        public Task<PagingModel<CategoryResponseDTO>> SearchAllCategories(int page, int pageSize, string? categoryName);
 
        public Task<CategoryResponseDTO> AddCategory(CategoryRequestDTO categoryRequestDTO);

        public Task<CategoryResponseDTO> UpdateCategory(CategoryRequestDTO categoryRequestDTO);

        public Task<bool> CheckExistCategory(CategoryRequestDTO categoryRequestDTO);

        public Task<CategoryResponseDTO> GetCategoryById(int categoryId);

        public Task<bool> DeleteCategory(int categoryId);
    }
}