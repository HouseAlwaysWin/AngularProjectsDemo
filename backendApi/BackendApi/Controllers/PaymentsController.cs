using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using BackendApi.Core.Models.Entities;
using BackendApi.Core.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Stripe;

namespace BackendApi.Controllers
{
    public class PaymentsController:BaseApiController
    {
        private readonly IPaymentService _paymentService;
        private readonly ILogger<IPaymentService> _logger;
        private readonly IConfiguration _config;
        private readonly string  _env;

        // private readonly string WhSecret = "whsec_DPWXxisCfoIq6aXoZrpU9SPhX7Etdk3z";
        public PaymentsController(
            IPaymentService paymentService,
            ILogger<IPaymentService> logger,
            IConfiguration config)
        {
            this._logger = logger;
            this._config = config;
            this._paymentService = paymentService;

             _env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
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

            var secretKey =(_env == "Development") ? _config["StripeSettings:WebhookSecretKey"] : Environment.GetEnvironmentVariable("StripeSettings:WebhookSecretKey");

            var stripeEvent = EventUtility.ConstructEvent(json,Request.Headers["Stripe-Signature"],
                secretKey
            );

            if(stripeEvent ==null){
                return BaseApiBadRequest("StripeEvent is null");
            }

            PaymentIntent intent;
            Core.Models.Entities.Order order;

            switch(stripeEvent.Type){
                case "payment_intent.succeeded":
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