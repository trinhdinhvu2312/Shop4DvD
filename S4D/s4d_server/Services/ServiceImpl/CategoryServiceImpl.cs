using AutoMapper;
using Microsoft.EntityFrameworkCore;
using s4dServer.DTOs.Request;
using s4dServer.DTOs.Response;
using s4dServer.Models;

namespace s4dServer.Services.ServiceImpl
{
    public class CategoryServiceImpl : ICategoryService
    {
        private readonly S4DContext _context;
        private readonly IMapper _mapper;

        public CategoryServiceImpl(S4DContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CategoryResponseDTO> AddCategory(CategoryRequestDTO categoryRequestDTO)
        {
            try
            {
                var category = _mapper.Map<Category>(categoryRequestDTO);

                _context.Categories.Add(category);
                await _context.SaveChangesAsync();

                var categoryResponseDTO = _mapper.Map<CategoryResponseDTO>(category);
                return categoryResponseDTO;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<bool> CheckExistCategory(CategoryRequestDTO categoryRequestDTO)
        {
            try
            {
                var existingCategory = await _context.Categories.FirstOrDefaultAsync(c => c.CategoryName.ToLower().Trim() == categoryRequestDTO.CategoryName.ToLower().Trim());
                return existingCategory != null;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<bool> DeleteCategory(int categoryId)
        {
            try
            {
                var category = await _context.Categories.FindAsync(categoryId);
                if (category == null)
                {
                    throw new Exception("Category not found.");
                }

                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<CategoryResponseDTO> GetCategoryById(int categoryId)
        {
            try
            {
                var category = await _context.Categories.FindAsync(categoryId);
                if (category == null)
                {
                    throw new Exception("Category not found.");
                }

                var categoryResponseDTO = _mapper.Map<CategoryResponseDTO>(category);
                return categoryResponseDTO;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<PagingModel<CategoryResponseDTO>> SearchAllCategories(int page, int pageSize, string? categoryName)
        {
            try
            {
                IQueryable<Category> query = _context.Categories;
                if (!string.IsNullOrEmpty(categoryName))
                {
                    query = query.Where(c => c.CategoryName.ToLower().Contains(categoryName.ToLower()));
                }         

                int totalCount = await query.CountAsync();

                var categories = await query
                    .OrderByDescending(c => c.CategoryID)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var categoryResponseDTOs = _mapper.Map<List<CategoryResponseDTO>>(categories);

                var pagingModel = new PagingModel<CategoryResponseDTO>
                {
                    Data = categoryResponseDTOs,
                    TotalCount = totalCount,
                    TotalPage = (int)Math.Ceiling((double)totalCount / pageSize)
                };

                return pagingModel;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<CategoryResponseDTO> UpdateCategory(CategoryRequestDTO categoryRequestDTO)
        {
            try
            {
                var category = await _context.Categories.FindAsync(categoryRequestDTO.CategoryID);
                if (category == null)
                {
                    throw new Exception("Category not found.");
                }

                category.CategoryName = categoryRequestDTO.CategoryName ?? category.CategoryName;

                await _context.SaveChangesAsync();

                var categoryResponseDTO = _mapper.Map<CategoryResponseDTO>(category);
                return categoryResponseDTO;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}