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
    public class ArtistController : ControllerBase
    {
        private readonly IArtistService artistService;

        public ArtistController(IArtistService artistService)
        {
            this.artistService = artistService;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<ServiceResponse<PagingModel<ArtistResponseDTO>>>> GetAllArtists(int page, int pageSize, string? artistName)
        {
            var response = new ServiceResponse<PagingModel<ArtistResponseDTO>>();

            try
            {
                var artists = await artistService.SearchAllArtists(page, pageSize, artistName);
                response.Data = artists;
                response.Status = true;
                response.ErrorCode = 200;
                response.Message = "Artists retrieved successfully.";
                return response;
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Status = 500,
                    Title = "An error occurred while retrieving artists.",
                    Detail = e.Message
                });
            }
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<ArtistResponseDTO>>> AddArtist([FromBody] ArtistRequestDTO artistRequestDTO)
        {
            var response = new ServiceResponse<ArtistResponseDTO>();

            try
            {
                var exists = await artistService.CheckExistArtist(artistRequestDTO);
                if (exists)
                {
                    return BadRequest(new ProblemDetails
                    {
                        Status = 400,
                        Title = "Artist already exists.",
                        Detail = "An artist with the same name already exists."
                    });
                }

                var artist = await artistService.AddArtist(artistRequestDTO);
                response.Data = artist;
                response.Status = true;
                response.ErrorCode = 200;
                response.Message = "Artist added successfully.";
                return response;
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Status = 500,
                    Title = "An error occurred while adding the artist.",
                    Detail = e.Message
                });
            }
        }

        [HttpPut("{artistId}")]
        public async Task<ActionResult<ServiceResponse<ArtistResponseDTO>>> UpdateArtist(int artistId, [FromBody] ArtistRequestDTO artistRequestDTO)
        {
            var response = new ServiceResponse<ArtistResponseDTO>();

            try
            {
                var existingArtist = await artistService.GetArtistById(artistId);
                if (existingArtist == null)
                {
                    return NotFound(new ProblemDetails
                    {
                        Status = 404,
                        Title = "Artist not found."
                    });
                }

                var artist = await artistService.UpdateArtist(artistRequestDTO);
                response.Data = artist;
                response.Status = true;
                response.ErrorCode = 200;
                response.Message = "Artist updated successfully.";
                return response;
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Status = 500,
                    Title = "An error occurred while updating the artist.",
                    Detail = e.Message
                });
            }
        }

        [AllowAnonymous]
        [HttpGet("{artistId}")]
        public async Task<ActionResult<ServiceResponse<ArtistResponseDTO>>> GetArtistById(int artistId)
        {
            var response = new ServiceResponse<ArtistResponseDTO>();

            try
            {
                var artist = await artistService.GetArtistById(artistId);
                if (artist == null)
                {
                    return NotFound(new ProblemDetails
                    {
                        Status = 404,
                        Title = "Artist not found."
                    });
                }

                response.Data = artist;
                response.Status = true;
                response.ErrorCode = 200;
                response.Message = "Artist retrieved successfully.";
                return response;
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Status = 500,
                    Title = "An error occurred while retrieving the artist.",
                    Detail = e.Message
                });
            }
        }

        [HttpDelete("{artistId}")]
        public async Task<ActionResult<ServiceResponse<bool>>> DeleteArtist(int artistId)
        {
            var response = new ServiceResponse<bool>();

            try
            {
                var existingArtist = await artistService.GetArtistById(artistId);
                if (existingArtist == null)
                {
                    return NotFound(new ProblemDetails
                    {
                        Status = 404,
                        Title = "Artist not found."
                    });
                }

                var result = await artistService.DeleteArtist(artistId);
                response.Data = result;
                response.Status = true;
                response.ErrorCode = 200;
                response.Message = "Artist deleted successfully.";
                return response;
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Status = 500,
                    Title = "An error occurred while deleting the artist.",
                    Detail = e.Message
                });
            }
        }
    }
}