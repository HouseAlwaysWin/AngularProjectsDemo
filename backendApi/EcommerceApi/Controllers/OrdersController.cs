using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using EcommerceApi.Core.Models.Dtos;
using EcommerceApi.Core.Models.Entities;
using EcommerceApi.Core.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace EcommerceApi.Controllers
{
    [Authorize]
    public class OrdersController : BaseApiController
    {
         private readonly IOrderService _orderService;
        private readonly IMapper _mapper;
 private readonly IStringLocalizer _localizer;

        public OrdersController(IOrderService orderService,
        IStringLocalizer localizer,
            IMapper mapper)
        {
            this._orderService = orderService;
            this._mapper = mapper;
            this._localizer = localizer;
        }

        [HttpPost("create")]
        public async Task<ActionResult<Order>> CreateOrder(CreateOrderParam orderDto){
            var email = HttpContext.User?.Claims?.FirstOrDefault(x => x.Type==ClaimTypes.Email)?.Value;
            var username = HttpContext.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;

            var  address = _mapper.Map<AddressDto,OrderAddress>(orderDto.ShipToAddress);

            var order = await _orderService.CreateOrderAsync(email,username,orderDto.DeliveryMethodId,orderDto.BasketId,address);

            if(order == null){
                return BaseApiBadRequest("Created Order Failed.");
            }
            return BaseApiOk(order);
        }

      

        [HttpGet("list")]
        public async Task<ActionResult> GetOrderByEmail([FromQuery]GetOrderParam param){
            if(string.IsNullOrEmpty(param.Email)){
                param.Email = HttpContext.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            }

            var order = await _orderService.GetOrderByEmailListSpec(param);

            if(order == null){
                return BaseApiNotFound("Order Not Found.");
            }
            var result =  _mapper.Map<IReadOnlyList<Order>,IReadOnlyList<OrderDto>>(order);
            return BaseApiOk(result);
        }

        [HttpGet("deliveryMethods")]
        public async Task<ActionResult<List<DeliveryMethodDto>>> GetDeliveryMethods(){
            var result = await _orderService.GetDeliveryMethodAsync();
            return BaseApiOk(result);
        } 

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDto>> GetOrderById(int id){
            var email = HttpContext.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            var order = await _orderService.GetOrderByIdAsync(id,email);

            if(order == null){
                return BaseApiNotFound("Order Not Found.");
            }
            var result =  _mapper.Map<Order,OrderDto>(order);
            return BaseApiOk(result);
        }
    }
}