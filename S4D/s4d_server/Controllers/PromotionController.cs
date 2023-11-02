using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using s4dServer.DTOs.Request;
using s4dServer.DTOs.Response;
using s4dServer.Models;
using s4dServer.Services;
using System;
using System.Threading.Tasks;

namespace s4dServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PromotionController : ControllerBase
    {
        private readonly IPromotionService promotionService;

        public PromotionController(IPromotionService promotionService)
        {
            this.promotionService = promotionService;
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<ServiceResponse<PagingModel<PromotionResponseDTO>>>> GetAllPromotions(int page, int pageSize)
        {
            var response = new ServiceResponse<PagingModel<PromotionResponseDTO>>();

            try
            {
                var promotions = await promotionService.GetAllPromotions(page, pageSize);
                response.Data = promotions;
                response.Status = true;
                response.Message = "Promotions retrieved successfully.";
                return response;
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Status = 500,
                    Title = "An error occurred while retrieving promotions.",
                    Detail = e.Message
                });
            }
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<ServiceResponse<PromotionResponseDTO>>> AddPromotion([FromBody] PromotionRequestDTO newPromotion)
        {
            var response = await promotionService.AddPromotion(newPromotion);
            return response;
        }

        [HttpPut("[action]/{promotionId}")]
        public async Task<ActionResult<ServiceResponse<PromotionResponseDTO>>> UpdatePromotion(int promotionId, [FromBody] PromotionRequestDTO updatedPromotion)
        {
            var response = await promotionService.UpdatePromotion(updatedPromotion);
            return response;
        }

        [HttpDelete("[action]/{promotionId}")]
        public async Task<ActionResult<ServiceResponse<bool>>> DeletePromotion(int promotionId)
        {
            var response = await promotionService.DeletePromotion(promotionId);
            return response;
        }

        [HttpPost("[action]/{promotionId}/products/{productId}")]
        public async Task<ActionResult<ServiceResponse<bool>>> AddProductToPromotion(int promotionId, int productId)
        {
            var response = await promotionService.AddProductToPromotion(promotionId, productId);
            return response;
        }

        [HttpDelete("[action]/{promotionId}/products/{productId}")]
        public async Task<ActionResult<ServiceResponse<bool>>> RemoveProductFromPromotion(int promotionId, int productId)
        {
            var response = await promotionService.RemoveProductFromPromotion(promotionId, productId);
            return response;
        }

        [HttpGet("[action]/{promotionId}/products")]
        public async Task<ActionResult<List<ProductResponseDTO>>> GetProductsInPromotion(int promotionId)
        {
            var products = await promotionService.GetProductsInPromotion(promotionId);
            return products;
        }

        [HttpGet("[action]/{promotionId}")]
        public async Task<ActionResult<PromotionResponseDTO>> GetPromotionById(int promotionId)
        {
            var promotion = await promotionService.GetPromotionById(promotionId);
            if (promotion == null)
            {
                return NotFound(new ProblemDetails
                {
                    Status = 404,
                    Title = "Promotion not found."
                });
            }

            return promotion;
        }
    }
}