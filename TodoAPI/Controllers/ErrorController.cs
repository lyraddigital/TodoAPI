using Microsoft.AspNetCore.Mvc;

namespace TodoAPI.Controllers
{
    [Route("error")]
    [ApiController]
    public class ErrorController : Controller
    {
        [HttpGet]
        public string GetErrorString()
        {
            // TODO: This is used as a generic error controller
            // We should really return a smarter message than what
            // I'm returning here.
            return "Error";
        }
    }
}
