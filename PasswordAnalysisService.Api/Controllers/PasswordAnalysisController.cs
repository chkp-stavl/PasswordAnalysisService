using Core.Abstractions;
using Microsoft.AspNetCore.Mvc;
using PasswordAnalysisService.Mappers;
using PasswordAnalysisService.Models.Requests;
using PasswordAnalysisService.Models.Responses;


namespace PasswordAnalysisService.Controllers
{
    [ApiController]
    [Route("api/password-analysis")]
    public sealed class PasswordAnalysisController : ControllerBase
    {
        private readonly IMain _main;

        public PasswordAnalysisController(IMain passwordAnalysisService)
        {
            _main = passwordAnalysisService;
        }

        [HttpPost("analyze")]
        public async Task<ActionResult<AnalyzePasswordResponseDto>> Analyze(
            [FromBody] AnalyzePasswordRequestDto request,
            CancellationToken ct)
        {
            try
            {
                var riskResult = await _main
                .Analyze(request.Password, ct);
                var response = AnalyzePasswordResponseDtoMapper
                .From(riskResult);

                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }
    }

}
