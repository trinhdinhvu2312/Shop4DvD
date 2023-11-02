using s4dServer.DTOs.Request;
using s4dServer.DTOs.Response;
using s4dServer.Models;

namespace s4dServer.Services
{
    public interface IPromotionService
    {
        public Task<PagingModel<PromotionResponseDTO>> GetAllPromotions(int page, int pageSize);

        public Task<ServiceResponse<PromotionResponseDTO>> AddPromotion(PromotionRequestDTO newPromotion);

        public Task<ServiceResponse<PromotionResponseDTO>> UpdatePromotion(PromotionRequestDTO updatedPromotion);

        public Task<ServiceResponse<bool>> DeletePromotion(int promotionId);

        public Task<ServiceResponse<bool>> AddProductToPromotion(int promotionId, int productId);

        public Task<ServiceResponse<bool>> RemoveProductFromPromotion(int promotionId, int productId);

        public Task<List<ProductResponseDTO>> GetProductsInPromotion(int promotionId);

        public Task<PromotionResponseDTO> GetPromotionById(int promotionId);
    }
}