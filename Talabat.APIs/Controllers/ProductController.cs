using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.DTOs;
using Talabat.APIs.Helpers;
using Talabat.Core.Entities;
using Talabat.Core.IRepositories;
using Talabat.Core.Specifications;

namespace Talabat.APIs.Controllers
{
    public class ProductController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductController(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [CachedAttribute(600)]
        [HttpGet]
        public async Task<ActionResult<Pagination<ProductDTO>>> GetProducts([FromQuery] ProductSpecificationParameters productSpecificationParameters)
        {
            var specifications = new ProductSpecifications(productSpecificationParameters);
            var products = await _unitOfWork.Repository<Product>().GetAllWithSpecificationAsync(specifications);
            var model = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductDTO>>(products);
            var specificationForCount = new ProductWithFillterForCountSpecification(productSpecificationParameters); 
            var count = await _unitOfWork.Repository<Product>().GetCountAsync(specificationForCount);
            return Ok(new Pagination<ProductDTO>(productSpecificationParameters.PageIndex , productSpecificationParameters.PageSize ,count , model));
        }

        [CachedAttribute(600)]
        [HttpGet("{id:int}")]
        //[Route("{id:int}")]
        public async Task<ActionResult<ProductDTO>> GetProductById(int id)
        {
            var specifications = new ProductSpecifications(id);
            var product = await _unitOfWork.Repository<Product>().GetByIdWithSpecificationAsync(specifications);
            var model = _mapper.Map<Product, ProductDTO>(product);
            return Ok(model);
        }

        [CachedAttribute(600)]
        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetBrands()
        {
            var brands = await _unitOfWork.Repository<ProductBrand>().GetAllAsync();
            return Ok(brands);
        }

        [CachedAttribute(600)]
        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetTypes()
        {
            var types = await _unitOfWork.Repository<ProductType>().GetAllAsync();
            return Ok(types);
        }
    }
}
