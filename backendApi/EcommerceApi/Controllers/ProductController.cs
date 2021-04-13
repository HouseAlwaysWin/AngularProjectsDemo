using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using EcommerceApi.Core.Data.QuerySpecs;
using EcommerceApi.Core.Data.Repositories.Interfaces;
using EcommerceApi.Core.Entities;
using EcommerceApi.Core.Models.Dtos;
using EcommerceApi.Core.Models.Entities;
using EcommerceApi.Core.Services.Interfaces;
using EcommerceApi.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceApi.Controllers
{
    public class ProductController : BaseApiController 
    {
        private readonly IGenericRepository<Product> _productRepoGeneric;
        private readonly IGenericRepository<ProductCategory> _productCategoryRepo;
        private readonly IGenericRepository<ProductBrand> _productBrand;
        private readonly IProductService _productService;
        private readonly IProductRepository _productRepo;
        private readonly IMapper _mapper;

        public ProductController(
            IGenericRepository<Product> productRepoGeneric,
            IGenericRepository<ProductCategory> productCategoryRepo,
            IGenericRepository<ProductBrand> productBrand,
            IProductService productService,
            IProductRepository productRepo,
            IMapper mapper
        )
        {
            this._productRepoGeneric = productRepoGeneric;
            this._productCategoryRepo = productCategoryRepo;
            this._productBrand = productBrand;
            this._productService = productService;
            this._productRepo = productRepo;
            this._mapper = mapper;
        }

        [HttpGet("autocomplete")]
        public async Task<ActionResult> GetAutoComplete([FromQuery]GetProductLikeParam param){

            var querySpec = new GetProductLikeQuery(param);

            var countSpec = new GetProductLikeQuery(param.Search);

            var totalItem = await _productRepoGeneric.CountAsync(countSpec);

            var products = await _productRepoGeneric.ListAsync(querySpec);

            var data =  _mapper.Map<IReadOnlyList<Product>,IReadOnlyList<ProductDto>>(products);
            return BaseApiOk(data, totalItem);
        }

        [HttpGet("search")]
        public async Task<ActionResult> GetProductsSearch([FromQuery]GetProductLikeParam param){

            var querySpec = new GetProductLikeQuery(param);

            var countSpec = new GetProductLikeQuery(param.Search);

            var totalItem = await _productRepoGeneric.CountAsync(countSpec);

            var products = await _productRepoGeneric.ListAsync(querySpec);

            var data =  _mapper.Map<IReadOnlyList<Product>,IReadOnlyList<ProductDto>>(products);
            return BaseApiOk(data, totalItem);
        }

        [HttpGet]
        public async Task<ActionResult> GetProducts([FromQuery]ProductListParam param){
            var querySpec = new GetProductQuerySpec(param);

            var countSpec = new ProductCountQuerySpec(param);

            var totalItem = await _productRepoGeneric.CountAsync(countSpec);

            // var products = await _productRepoGeneric.ListAsync(querySpec);
            // var products = await _productRepo.GetProductsAsync(param);
            // var data = _mapper.Map<IReadOnlyList<Product>,IReadOnlyList<ProductDto>>(products);
            var data = await _productService.GetProductDtosAsync(param);

            return BaseApiOk(data, totalItem);
        }

        [HttpGet("{id}")]

        public async Task<ActionResult> GetProduct(int id){
            var querySpec = new GetProductQuerySpec(id);
            var product = await _productRepoGeneric.GetEntityWithSpec(querySpec);
            if(product == null) return BaseApiNotFound(new ApiResponse(false,""));

            var data = _mapper.Map<Product,ProductDto>(product);

            return BaseApiOk(data);
        }

        [HttpGet("categories")]
        public async Task<ActionResult> GetProductCategories(){
            var categories = await _productCategoryRepo.ListAllAsync();
            return BaseApiOk(categories);
        }

        [HttpGet("categoriestree")]
        public async Task<ActionResult> GetproductCategoriesTree(){
            var categories = await _productService.GetProductCategoriesTree();
            return BaseApiOk(categories);
        }



        [HttpGet("brands")]
        public async Task<ActionResult> GetProductBrands(){
            var brands = await _productBrand.ListAllAsync();
            return BaseApiOk(brands);
        }


    }
}