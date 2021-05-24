// using System.Threading.Tasks;
// using AutoMapper;
// using BackendApi.Core.Services.Interfaces;
// using Microsoft.AspNetCore.Mvc;

// namespace BackendApi.Controllers
// {
//     public class MessagesController:BaseApiController
//     {
//         private readonly IMapper _mapper;
//         private readonly IMessageService _messageService;

//         public MessagesController(
//             IMessageService messageService,
//             IMapper mapper)
//         {
//             this._messageService = messageService;
//             this._mapper = mapper;
//         }



//         public async Task<ActionResult> GetMessagesForUser(){
//             return Ok();
//         }
        
//     }
// }