using Microsoft.AspNetCore.Mvc;
using DotNetEmailClassifierApi.Services;
using System.Diagnostics;
using System.Threading.Tasks;
using DotNetEmailClassifierApi.Models;

namespace DotNetEmailClassifierApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmailClassificationController : ControllerBase
    {
        private readonly AiServiceClient _aiServiceClient;

        public EmailClassificationController(AiServiceClient aiServiceClient)
        {
            _aiServiceClient = aiServiceClient;
        }

        [HttpPost("classify")]
        public async Task<IActionResult> Classify([FromBody] EmailRequest request)
        {
            var stopwatch = Stopwatch.StartNew();

            var result = await _aiServiceClient.SendEmailForClassification(request);

            stopwatch.Stop();
            result.ElapsedTime = stopwatch.Elapsed.TotalSeconds;

            return Ok(result);
        }
    }
}