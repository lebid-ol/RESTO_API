using Microsoft.AspNetCore.Mvc;

namespace BankAccounts.API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class PingController : ControllerBase
    {
        [HttpGet]
        public IActionResult Ping()
        {
            var response = new
            {
                Version = "1.0.0",
                ServerTimeUtc = DateTime.UtcNow.ToString("O")
            };

            return Ok(response);
        }
    }
}
