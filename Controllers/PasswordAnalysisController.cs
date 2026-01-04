using Microsoft.AspNetCore.Mvc;
using PasswordAnalysisService.Models.Requests;
using PasswordAnalysisService.Models.Responses;
using PasswordAnalysisService.Services;

namespace PasswordAnalysisService.Controllers
{
    [ApiController]
    [Route("password-analysis")]
    public class PasswordAnalysisController : ControllerBase
    {
        private readonly IPasswordAnalysisService iPasswordAnalysisService;

        public PasswordAnalysisController(IPasswordAnalysisService iPasswordAnalysisService)
        {
            this.iPasswordAnalysisService = iPasswordAnalysisService;
        }

        [HttpPost("analyze")]
        public IActionResult Analyze(PasswordAnalysisRequest request)
        {
            try
            {
                var result = iPasswordAnalysisService.Analyze(request.Password);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
    
        }
    }
}
