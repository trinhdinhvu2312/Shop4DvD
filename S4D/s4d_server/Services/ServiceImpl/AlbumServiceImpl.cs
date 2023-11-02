using AutoMapper;
using Microsoft.EntityFrameworkCore;
using s4dServer.DTOs.Request;
using s4dServer.DTOs.Response;
using s4dServer.Models;

namespace s4dServer.Services.ServiceImpl
{
    public class AlbumServiceImpl : IAlbumService
    {
        private readonly S4DContext _context;
        private readonly IMapper _mapper;

        public AlbumServiceImpl(S4DContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<AlbumResponseDTO> AddAlbum(AlbumRequestDTO albumRequestDTO)
        {
            try
            {
                var album = _mapper.Map<Album>(albumRequestDTO);

                _context.Albums.Add(album);
                await _context.SaveChangesAsync();

                var albumResponseDTO = _mapper.Map<AlbumResponseDTO>(album);
                return albumResponseDTO;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<bool> CheckExistAlbum(AlbumRequestDTO albumRequestDTO)
        {
            try
            {
                var existingAlbum = await _context.Albums.FirstOrDefaultAsync(a => a.AlbumTitle.ToLower().Trim() == albumRequestDTO.AlbumTitle.ToLower().Trim());
                return existingAlbum != null;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<bool> DeleteAlbum(int albumId)
        {
            try
            {
                var album = await _context.Albums.FindAsync(albumId);
                if (album == null)
                {
                    throw new Exception("Album not found.");
                }

                _context.Albums.Remove(album);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<AlbumResponseDTO> GetAlbumById(int albumId)
        {
            try
            {
                var album = await _context.Albums.FindAsync(albumId);
                if (album == null)
                {
                    throw new Exception("Album not found.");
                }

                var albumResponseDTO = _mapper.Map<AlbumResponseDTO>(album);
                return albumResponseDTO;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<PagingModel<AlbumResponseDTO>> SearchAllAlbums(int page, int pageSize, string? albumName)
        {
            try
            {
                IQueryable<Album> query = _context.Albums;
                if (!string.IsNullOrEmpty(albumName))
                {
                    query = query.Where(a => a.AlbumTitle.ToLower().Contains(albumName.ToLower()));
                }

                int totalCount = await query.CountAsync();

                var albums = await query
                    .OrderByDescending(a => a.AlbumID)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var albumResponseDTOs = _mapper.Map<List<AlbumResponseDTO>>(albums);

                var pagingModel = new PagingModel<AlbumResponseDTO>
                {
                    Data = albumResponseDTOs,
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

        public async Task<AlbumResponseDTO> UpdateAlbum(AlbumRequestDTO albumRequestDTO)
        {
            try
            {
                var album = await _context.Albums.FindAsync(albumRequestDTO.AlbumId);
                if (album == null)
                {
                    throw new Exception("Album not found.");
                }

                album.AlbumTitle = albumRequestDTO.AlbumTitle ?? album.AlbumTitle;

                await _context.SaveChangesAsync();

                var albumResponseDTO = _mapper.Map<AlbumResponseDTO>(album);
                return albumResponseDTO;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}