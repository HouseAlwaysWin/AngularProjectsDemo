using System.Collections.Generic;
using System.Linq;
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
using Microsoft.EntityFrameworkCore;

namespace EcommerceApi.Controllers
{
    public class ProductController : BaseApiController 
    {
        private readonly IEntityRepository<Product> _entityRepository;
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public ProductController(
            IEntityRepository<Product> entityRepository,
            IProductService productService,
            IMapper mapper
        )
        {
            this._entityRepository = entityRepository;
            this._productService = productService;
            this._mapper = mapper;
        }

        [HttpGet("autocomplete")]
        public async Task<ActionResult> GetAutoComplete([FromQuery]ProductLikeParam param){

            var products = await _productService.GetProductsLikeAsync(param,true);
            return BaseApiOk(products);
        }

        [HttpGet("search")]
        public async Task<ActionResult> GetProductsSearch([FromQuery]ProductLikeParam param){

            var result = await _productService.GetProductsLikePagedAsync(param,true);

            return BaseApiOk(result.Data,result.TotalCount);
        }

        [HttpGet]
        public async Task<ActionResult> GetProducts([FromQuery]ProductListParam param){

            var result = await _productService.GetProductsPagedAsync(param,true);
            return BaseApiOk(result.Data,result.TotalCount);
        }

        [HttpGet("{id}")]

        public async Task<ActionResult> GetProduct(int id){

            var product = await _productService.GetProductByIdAsync(id);

            if(product == null) return BaseApiNotFound(new ApiResponse(false,""));

            var data = _mapper.Map<Product,ProductDto>(product);

            return BaseApiOk(data);
        }


        [HttpGet("categoriestree")]
        public async Task<ActionResult> GetproductCategoriesTree(){

            var categories = await _productService.GetProductCategoriesTree();

            return BaseApiOk(categories);
        }


    }
}