using PasswordAnalysisService.Models.Requests;
using FluentValidation;

namespace PasswordAnalysisService.Api.Validation
{
    public class AnalyzePasswordRequestValidator
     : AbstractValidator<AnalyzePasswordRequestDto>
    {
        public AnalyzePasswordRequestValidator()
        {
            RuleFor(x => x.Password)
            .NotEmpty()
            .Must(p => !string.IsNullOrWhiteSpace(p))
            .WithMessage("Password cannot be empty or whitespace");
        }
    }

}
