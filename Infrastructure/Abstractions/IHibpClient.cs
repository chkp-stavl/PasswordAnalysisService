using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordAnalysisService.Infrastructure.Abstractions
{
    public interface IHibpClient
    {
        Task<string?> GetRangeAsync(string prefix, CancellationToken ct);
    }
}
