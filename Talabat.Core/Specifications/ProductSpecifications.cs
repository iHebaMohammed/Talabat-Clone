using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications
{
    public class ProductSpecifications : BaseSpecification<Product>
    {
        // /this instructor is used for include => Get All Product
        public ProductSpecifications(ProductSpecificationParameters productSpecificationParameters) : 
            base(P => 
                        (!productSpecificationParameters.BrandId.HasValue || P.ProductBrandId == productSpecificationParameters.BrandId.Value) &&
                        (!productSpecificationParameters.TypeId.HasValue || P.ProductTypeId == productSpecificationParameters.TypeId.Value) &&
                        (string.IsNullOrEmpty(productSpecificationParameters.Search) || P.Name.ToLower().Contains(productSpecificationParameters.Search))
            )
        {
            Includes.Add(P => P.ProductBrand);
            Includes.Add(P => P.ProductType);

            if (!string.IsNullOrEmpty(productSpecificationParameters.Sort))
            {
                switch (productSpecificationParameters.Sort)
                {
                    case "priceAsc":
                        AddOrderBy(P => P.Price);
                        break;
                    case "priceDesc":
                        AddOrderByDescending(P => P.Price);
                        break;
                    case "nameAsc":
                        AddOrderBy(P => P.Name);
                        break;
                    case "nameDesc":
                        AddOrderByDescending(P => P.Name);
                        break;
                    default:
                        AddOrderBy(P => P.Name);
                        break;
                }
            }

            // Total = 100
            // PageSize = 10 , PageIndex = 2

            ApplyPagination(productSpecificationParameters.PageSize.Value * (productSpecificationParameters.PageIndex.Value - 1) , productSpecificationParameters.PageSize.Value);
        }
        //this instructor is use for criteria => where => Get spacific Product
        public ProductSpecifications(int id) : base(P => P.Id == id)
        {
            Includes.Add(P => P.ProductBrand);
            Includes.Add(P => P.ProductType);
        }
    }
}
