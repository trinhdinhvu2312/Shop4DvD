using s4dServer.DTOs.Request;
using s4dServer.DTOs.Response;
using s4dServer.Models;

namespace s4dServer.Services
{
    public interface IArtistService
    {
        public Task<PagingModel<ArtistResponseDTO>> SearchAllArtists(int page, int pageSize, string? artistName);

        public Task<ArtistResponseDTO> AddArtist(ArtistRequestDTO artistRequestDTO);

        public Task<ArtistResponseDTO> UpdateArtist(ArtistRequestDTO artistRequestDTO);

        public Task<bool> CheckExistArtist(ArtistRequestDTO artistRequestDTO);

        public Task<ArtistResponseDTO> GetArtistById(int artistId);

        public Task<bool> DeleteArtist(int artistId);
    }
}