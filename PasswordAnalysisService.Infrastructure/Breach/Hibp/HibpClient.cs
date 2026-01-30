using Infrastructure.Breach;
using PasswordAnalysisService.Infrastructure.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordAnalysisService.Infrastructure.Breach.Hibp
{
    public sealed class HibpClient : IHibpClient
    {
        private readonly HttpClient httpClient;

        public HibpClient(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<string?> GetRangeAsync(string prefix, CancellationToken ct)
        {
            var response = await httpClient.GetAsync(
                $"https://api.pwnedpasswords.com/range/{prefix}", ct);

            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadAsStringAsync(ct);
        }
    }
}
