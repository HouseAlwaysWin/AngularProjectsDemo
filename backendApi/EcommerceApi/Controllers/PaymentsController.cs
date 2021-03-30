using System.IO;
using System.Threading.Tasks;
using EcommerceApi.Core.Models.Entities;
using EcommerceApi.Core.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Stripe;

namespace EcommerceApi.Controllers
{
    public class PaymentsController:BaseApiController
    {
        private readonly IPaymentService _paymentService;
        private readonly ILogger<IPaymentService> _logger;
        private readonly string WhSecret = "";
        public PaymentsController(IPaymentService paymentService,ILogger<IPaymentService> logger)
        {
            this._logger = logger;
            this._paymentService = paymentService;
        }

        [Authorize]
        [HttpPost("{basketId}")]
        public async Task<ActionResult<Basket>> CreateOrUpdatePaymentIntent(string basketId){
            var basket = await _paymentService.CreateOrUpdatePaymentIntent(basketId);
            if(basket ==null) return BaseApiBadRequest("Update PaymentIntent Failed");
            return BaseApiOk(basket);
        }


        [HttpPost("webhook")]
        public async Task<ActionResult> StripeWebhook(){
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            var stripeEvent = EventUtility.ConstructEvent(json,Request.Headers["Stripe-Signature"],WhSecret);

            PaymentIntent intent;
            Core.Models.Entities.Order order;

            switch(stripeEvent.Type){
                case "payment_intent.successed":
                    intent =(PaymentIntent) stripeEvent.Data.Object;
                    _logger.LogInformation("Payment Successed",intent.Id);
                    order = await _paymentService.UpdateOrderPaymentSuccessed(intent.Id);
                    _logger.LogInformation("Order Update Payment Received",order.Id);
                    break;
                case "payment_intent.payment_failed":
                    intent =(PaymentIntent) stripeEvent.Data.Object;
                    _logger.LogInformation("Payment Failed",intent.Id);
                    order = await _paymentService.UpdateOrderPaymentFailed(intent.Id);
                    _logger.LogInformation("Update Payment Failed",order.Id);
                    break;
            }

            return BaseApiOk(); 
        }





    }
}