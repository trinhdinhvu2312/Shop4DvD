using s4dServer.DTOs.Request;
using s4dServer.DTOs.Response;
using s4dServer.Models;

namespace s4dServer.Services
{
    public interface IAlbumService
    {
        public Task<PagingModel<AlbumResponseDTO>> SearchAllAlbums(int page, int pageSize, string? albumName);

        public Task<AlbumResponseDTO> AddAlbum(AlbumRequestDTO albumRequestDTO);

        public Task<AlbumResponseDTO> UpdateAlbum(AlbumRequestDTO albumRequestDTO);

        public Task<bool> CheckExistAlbum(AlbumRequestDTO albumRequestDTO);

        public Task<AlbumResponseDTO> GetAlbumById(int albumId);

        public Task<bool> DeleteAlbum(int albumId);
    }
}