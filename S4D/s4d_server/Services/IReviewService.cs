using s4dServer.DTOs.Request;
using s4dServer.DTOs.Response;
using s4dServer.Models;

namespace s4dServer.Services
{
    public interface IReviewService
    {
        public Task<ServiceResponse<ReviewResponseDTO>> AddReview(ReviewRequestDTO newReview);

        public Task<ServiceResponse<ReviewResponseDTO>> UpdateReview(ReviewRequestDTO updatedReview);

        public Task<ServiceResponse<bool>> DeleteReview(int reviewId);

        public Task<ReviewResponseDTO> GetReviewById(int reviewId);

        public Task<List<ReviewResponseDTO>> GetReviewsByProductId(int productId);

        public Task<List<ReviewResponseDTO>> GetReviewsByUserId(int userId);
    }
}