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
    public class AlbumController : ControllerBase
    {
        private readonly IAlbumService albumService;

        public AlbumController(IAlbumService albumService)
        {
            this.albumService = albumService;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<ServiceResponse<PagingModel<AlbumResponseDTO>>>> GetAllAlbums(int page, int pageSize, string? albumName)
        {
            var response = new ServiceResponse<PagingModel<AlbumResponseDTO>>();

            try
            {
                var albums = await albumService.SearchAllAlbums(page, pageSize, albumName);
                response.Data = albums;
                response.Status = true;
                response.ErrorCode = 200;
                response.Message = "Albums retrieved successfully.";
                return response;
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Status = 500,
                    Title = "An error occurred while retrieving albums.",
                    Detail = e.Message
                });
            }
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<AlbumResponseDTO>>> AddAlbum([FromBody] AlbumRequestDTO albumRequestDTO)
        {
            var response = new ServiceResponse<AlbumResponseDTO>();

            try
            {
                var album = await albumService.AddAlbum(albumRequestDTO);
                response.Data = album;
                response.Status = true;
                response.ErrorCode = 200;
                response.Message = "Album added successfully.";
                return response;
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Status = 500,
                    Title = "An error occurred while adding the album.",
                    Detail = e.Message
                });
            }
        }

        [HttpPut("{albumId}")]
        public async Task<ActionResult<ServiceResponse<AlbumResponseDTO>>> UpdateAlbum(int albumId, [FromBody] AlbumRequestDTO albumRequestDTO)
        {
            var response = new ServiceResponse<AlbumResponseDTO>();

            try
            {
                var existingAlbum = await albumService.GetAlbumById(albumId);
                if (existingAlbum == null)
                {
                    return NotFound(new ProblemDetails
                    {
                        Status = 404,
                        Title = "Album not found."
                    });
                }

                var updatedAlbum = await albumService.UpdateAlbum(albumRequestDTO);
                response.Data = updatedAlbum;
                response.Status = true;
                response.ErrorCode = 200;
                response.Message = "Album updated successfully.";
                return response;
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Status = 500,
                    Title = "An error occurred while updating the album.",
                    Detail = e.Message
                });
            }
        }

        [AllowAnonymous]
        [HttpGet("{albumId}")]
        public async Task<ActionResult<ServiceResponse<AlbumResponseDTO>>> GetAlbumById(int albumId)
        {
            var response = new ServiceResponse<AlbumResponseDTO>();

            try
            {
                var album = await albumService.GetAlbumById(albumId);
                if (album == null)
                {
                    return NotFound(new ProblemDetails
                    {
                        Status = 404,
                        Title = "Album not found."
                    });
                }

                response.Data = album;
                response.Status = true;
                response.ErrorCode = 200;
                response.Message = "Album retrieved successfully.";
                return response;
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Status = 500,
                    Title = "An error occurred while retrieving the album.",
                    Detail = e.Message
                });
            }
        }

        [HttpDelete("{albumId}")]
        public async Task<ActionResult<ServiceResponse<bool>>> DeleteAlbum(int albumId)
        {
            var response = new ServiceResponse<bool>();

            try
            {
                var existingAlbum = await albumService.GetAlbumById(albumId);
                if (existingAlbum == null)
                {
                    return NotFound(new ProblemDetails
                    {
                        Status = 404,
                        Title = "Album not found."
                    });
                }

                var result = await albumService.DeleteAlbum(albumId);
                response.Data = result;
                response.Status = true;
                response.ErrorCode = 200;
                response.Message = "Album deleted successfully.";
                return response;
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Status = 500,
                    Title = "An error occurred while deleting the album.",
                    Detail = e.Message
                });
            }
        }
    }
}