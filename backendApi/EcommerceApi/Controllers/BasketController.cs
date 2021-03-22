using System.Threading.Tasks;
using EcommerceApi.Core.Data.Repositories.Interfaces;
using EcommerceApi.Core.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceApi.Controllers
{
    public class BasketController:BaseApiController
    {
        private readonly IBasketRepository _basketRepository;

        public BasketController(IBasketRepository basketRepository)
        {
            this._basketRepository = basketRepository;
        } 

        [HttpGet]
        public async Task<ActionResult> GetBasketByIdAsync(string id){
            var basket = await _basketRepository.GetBasketAsync(id);
            return Ok(basket ?? new Basket(id));
        }

        [HttpPost]
        public async Task<ActionResult> UpdateBasketAsync(Basket basket){
            var updateBasket = await _basketRepository.UpdateBasketAsync(basket);
            return Ok(updateBasket);
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteBasketAsync(string id){
            await _basketRepository.DeleteBasketAsync(id);
            return Ok();
        }



    }
}