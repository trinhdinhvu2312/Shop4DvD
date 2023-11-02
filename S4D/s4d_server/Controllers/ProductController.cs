using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using s4dServer.DTOs.Request;
using s4dServer.DTOs.Response;
using s4dServer.Models;
using s4dServer.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace s4dServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly IProductService productService;

        public ProductController(IProductService productService)
        {
            this.productService = productService;
        }

        [AllowAnonymous]
        [HttpGet("[action]")]
        public async Task<ActionResult<ServiceResponse<PagingModel<ProductResponseDTO>>>> GetAllProducts(int page, int pageSize, string? productName, string? category)
        {
            var response = new ServiceResponse<PagingModel<ProductResponseDTO>>();

            try
            {
                var products = await productService.GetAllProducts(page, pageSize, productName, category);
                response.Data = products;
                response.Status = true;
                response.ErrorCode = 200;
                response.Message = "Products retrieved successfully.";
                return response;
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Status = 500,
                    Title = "An error occurred while retrieving products.",
                    Detail = e.Message
                });
            }
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<ServiceResponse<ProductResponseDTO>>> AddProduct([FromBody] ProductRequestDTO newItem)
        {
            var response = await productService.AddItem(newItem);
            return response;
        }

        [HttpPut("[action]/{productId}")]
        public async Task<ActionResult<ServiceResponse<ProductResponseDTO>>> UpdateProduct(int productId, [FromBody] ProductRequestDTO updatedItem)
        {
            var response = await productService.UpdateItem(updatedItem);
            return response;
        }

        [AllowAnonymous]
        [HttpGet("[action]/{productId}")]
        public async Task<ActionResult<ServiceResponse<ProductResponseDTO>>> GetProductById(int productId)
        {
            var response = new ServiceResponse<ProductResponseDTO>();

            try
            {
                var product = productService.GetProductById(productId);
                if (product == null)
                {
                    return NotFound(new ProblemDetails
                    {
                        Status = 404,
                        Title = "Product not found."
                    });
                }

                response.Data = product;
                response.Status = true;
                response.ErrorCode = 200;
                response.Message = "Product retrieved successfully.";
                return response;
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Status = 500,
                    Title = "An error occurred while retrieving the product.",
                    Detail = e.Message
                });
            }
        }

        [HttpDelete("[action]/{productId}")]
        public async Task<ActionResult<ServiceResponse<int>>> DeleteProduct(int productId)
        {
            var response = new ServiceResponse<int>();

            try
            {
                var existingProduct = productService.GetProductById(productId);
                if (existingProduct == null)
                {
                    return NotFound(new ProblemDetails
                    {
                        Status = 404,
                        Title = "Product not found."
                    });
                }

                var result = await productService.DeleteProduct(productId);
                response.Status = true;
                response.ErrorCode = 200;
                response.Message = "Product deleted successfully.";
                return response;
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Status = 500,
                    Title = "An error occurred while deleting the product.",
                    Detail = e.Message
                });
            }
        }

        [AllowAnonymous]
        [HttpGet("[action]/category/{categoryId}")]
        public ActionResult<List<ProductResponseDTO>> GetProductsByCategoryId(int categoryId)
        {
            var products = productService.GetProductByCategoryId(categoryId);
            return products;
        }

        [AllowAnonymous]
        [HttpGet("[action]/search")]
        public async Task<ActionResult<List<ProductResponseDTO>>> SearchProducts(int categoryId, string? search)
        {
            var products = await productService.SearchProduct(categoryId, search);
            return products;
        }
    }
}