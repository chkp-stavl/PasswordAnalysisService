using System.ComponentModel.DataAnnotations;

namespace PasswordAnalysisService.Models.Requests
{
    public class AnalyzePasswordRequestDto
    {
        [Required]
        public string Password { get; set; } = null!;
    }
}
