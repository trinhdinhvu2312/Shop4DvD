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
    public class ProductServiceImpl : IProductService
    {
        private readonly S4DContext _context;
        private readonly IMapper _mapper;

        public ProductServiceImpl(S4DContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PagingModel<ProductResponseDTO>> GetAllProducts(int page, int pageSize, string? productName, string? category)
        {
            var pagingModel = new PagingModel<ProductResponseDTO>();

            var query = _context.Products.Include(p => p.Category).AsQueryable();

            if (!string.IsNullOrEmpty(productName))
            {
                query = query.Where(p => p.ProductName.Contains(productName));
            }

            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(p => p.Category.CategoryName == category);
            }

            var totalCount = await query.CountAsync();

            var products = await query.Skip((page - 1) * pageSize).Take(pageSize)
                .ToListAsync();

            var responseDTOs = _mapper.Map<List<ProductResponseDTO>>(products);

            pagingModel.Data = responseDTOs;
            pagingModel.TotalCount = totalCount;
            pagingModel.TotalPage = (int)System.Math.Ceiling((double)totalCount / pageSize);

            return pagingModel;
        }

        public async Task<ServiceResponse<ProductResponseDTO>> AddItem(ProductRequestDTO newItem)
        {
            var category = await _context.Categories.FindAsync(newItem.CategoryID);

            if (category == null)
            {
                return new ServiceResponse<ProductResponseDTO>
                {
                    Data = null,
                    Message = "Category not found",
                    Status = false
                };
            }

            var product = _mapper.Map<Product>(newItem);
            product.Category = category;

            _context.Products.Add(product);
            
            await _context.SaveChangesAsync();

            var responseDTO = _mapper.Map<ProductResponseDTO>(product);

            return new ServiceResponse<ProductResponseDTO>
            {
                Data = responseDTO,
                Message = "Product added successfully",
                Status = true
            };
        }

        public async Task<ServiceResponse<ProductResponseDTO>> UpdateItem(ProductRequestDTO updatedItem)
        {
            var product = await _context.Products.FindAsync(updatedItem.ProductId);

            if (product == null)
            {
                return new ServiceResponse<ProductResponseDTO>
                {
                    Data = null,
                    Message = "Product not found",
                    Status = false
                };
            }

            var category = await _context.Categories.FindAsync(updatedItem.CategoryID);

            if (category == null)
            {
                return new ServiceResponse<ProductResponseDTO>
                {
                    Data = null,
                    Message = "Category not found",
                    Status = false
                };
            }

            _mapper.Map(updatedItem, product);
            product.Category = category;

            _context.Products.Update(product);
            await _context.SaveChangesAsync();

            var responseDTO = _mapper.Map<ProductResponseDTO>(product);

            return new ServiceResponse<ProductResponseDTO>
            {
                Data = responseDTO,
                Message = "Product updated successfully",
                Status = true
            };
        }

        public async Task<List<ProductResponseDTO>> GetListProduct()
        {
            var products = await _context.Products.ToListAsync();
            var responseDTOs = _mapper.Map<List<ProductResponseDTO>>(products);
            return responseDTOs;
        }

        public List<ProductResponseDTO> GetProductByCategoryId(int categoryId)
        {
            var products = _context.Products.Where(p => p.CategoryID == categoryId).ToList();
            var responseDTOs = _mapper.Map<List<ProductResponseDTO>>(products);
            return responseDTOs;
        }

        public async Task<List<ProductResponseDTO>> SearchProduct(int categoryId, string? search)
        {
            var query = _context.Products.Where(p => p.CategoryID == categoryId);

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(p => p.ProductName.Contains(search));
            }

            var products = await query.ToListAsync();
            var responseDTOs = _mapper.Map<List<ProductResponseDTO>>(products);
            return responseDTOs;
        }

        public ProductResponseDTO GetProductById(int id)
        {
            var product = _context.Products.Find(id);
            var responseDTO = _mapper.Map<ProductResponseDTO>(product);
            return responseDTO;
        }

        public async Task<ServiceResponse<bool>> DeleteProduct(int productId)
        {
            var product = await _context.Products.FindAsync(productId);

            if (product == null)
            {
                return new ServiceResponse<bool>
                {
                    Data = false,
                    Message = "Product not found",
                    Status = false
                };
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return new ServiceResponse<bool>
            {
                Data = true,
                Message = "Product deleted successfully",
                Status = true
            };
        }
    }
}