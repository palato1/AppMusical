using Microsoft.AspNetCore.Mvc;

namespace Seraphine.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LifeCheckController() : ControllerBase
    {
        [HttpHead]
        public IActionResult LifeChecker() => Ok();
    }
}
