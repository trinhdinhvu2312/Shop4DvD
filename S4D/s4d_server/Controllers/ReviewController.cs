using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using s4dServer.DTOs.Request;
using s4dServer.DTOs.Response;
using s4dServer.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace s4dServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService reviewService;

        public ReviewController(IReviewService reviewService)
        {
            this.reviewService = reviewService;
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<ServiceResponse<ReviewResponseDTO>>> AddReview([FromBody] ReviewRequestDTO newReview)
        {
            var response = await reviewService.AddReview(newReview);
            return response;
        }

        [HttpPut("[action]/{reviewId}")]
        public async Task<ActionResult<ServiceResponse<ReviewResponseDTO>>> UpdateReview(int reviewId, [FromBody] ReviewRequestDTO updatedReview)
        {
            var response = await reviewService.UpdateReview(updatedReview);
            return response;
        }

        [HttpDelete("[action]/{reviewId}")]
        public async Task<ActionResult<ServiceResponse<bool>>> DeleteReview(int reviewId)
        {
            var response = await reviewService.DeleteReview(reviewId);
            return response;
        }

        [AllowAnonymous]
        [HttpGet("[action]/{reviewId}")]
        public async Task<ActionResult<ReviewResponseDTO>> GetReviewById(int reviewId)
        {
            var review = await reviewService.GetReviewById(reviewId);

            if (review == null)
            {
                return NotFound(new ProblemDetails
                {
                    Status = 404,
                    Title = "Review not found."
                });
            }

            return review;
        }

        [AllowAnonymous]
        [HttpGet("[action]/product/{productId}")]
        public async Task<ActionResult<List<ReviewResponseDTO>>> GetReviewsByProductId(int productId)
        {
            var reviews = await reviewService.GetReviewsByProductId(productId);
            return reviews;
        }

        [AllowAnonymous]
        [HttpGet("[action]/user/{userId}")]
        public async Task<ActionResult<List<ReviewResponseDTO>>> GetReviewsByUserId(int userId)
        {
            var reviews = await reviewService.GetReviewsByUserId(userId);
            return reviews;
        }
    }
}