using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using Talabat.APIs.DTOs;
using Talabat.Core.Entities;

namespace Talabat.APIs.Helpers
{
    public class ProductPictureUrlResolver : IValueResolver<Product, ProductDTO, string>
    {
        private readonly IConfiguration configuration;

        public ProductPictureUrlResolver(IConfiguration configuration) 
        {
            this.configuration=configuration;
        }

        public string Resolve(Product source, ProductDTO destination, string destMember, ResolutionContext context)
        {
            if (!source.PictureUrl.IsNullOrEmpty()) 
            {
                return $"{configuration["BaseApiUrl"]}/{source.PictureUrl}";
            }
            return null;
        }
    }
}
