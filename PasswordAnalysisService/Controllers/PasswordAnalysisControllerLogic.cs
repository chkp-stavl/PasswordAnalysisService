using API.ControllerLogics;
using Microsoft.AspNetCore.Mvc;
using PasswordAnalysisService.Models.Requests;
using PasswordAnalysisService.Models.Responses;


namespace PasswordAnalysisService.Controllers
{
    [ApiController]
    [Route("api/password-analysis")]
    public sealed class PasswordAnalysisController : ControllerBase
    {
        private readonly IPasswordAnalysisControllerLogic _passwordAnalysisControllerLogic;

        public PasswordAnalysisController(
            IPasswordAnalysisControllerLogic logic)
        {
            _passwordAnalysisControllerLogic = logic;
        }

        [HttpPost("analyze")]
        public async Task<ActionResult<AnalyzePasswordResponseDto>> Analyze(
            [FromBody] AnalyzePasswordRequestDto request,
            CancellationToken ct)
        {
            try
            {
                var response = await _passwordAnalysisControllerLogic.Analyze(request, ct);

                if (!response.IsValid)
                {
                    return BadRequest(response);
                }

                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }
    }

}
