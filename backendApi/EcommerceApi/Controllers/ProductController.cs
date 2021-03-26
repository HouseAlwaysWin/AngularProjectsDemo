using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using EcommerceApi.Core.Data.QuerySpecs;
using EcommerceApi.Core.Data.Repositories.Interfaces;
using EcommerceApi.Core.Entities;
using EcommerceApi.Core.Models.Dtos;
using EcommerceApi.Core.Models.Entities;
using EcommerceApi.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceApi.Controllers
{
    public class ProductController : BaseApiController 
    {
        private readonly IGenericRepository<Product> _productRepo;
        private readonly IGenericRepository<ProductCategory> _productCategoryRepo;
        private readonly IGenericRepository<ProductBrand> _productBrand;
        private readonly IMapper _mapper;

        public ProductController(
            IGenericRepository<Product> productRepo,
            IGenericRepository<ProductCategory> productCategoryRepo,
            IGenericRepository<ProductBrand> productBrand,
            IMapper mapper
        )
        {
            this._productRepo = productRepo;
            this._productCategoryRepo = productCategoryRepo;
            this._productBrand = productBrand;
            this._mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult> GetProducts([FromQuery]GetProductParam param){
            var querySpec = new GetProductQuerySpec(param);

            var countSpec = new ProductCountQuerySpec(param);

            var totalItem = await _productRepo.CountAsync(countSpec);

            var products = await _productRepo.ListAsync(querySpec);

            var data = _mapper.Map<IReadOnlyList<Product>,IReadOnlyList<GetProductDto>>(products);

            return BaseApiOk(data,param.PageIndex, param.PageSize, totalItem);
        }

        [HttpGet("{id}")]

        public async Task<ActionResult> GetProduct(int id){
            var querySpec = new GetProductQuerySpec(id);
            var product = await _productRepo.GetEntityWithSpec(querySpec);
            if(product == null) return BaseApiNotFound(new ApiResponse(false,""));

            var data = _mapper.Map<Product,GetProductDto>(product);

            return BaseApiOk(data);
        }

        [HttpGet("categories")]
        public async Task<ActionResult> GetProductCategories(){
            var categories = await _productCategoryRepo.ListAllAsync();
            return BaseApiOk(categories);
        }


        [HttpGet("brands")]
        public async Task<ActionResult> GetProductBrands(){
            var brands = await _productBrand.ListAllAsync();
            return BaseApiOk(brands);
        }


    }
}