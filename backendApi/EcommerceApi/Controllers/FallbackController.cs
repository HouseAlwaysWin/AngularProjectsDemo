using System.IO;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceApi.Controllers
{
    public class FallbackController:BaseApiController
    {

        public IActionResult Index(){
            return PhysicalFile(
                Path.Combine(Directory.GetCurrentDirectory(),
                "wwwroot","index.html"),
                "text/HTML");
        }
        
    }
}