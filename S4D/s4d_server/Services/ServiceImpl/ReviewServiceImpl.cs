using AutoMapper;
using Microsoft.EntityFrameworkCore;
using s4dServer.DTOs.Request;
using s4dServer.DTOs.Response;
using s4dServer.Models;
using s4dServer.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace s4dServer.Services.ServiceImpl
{
    public class ReviewServiceImpl : IReviewService
    {
        private readonly S4DContext _context;
        private readonly IMapper _mapper;

        public ReviewServiceImpl(S4DContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<ReviewResponseDTO>> AddReview(ReviewRequestDTO newReview)
        {
            var product = await _context.Products.FindAsync(newReview.ProductID);
            var user = await _context.Users.FindAsync(newReview.UserID);

            if (product == null || user == null)
            {
                return new ServiceResponse<ReviewResponseDTO>
                {
                    Data = null,
                    Message = "Product or user not found",
                    Status = false
                };
            }

            var review = _mapper.Map<Review>(newReview);
            review.Product = product;
            review.User = user;

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            var responseDTO = _mapper.Map<ReviewResponseDTO>(review);

            return new ServiceResponse<ReviewResponseDTO>
            {
                Data = responseDTO,
                Message = "Review added successfully",
                Status = true
            };
        }

        public async Task<ServiceResponse<ReviewResponseDTO>> UpdateReview(ReviewRequestDTO updatedReview)
        {
            var review = await _context.Reviews.FindAsync(updatedReview.ReviewId);

            if (review == null)
            {
                return new ServiceResponse<ReviewResponseDTO>
                {
                    Data = null,
                    Message = "Review not found",
                    Status = false
                };
            }

            var product = await _context.Products.FindAsync(updatedReview.ProductID);
            var user = await _context.Users.FindAsync(updatedReview.UserID);

            if (product == null || user == null)
            {
                return new ServiceResponse<ReviewResponseDTO>
                {
                    Data = null,
                    Message = "Product or user not found",
                    Status = false
                };
            }

            _mapper.Map(updatedReview, review);
            review.Product = product;
            review.User = user;

            _context.Reviews.Update(review);
            await _context.SaveChangesAsync();

            var responseDTO = _mapper.Map<ReviewResponseDTO>(review);

            return new ServiceResponse<ReviewResponseDTO>
            {
                Data = responseDTO,
                Message = "Review updated successfully",
                Status = true
            };
        }

        public async Task<ServiceResponse<bool>> DeleteReview(int reviewId)
        {
            var review = await _context.Reviews.FindAsync(reviewId);

            if (review == null)
            {
                return new ServiceResponse<bool>
                {
                    Data = false,
                    Message = "Review not found",
                    Status = false
                };
            }

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();

            return new ServiceResponse<bool>
            {
                Data = true,
                Message = "Review deleted successfully",
                Status = true
            };
        }

        public async Task<ReviewResponseDTO> GetReviewById(int reviewId)
        {
            var review = await _context.Reviews.FindAsync(reviewId);
            var responseDTO = _mapper.Map<ReviewResponseDTO>(review);
            return responseDTO;
        }

        public async Task<List<ReviewResponseDTO>> GetReviewsByProductId(int productId)
        {
            var reviews = await _context.Reviews.Where(r => r.ProductID == productId).ToListAsync();
            var responseDTOs = _mapper.Map<List<ReviewResponseDTO>>(reviews);
            return responseDTOs;
        }

        public async Task<List<ReviewResponseDTO>> GetReviewsByUserId(int userId)
        {
            var reviews = await _context.Reviews.Where(r => r.UserID == userId).ToListAsync();
            var responseDTOs = _mapper.Map<List<ReviewResponseDTO>>(reviews);
            return responseDTOs;
        }
    }
}