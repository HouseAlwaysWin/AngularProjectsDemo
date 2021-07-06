using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BackendApi.Core.Data.QuerySpecs;
using BackendApi.Core.Data.Repositories.Interfaces;
using BackendApi.Core.Entities;
using BackendApi.Core.Models.Dtos;
using BackendApi.Core.Models.Entities;
using BackendApi.Core.Services.Interfaces;
using BackendApi.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace BackendApi.Controllers
{
    public class ProductController : BaseApiController 
    {
        private readonly IStoreRepository _storeRepo;
        private readonly IProductService _productService;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _db;

        public ProductController(
            IStoreRepository storeRepo,
            IProductService productService,
            IMapper mapper,
            IDistributedCache db
        )
        {
            this._storeRepo = storeRepo;
            this._productService = productService;
            this._mapper = mapper;
            this._db = db;
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

            var product = await _productService.GetProductByIdAsync(id,true);
            return BaseApiOk(product);
        }


        [HttpGet("categoriestree")]
        public async Task<ActionResult> GetproductCategoriesTree(){

            var categories = await _productService.GetProductCategoriesTree(true);

            return BaseApiOk(categories);
        }

        [HttpPost("add")]
        public async Task<ActionResult> AddProduct(Product product){
            await _productService.AddProductAsync(product);
            return BaseApiOk();
        }

        [HttpPost("add-products")]
        public async Task<ActionResult> AddProducts(List<Product> products){
            await _productService.AddProductsAsync(products);
            return BaseApiOk();
        }


    }
}