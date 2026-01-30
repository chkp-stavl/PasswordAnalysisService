using Domain.Enums;
using System.Collections.Immutable;

namespace Domain.Models
{
    public record StrengthResult
    (
        int Score,
        PasswordStrengthLevel Level,
        List<string> Issues
    );
}
