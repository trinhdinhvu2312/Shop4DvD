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
    public class PromotionServiceImpl : IPromotionService
    {
        private readonly S4DContext _context;
        private readonly IMapper _mapper;

        public PromotionServiceImpl(S4DContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PagingModel<PromotionResponseDTO>> GetAllPromotions(int page, int pageSize)
        {
            var pagingModel = new PagingModel<PromotionResponseDTO>();

            var totalCount = await _context.Promotions.CountAsync();

            var promotions = await _context.Promotions.Skip((page - 1) * pageSize).Take(pageSize)
                .ToListAsync();

            var responseDTOs = _mapper.Map<List<PromotionResponseDTO>>(promotions);

            pagingModel.Data = responseDTOs;
            pagingModel.TotalCount = totalCount;
            pagingModel.TotalPage = (int)System.Math.Ceiling((double)totalCount / pageSize);

            return pagingModel;
        }

        public async Task<ServiceResponse<PromotionResponseDTO>> AddPromotion(PromotionRequestDTO newPromotion)
        {
            var promotion = _mapper.Map<Promotion>(newPromotion);

            _context.Promotions.Add(promotion);
            await _context.SaveChangesAsync();

            var responseDTO = _mapper.Map<PromotionResponseDTO>(promotion);

            return new ServiceResponse<PromotionResponseDTO>
            {
                Data = responseDTO,
                Message = "Promotion added successfully",
                Status = true
            };
        }

        public async Task<ServiceResponse<PromotionResponseDTO>> UpdatePromotion(PromotionRequestDTO updatedPromotion)
        {
            var promotion = await _context.Promotions.FindAsync(updatedPromotion.PromotionID);

            if (promotion == null)
            {
                return new ServiceResponse<PromotionResponseDTO>
                {
                    Data = null,
                    Message = "Promotion not found",
                    Status = false
                };
            }

            _mapper.Map(updatedPromotion, promotion);

            _context.Promotions.Update(promotion);
            await _context.SaveChangesAsync();

            var responseDTO = _mapper.Map<PromotionResponseDTO>(promotion);

            return new ServiceResponse<PromotionResponseDTO>
            {
                Data = responseDTO,
                Message = "Promotion updated successfully",
                Status = true
            };
        }

        public async Task<ServiceResponse<bool>> DeletePromotion(int promotionId)
        {
            var promotion = await _context.Promotions.FindAsync(promotionId);

            if (promotion == null)
            {
                return new ServiceResponse<bool>
                {
                    Data = false,
                    Message = "Promotion not found",
                    Status = false
                };
            }

            _context.Promotions.Remove(promotion);
            await _context.SaveChangesAsync();

            return new ServiceResponse<bool>
            {
                Data = true,
                Message = "Promotion deleted successfully",
                Status = true
            };
        }

        public async Task<ServiceResponse<bool>> AddProductToPromotion(int promotionId, int productId)
        {
            var promotion = await _context.Promotions.FindAsync(promotionId);
            var product = await _context.Products.FindAsync(productId);

            if (promotion == null || product == null)
            {
                return new ServiceResponse<bool>
                {
                    Data = false,
                    Message = "Promotion or Product not found",
                    Status = false
                };
            }

            promotion.Products.Add(product);

            await _context.SaveChangesAsync();

            return new ServiceResponse<bool>
            {
                Data = true,
                Message = "Product added to Promotion successfully",
                Status = true
            };
        }

        public async Task<ServiceResponse<bool>> RemoveProductFromPromotion(int promotionId, int productId)
        {
            var promotion = await _context.Promotions.FindAsync(promotionId);
            var product = await _context.Products.FindAsync(productId);

            if (promotion == null || product == null)
            {
                return new ServiceResponse<bool>
                {
                    Data = false,
                    Message = "Promotion or Product not found",
                    Status = false
                };
            }

            promotion.Products.Remove(product);

            await _context.SaveChangesAsync();

            return new ServiceResponse<bool>
            {
                Data = true,
                Message = "Product removed from Promotion successfully",
                Status = true
            };
        }

        public async Task<List<ProductResponseDTO>> GetProductsInPromotion(int promotionId)
        {
            var promotion = await _context.Promotions.Include(p => p.Products)
                                                     .FirstOrDefaultAsync(p => p.PromotionID == promotionId);

            if (promotion == null)
            {
                return null;
            }

            var products = promotion.Products.ToList();
            var responseDTOs = _mapper.Map<List<ProductResponseDTO>>(products);
            return responseDTOs;
        }

        public async Task<PromotionResponseDTO> GetPromotionById(int promotionId)
        {
            var promotion = await _context.Promotions.FindAsync(promotionId);

            if (promotion == null)
            {
                return null;
            }

            var responseDTO = _mapper.Map<PromotionResponseDTO>(promotion);
            return responseDTO;
        }
    }
}