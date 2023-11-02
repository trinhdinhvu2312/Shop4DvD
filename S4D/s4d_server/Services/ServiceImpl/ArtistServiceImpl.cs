using AutoMapper;
using Microsoft.EntityFrameworkCore;
using s4dServer.DTOs.Request;
using s4dServer.DTOs.Response;
using s4dServer.Models;

namespace s4dServer.Services.ServiceImpl
{
    public class ArtistServiceImpl : IArtistService
    {
        private readonly S4DContext _context;
        private readonly IMapper _mapper;

        public ArtistServiceImpl(S4DContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ArtistResponseDTO> AddArtist(ArtistRequestDTO artistRequestDTO)
        {
            try
            {
                var artist = _mapper.Map<Artist>(artistRequestDTO);

                _context.Artists.Add(artist);
                await _context.SaveChangesAsync();

                var artistResponseDTO = _mapper.Map<ArtistResponseDTO>(artist);
                return artistResponseDTO;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<bool> CheckExistArtist(ArtistRequestDTO artistRequestDTO)
        {
            try
            {
                var existingArtist = await _context.Artists.FirstOrDefaultAsync(a => a.ArtistName.ToLower().Trim() == artistRequestDTO.ArtistName.ToLower().Trim());
                return existingArtist != null;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<bool> DeleteArtist(int artistId)
        {
            try
            {
                var artist = await _context.Artists.FindAsync(artistId);
                if (artist == null)
                {
                    throw new Exception("Artist not found.");
                }

                _context.Artists.Remove(artist);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<ArtistResponseDTO> GetArtistById(int artistId)
        {
            try
            {
                var artist = await _context.Artists.FindAsync(artistId);
                if (artist == null)
                {
                    throw new Exception("Artist not found.");
                }

                var artistResponseDTO = _mapper.Map<ArtistResponseDTO>(artist);
                return artistResponseDTO;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<PagingModel<ArtistResponseDTO>> SearchAllArtists(int page, int pageSize, string? artistName)
        {
            try
            {
                IQueryable<Artist> query = _context.Artists;
                if (!string.IsNullOrEmpty(artistName))
                {
                    query = query.Where(a => a.ArtistName.ToLower().Contains(artistName.ToLower()));
                }

                int totalCount = await query.CountAsync();

                var artists = await query
                    .OrderByDescending(a => a.ArtistID)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var artistResponseDTOs = _mapper.Map<List<ArtistResponseDTO>>(artists);

                var pagingModel = new PagingModel<ArtistResponseDTO>
                {
                    Data = artistResponseDTOs,
                    TotalCount = totalCount,
                    TotalPage = (int)Math.Ceiling((double)totalCount / pageSize)
                };

                return pagingModel;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<ArtistResponseDTO> UpdateArtist(ArtistRequestDTO artistRequestDTO)
        {
            try
            {
                var artist = await _context.Artists.FindAsync(artistRequestDTO.ArtistId);
                if (artist == null)
                {
                    throw new Exception("Artist not found.");
                }

                artist.ArtistName = artistRequestDTO.ArtistName ?? artist.ArtistName;

                await _context.SaveChangesAsync();

                var artistResponseDTO = _mapper.Map<ArtistResponseDTO>(artist);
                return artistResponseDTO;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}