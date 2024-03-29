using System.Threading.Tasks;
using AutoMapper;
using BackendApi.Core.Data.Repositories.Interfaces;
using BackendApi.Core.Data.Services.Interfaces;
using BackendApi.Core.Entities;
using BackendApi.Core.Models.Dtos;
using BackendApi.Core.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BackendApi.Controllers
{

    public class BasketController:BaseApiController
    {
        private readonly IBasketService _basketRepository;
        private readonly IMapper _mapper;

        public BasketController(
            IBasketService basketRepository,
            IMapper mapper)
        {
            this._mapper = mapper;
            this._basketRepository = basketRepository;
        } 

        [HttpGet]
        public async Task<ActionResult> GetBasketByIdAsync(string id){
            var basket = await _basketRepository.GetBasketAsync(id);
            return BaseApiOk(basket ?? new Basket(id));
        }


        [HttpPost("updateBasket")]
        public async Task<ActionResult> UpdateBasketAsync(Basket basket){
            var updateBasket = await _basketRepository.UpdateBasketAsync(basket);
            return BaseApiOk(updateBasket);
        }

        // [HttpPost("basketItemQuantity")]
        // public async Task<ActionResult> UpdateBasketItemQuantityAsync(UpdateBasketItemDto basketItemDto){
            
        //     var updateBasket = await _basketRepository.UpdateBasketItemQuantityAsync(basketItemDto.BasketId,basketItemDto.BasketItem);
        //     return BaseApiOk(updateBasket);
        // }

        [HttpDelete("{id}")]
        public async Task<ActionResult> RemoveBasketAsync(string id){
            await _basketRepository.RemoveBasketAsync(id);
            return BaseApiOk();
        }

        // [HttpDelete("removeBasketItem")]
        // public async Task<ActionResult>  RemoveBasketItemAsync(UpdateBasketItemDto basketItemDto){
        //     var basket = await _basketRepository.RemoveBasketItemAsync(basketItemDto.BasketId,basketItemDto.BasketItem);
        //     return BaseApiOk(basket);
        // }



    }
}