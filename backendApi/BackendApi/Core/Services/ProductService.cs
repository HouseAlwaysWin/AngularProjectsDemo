using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BackendApi.Core.Data.QuerySpecs;
using BackendApi.Core.Data.Repositories.Interfaces;
using BackendApi.Core.Models.Dtos;
using BackendApi.Core.Models.Entities;
using BackendApi.Core.Services.Interfaces;
using BackendApi.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace BackendApi.Core.Services
{
    public class ProductService : IProductService
    {
        private readonly IStoreRepository _storeRepo;
        private readonly ICachedService _cachedService;
        private readonly ILocalizedService _localizedService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        private string _productAllQueryKey { get { return $"{_cachedService.GetCurrentLang()}_all__productAllQuery_key"; } }
        private string _productKey { get { return $"{_cachedService.GetCurrentLang()}_all_products_key"; } }
        private string _productAttrKey { get { return $"{_cachedService.GetCurrentLang()}_all_productAttr_key"; } }
        private string _productAttrValueKey { get { return $"{_cachedService.GetCurrentLang()}_all_productAttrValue_key"; } }
        private string _productAttrMapKey { get { return $"{_cachedService.GetCurrentLang()}_all_productAttrMap_key"; } }
        private string _categoryKey { get { return $"{_cachedService.GetCurrentLang()}_all_categories_key"; } }
        private string _pictureKey { get { return $"{_cachedService.GetCurrentLang()}_all_pictures_key"; } }
        private string _pictureMapKey { get { return $"{_cachedService.GetCurrentLang()}_all_pictureMap_key"; } }

        public ProductService(
            IStoreRepository storeRepo,
            ICachedService cachedService,
            ILocalizedService localizedService,
            ILogger<ProductService> logger,
            IMapper mapper)
        {
            this._localizedService = localizedService;
            this._mapper = mapper;
            this._logger = logger;
            this._storeRepo = storeRepo;
            this._cachedService = cachedService;
        }

        private async Task SetProductsTranslation(List<Product> data)
        {
            try
            {
                foreach (var item in data)
                {
                    item.Name = await _localizedService.GetLocalizedAsync(item, p => p.Name);
                    item.Description = await _localizedService.GetLocalizedAsync(item, p => p.Description);
                    foreach (var attr in item.ProductAttributeMap.Select(a => a.ProductAttribute))
                    {
                        attr.Name = await _localizedService.GetLocalizedAsync(attr, p => p.Name);
                        foreach (var attrValue in attr.ProductAttributeValue)
                        {
                            attrValue.Name = await _localizedService.GetLocalizedAsync(attrValue, p => p.Name);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
            }
        }

        private IEnumerable<ProductDto> GetFilterQuery(IEnumerable<ProductDto> cachedData, ProductLikeParam param)
        {
            IEnumerable<ProductDto> query = cachedData.Where(p =>
               (!param.CategoryId.HasValue || p.ProductCategory.Id == param.CategoryId) &&
               (string.IsNullOrEmpty(param.Search) || p.Name.StartsWith(param.Search)));

            switch (param.Sort)
            {
                case "priceAsc":
                    query = query.OrderBy(p => p.Price);
                    break;
                case "priceDesc":
                    query = query.OrderByDescending(p => p.Price);
                    break;
                default:
                    query = query.OrderBy(p => p.Name);
                    break;
            }

            return query;
        }


        private async Task<List<ProductDto>> GetProductsDtoCachedAsync()
        {
            return await _cachedService.GetAndSetAsync<List<ProductDto>>(_productKey, async () =>
           {
               var data = await _storeRepo.GetAllAsync<Product>();
               foreach (var item in data)
               {
                   item.Name = await _localizedService.GetLocalizedAsync(item, p => p.Name);
                   item.Description = await _localizedService.GetLocalizedAsync(item, p => p.Description);
               }
               var dtos = _mapper.Map<List<Product>, List<ProductDto>>(data.ToList());
               return dtos;
           });
        }

        private async Task<List<ProductAttributeDto>> GetProductAttributeDtosCachedAsync()
        {
            return await _cachedService.GetAndSetAsync<List<ProductAttributeDto>>(_productAttrKey, async () =>
             {
                 var data = await _storeRepo.GetAllAsync<ProductAttribute>();
                 foreach (var item in data)
                 {
                     item.Name = await _localizedService.GetLocalizedAsync(item, p => p.Name);
                 }
                 var dtos = _mapper.Map<List<ProductAttribute>, List<ProductAttributeDto>>(data.ToList());
                 return dtos;
             });
        }

        private async Task<List<ProductAttributeValueDto>> GetProductAttributeValueDtosCachedAsync()
        {
            return await _cachedService.GetAndSetAsync<List<ProductAttributeValueDto>>(_productAttrValueKey, async () =>
           {
               var data = await _storeRepo.GetAllAsync<ProductAttributeValue>();
               foreach (var item in data)
               {
                   item.Name = await _localizedService.GetLocalizedAsync(item, p => p.Name);
               }
               var dtos = _mapper.Map<List<ProductAttributeValue>, List<ProductAttributeValueDto>>(data.ToList());
               return dtos;
           });
        }

        private async Task<List<Product_ProductAttributeDto>> GetProductAttributeMapDtosCachedAsync()
        {
            return await _cachedService.GetAndSetAsync<List<Product_ProductAttributeDto>>(_productAttrMapKey, async () =>
            {
                var data = await _storeRepo.GetAllAsync<Product_ProductAttribute>();
                var dtos = _mapper.Map<List<Product_ProductAttribute>, List<Product_ProductAttributeDto>>(data.ToList());
                return dtos;
            });
        }

        private async Task<List<ProductCategoryDto>> GetProductCategoryDtosCachedAsync()
        {
            return await _cachedService.GetAndSetAsync<List<ProductCategoryDto>>(_categoryKey, async () =>
            {
                var data = await _storeRepo.GetAllAsync<ProductCategory>();
                foreach (var item in data)
                {
                    item.Name = await _localizedService.GetLocalizedAsync(item, c => c.Name);
                }
                var dtos = _mapper.Map<List<ProductCategory>, List<ProductCategoryDto>>(data.OrderBy(d => d.SeqNo).ToList());
                return dtos;
            });
        }

        private async Task<List<PictureDto>> GetPictureDtosCachedAsync()
        {
            return await _cachedService.GetAndSetAsync<List<PictureDto>>(_pictureKey, async () =>
            {
                var data = await _storeRepo.GetAllAsync<Picture>();
                var dtos = _mapper.Map<List<Picture>, List<PictureDto>>(data.ToList());
                return dtos;
            });
        }

        private async Task<List<Product_PictureDto>> GetProductPictureDtosMapCachedAsync()
        {
            return await _cachedService.GetAndSetAsync<List<Product_PictureDto>>(_pictureMapKey, async () =>
            {
                var data = await _storeRepo.GetAllAsync<Product_Picture>();
                var dtos = _mapper.Map<List<Product_Picture>, List<Product_PictureDto>>(data.ToList());
                return dtos;
            });
        }

        private async Task<List<ProductDto>> GetAllProductRelatedCachedDataAsync()
        {
            try{
            var productQuerys = await _cachedService.GetAndSetAsync<List<ProductDto>>(_productAllQueryKey, async () =>
            {
                var products = await GetProductsDtoCachedAsync();
                var productAttrs = await GetProductAttributeDtosCachedAsync();
                var productAttrValues = await GetProductAttributeValueDtosCachedAsync();
                var productAttrMap = await GetProductAttributeMapDtosCachedAsync();
                var productCategories = await GetProductCategoryDtosCachedAsync();
                var productPictures = await GetPictureDtosCachedAsync();
                var productPicturesMap = await GetProductPictureDtosMapCachedAsync();

                var query = products
                    .GroupJoin(
                        productAttrMap,
                        p => p.Id,
                        m => m.ProductId,
                        (p, m) => new ProductDto
                        {
                            Id = p.Id,
                            Name = p.Name,
                            Price = p.Price,
                            Description = p.Description,
                            Published = p.Published,
                            Deleted = p.Deleted,
                            ProductAttributes = m.Where(m => m.ProductId == p.Id)
                                    .Join(
                                        productAttrs,
                                        m => m.ProductAttributeId,
                                        a => a.Id,
                                        (m, a) => new ProductAttributeDto
                                        {
                                            Id = a.Id,
                                            Name = a.Name,
                                            ProductAttributeValue = productAttrValues.Where(
                                               v => v.ProductAttributeId == a.Id).ToList()
                                        }).ToList(),
                            ProductCategory = productCategories.FirstOrDefault(c => c.Id == p.ProductCategoryId),
                            ProductPictures = productPicturesMap.Where(m => m.PictureId == p.Id)
                                .Join(
                                    productPictures,
                                    m => m.PictureId,
                                    pc => pc.Id,
                                    (m, pc) => pc).ToList()

                        }).ToList();

                return query;

            });

            return productQuerys;
            }catch(Exception ex){
                _logger.LogError(null,ex,ex.Message);
            } 
            return null;
        }

        public async Task<PagedList<ProductDto>> GetProductsPagedAsync(ProductListParam param, bool useCached = false)
        {

            PagedList<ProductDto> productsDto = null;
            ProductLikeParam paramTemp = null;
            if (useCached)
            {
                var cachedData = await GetAllProductRelatedCachedDataAsync();
                if(cachedData!=null){
                    paramTemp = _mapper.Map<ProductListParam, ProductLikeParam>(param);
                    var query = GetFilterQuery(cachedData, paramTemp);
                    var totalCount = query.Count();
                    var pagedData = query.Skip(param.PageIndex * param.PageSize).Take(param.PageSize).ToList(); ;
                    productsDto = new PagedList<ProductDto>(pagedData, totalCount);
                    return productsDto;
                }
            }

            paramTemp = _mapper.Map<ProductListParam, ProductLikeParam>(param);
            var result = await _storeRepo.GetAllPagedAsync<Product>(
            param.PageIndex, param.PageSize, query => ProductSpec.GetProducts(query, paramTemp));
            await SetProductsTranslation(result.Data);
            productsDto = _mapper.Map<PagedList<Product>, PagedList<ProductDto>>(result);

            return productsDto;
        }

        public async Task<List<ProductDto>> GetProductsLikeAsync(ProductLikeParam param, bool useCached = false)
        {
            List<ProductDto> productDtos = new List<ProductDto>();
            if (useCached)
            {
                var cachedData = await GetAllProductRelatedCachedDataAsync();
                var query = GetFilterQuery(cachedData, param);
                productDtos = query.ToList();
                return productDtos;
            }

            var products = await _storeRepo.GetAllAsync<Product>(query => ProductSpec.GetProductLikeQuery(query, param));
            foreach (var item in products)
            {
                item.Name = await _localizedService.GetLocalizedAsync(item, p => p.Name);
                item.Description = await _localizedService.GetLocalizedAsync(item, p => p.Description);
            }
            productDtos = _mapper.Map<List<Product>, List<ProductDto>>(products.ToList());

            return productDtos;
        }

        public async Task<PagedList<ProductDto>> GetProductsLikePagedAsync(ProductLikeParam param, bool useCached = false)
        {

            PagedList<ProductDto> productDtos = null;
            if (useCached)
            {
                var cachedData = await GetAllProductRelatedCachedDataAsync();
                var query = GetFilterQuery(cachedData, param);
                var total = query.Count();
                var pagedData = query.Skip(param.PageIndex * param.PageSize).Take(param.PageSize).ToList(); ;
                productDtos = new PagedList<ProductDto>(pagedData, total);
                return productDtos;
            }
            var result = await _storeRepo.GetAllPagedAsync<Product>(
                param.PageIndex, param.PageSize, query => ProductSpec.GetProductLikeQuery(query, param));
            await SetProductsTranslation(result.Data);
            productDtos = _mapper.Map<PagedList<Product>, PagedList<ProductDto>>(result);

            return productDtos;
        }

        public async Task<ProductDto> GetProductByIdAsync(int id, bool useCached = false)
        {
            ProductDto productDto = new ProductDto();
            if (useCached)
            {
                var cachedData = await GetAllProductRelatedCachedDataAsync();
                productDto = cachedData.FirstOrDefault(p => p.Id == id);
                return productDto;
            }
            var product = await _storeRepo.GetByAsync<Product>(q => ProductSpec.GetProductAll(q));
            productDto = _mapper.Map<Product, ProductDto>(product);
            return productDto;
        }

        public async Task AddProductAsync(Product product){
            try{
                await _storeRepo.AddAsync(product);
            } catch(Exception ex) {
                System.Console.WriteLine(ex);
            }
        }

        public async Task AddProductsAsync(IEnumerable<Product> products){
            try{
            await _storeRepo.BulkAddAsync(products);
            } catch(Exception ex) {
                System.Console.WriteLine(ex);
            }
        }


        public async Task<List<ProductCategoryDto>> GetProductCategoriesTree(bool useCached = false)
        {
            List<ProductCategoryDto> categories = new List<ProductCategoryDto>();
            if (useCached)
            {
                categories = await GetProductCategoryDtosCachedAsync();
            }
            else
            {
                var roots = await _storeRepo.GetAllAsync<ProductCategory>(query => query.Where(p => !p.ParentId.HasValue));

                var data = await _storeRepo.GetAllAsync<ProductCategory>();
                foreach (var item in data)
                {
                    item.Name = await _localizedService.GetLocalizedAsync(item, c => c.Name);
                }
                categories = _mapper.Map<List<ProductCategory>, List<ProductCategoryDto>>(data.ToList());
            }

            List<ProductCategoryDto> tree = categories.GenerateITree<ProductCategoryDto, int>(c => c.Id, c => c.ParentId.Value).ToList();

            return tree;
        }


    }
}