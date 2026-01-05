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
        private readonly IPasswordAnalysisService passwordAnalysisService;

        public PasswordAnalysisController(IPasswordAnalysisService passwordAnalysisService)
        {
            this.passwordAnalysisService = passwordAnalysisService;
        }

        [HttpPost("analyze")]
        public IActionResult Analyze(PasswordAnalysisRequest request)
        {
            try
            {
                var result = passwordAnalysisService.Analyze(request.Password);
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
    
        }
    }
}
