using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications
{
    public class ProductWithFillterForCountSpecification : BaseSpecification<Product>
    {
        public ProductWithFillterForCountSpecification(ProductSpecificationParameters productSpecificationParameters):
            base(P =>
                        (!productSpecificationParameters.BrandId.HasValue || P.ProductBrandId == productSpecificationParameters.BrandId.Value) &&
                        (!productSpecificationParameters.TypeId.HasValue || P.ProductTypeId == productSpecificationParameters.TypeId.Value) &&
                        (string.IsNullOrEmpty(productSpecificationParameters.Search) || P.Name.ToLower().Contains(productSpecificationParameters.Search))

            )
        {
            
        }
    }
}
