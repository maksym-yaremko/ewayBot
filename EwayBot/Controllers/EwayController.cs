using System.Threading.Tasks;
using EwayBot.BLL.EwayAPI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace EwayBot.Controllers
{
    [Route("api/v1/[controller]")]
    public class EwayController : Controller
    {
        private readonly EwayAPIClient _client;

        public EwayController(IOptions<EwayAPISettings> ewaySettings)
        {
            _client = new EwayAPIClient(ewaySettings);
        }

        [HttpGet("test")]
        public async Task<ActionResult> Test()
        {
            
            var lol = await _client.GetStopInfo(1428);
            return Ok(lol);
        }
    }
}
