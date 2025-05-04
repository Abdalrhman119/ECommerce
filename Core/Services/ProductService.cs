using AutoMapper;
using Domain.Contracts;
using Domain.Models;
using Services.Specifications;
using ServicesAbstraction;
using Shared;
using Shared.DTO;
using Shared.DTO.Products;
using Shared.Enums;

namespace Services
{
    public class ProductService(IUnitOfWork _unitOfWork, IMapper _mapper) : IProductService
    {
        public async Task<IEnumerable<BrandResponse>> GetAllBrandsAsync()
        {
            var repository = _unitOfWork.GetRepositary<ProductBrand, int>();
            var brands = await repository.GetAllAsync();
            return _mapper.Map<IEnumerable<BrandResponse>>(brands);
        }

        public async Task<IEnumerable<TypeResponse>> GetAllTypesAsync()
        {
            var repository = _unitOfWork.GetRepositary<ProductType, int>();
            var types = await repository.GetAllAsync();
            return _mapper.Map<IEnumerable<TypeResponse>>(types);
        }

        public async Task<PaginatedResponse<ProductResponse>> GetAllProductsAsync(ProductQueryParameters productQueryParameters)
        {
            var specs = new ProductWithTypeAndBrandSpecifications(productQueryParameters);
            var products = await _unitOfWork.GetRepositary<Product, int>().GetAllAsync(specs);
            var productsResponse = _mapper.Map<IEnumerable<ProductResponse>>(products);
            var res = new PaginatedResponse<ProductResponse>()
            {
                Data = productsResponse,
                PageIndex = productQueryParameters.PageIndex,
                PageSize = productQueryParameters.PageSize,
                TotalCount = productsResponse.Count()
            };

            return res;
        }

        public async Task<ProductResponse> GetProductByIdAsync(int id)
        {
            var specs = new ProductWithTypeAndBrandSpecifications(id);
            var product = await _unitOfWork.GetRepositary<Product, int>().GetByIdAsync(specs);
            return _mapper.Map<ProductResponse>(product);
        }
    }
}
