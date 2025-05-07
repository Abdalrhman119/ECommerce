using Domain.Models.Products;
using Shared.DTO;
using Shared.Enums;
using System.Linq.Expressions;

namespace Services.Specifications
{
    public class ProductWithTypeAndBrandSpecifications : BaseSpecifications<Product>
    {
        //To Get Product By Id
        public ProductWithTypeAndBrandSpecifications(int id) : base(prod => prod.Id == id)
        {
            AddInclude(prod => prod.ProductType);
            AddInclude(prod => prod.ProductBrand);
        }



        //To Get All Products
        public ProductWithTypeAndBrandSpecifications(ProductQueryParameters productQueryParameters) : base(CreateCriteria(productQueryParameters))
        {
            AddInclude(prod => prod.ProductType);
            AddInclude(prod => prod.ProductBrand);
            ApplySorting(productQueryParameters);
            ApplyPagination(productQueryParameters.PageSize, productQueryParameters.PageIndex);
        }

        private static Expression<Func<Product, bool>> CreateCriteria(ProductQueryParameters productQueryParameters)
        {
            return Product =>
        (!productQueryParameters.BrandId.HasValue || Product.BrandId == productQueryParameters.BrandId.Value) &&
        (!productQueryParameters.TypeId.HasValue || Product.TypeId == productQueryParameters.TypeId.Value) &&
          (string.IsNullOrWhiteSpace(productQueryParameters.Search) ||
          Product.Name.ToLower().Contains(productQueryParameters.Search.ToLower()));


        }

        private void ApplySorting(ProductQueryParameters productQueryParameters)
        {
            switch (productQueryParameters.ProductSortingOptions)
            {
                case ProductSortingOptions.NameAsc:
                    AddOrderBy(p => p.Name);
                    break;
                case ProductSortingOptions.NameDesc:
                    AddOrderByDescending(p => p.Name);
                    break;
                case ProductSortingOptions.PriceAsc:
                    AddOrderBy(p => p.Price);
                    break;
                case ProductSortingOptions.PriceDesc:
                    AddOrderByDescending(p => p.Price);
                    break;
            }
        }




    }
}
