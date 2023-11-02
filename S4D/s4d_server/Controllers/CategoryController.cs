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
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<ServiceResponse<PagingModel<CategoryResponseDTO>>>> GetAllCategories(int page, int pageSize, string? categoryName)
        {
            var response = new ServiceResponse<PagingModel<CategoryResponseDTO>>();

            try
            {
                var categories = await categoryService.SearchAllCategories(page, pageSize, categoryName);
                response.Data = categories;
                response.Status = true;
                response.ErrorCode = 200;
                response.Message = "Categories retrieved successfully.";
                return response;
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Status = 500,
                    Title = "An error occurred while retrieving categories.",
                    Detail = e.Message
                });
            }
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<CategoryResponseDTO>>> AddCategory([FromBody] CategoryRequestDTO categoryRequestDTO)
        {
            var response = new ServiceResponse<CategoryResponseDTO>();

            try
            {
                var category = await categoryService.AddCategory(categoryRequestDTO);
                response.Data = category;
                response.Status = true;
                response.ErrorCode = 200;
                response.Message = "Category added successfully.";
                return response;
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Status = 500,
                    Title = "An error occurred while adding the category.",
                    Detail = e.Message
                });
            }
        }

        [HttpPut("{categoryId}")]
        public async Task<ActionResult<ServiceResponse<CategoryResponseDTO>>> UpdateCategory(int categoryId, [FromBody] CategoryRequestDTO categoryRequestDTO)
        {
            var response = new ServiceResponse<CategoryResponseDTO>();

            try
            {
                var existingCategory = await categoryService.GetCategoryById(categoryId);
                if (existingCategory == null)
                {
                    return NotFound(new ProblemDetails
                    {
                        Status = 404,
                        Title = "Category not found."
                    });
                }

                var updatedCategory = await categoryService.UpdateCategory(categoryRequestDTO);
                response.Data = updatedCategory;
                response.Status = true;
                response.ErrorCode = 200;
                response.Message = "Category updated successfully.";
                return response;
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Status = 500,
                    Title = "An error occurred while updating the category.",
                    Detail = e.Message
                });
            }
        }

        [AllowAnonymous]
        [HttpGet("{categoryId}")]
        public async Task<ActionResult<ServiceResponse<CategoryResponseDTO>>> GetCategoryById(int categoryId)
        {
            var response = new ServiceResponse<CategoryResponseDTO>();

            try
            {
                var category = await categoryService.GetCategoryById(categoryId);
                if (category == null)
                {
                    return NotFound(new ProblemDetails
                    {
                        Status = 404,
                        Title = "Category not found."
                    });
                }

                response.Data = category;
                response.Status = true;
                response.ErrorCode = 200;
                response.Message = "Category retrieved successfully.";
                return response;
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Status = 500,
                    Title = "An error occurred while retrieving the category.",
                    Detail = e.Message
                });
            }
        }

        [HttpDelete("{categoryId}")]
        public async Task<ActionResult<ServiceResponse<bool>>> DeleteCategory(int categoryId)
        {
            var response = new ServiceResponse<bool>();

            try
            {
                var existingCategory = await categoryService.GetCategoryById(categoryId);
                if (existingCategory == null)
                {
                    return NotFound(new ProblemDetails
                    {
                        Status = 404,
                        Title = "Category not found."
                    });
                }

                var result = await categoryService.DeleteCategory(categoryId);
                response.Data = result;
                response.Status = true;
                response.ErrorCode = 200;
                response.Message = "Category deleted successfully.";
                return response;
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Status = 500,
                    Title = "An error occurred while deleting the category.",
                    Detail = e.Message
                });
            }
        }
    }
}