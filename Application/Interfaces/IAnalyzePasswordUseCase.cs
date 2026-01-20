using Application.Requests;
using Application.Responses;

namespace Application.Interfaces
{
    public interface IAnalyzePasswordUseCase
    {
        Task<AnalyzePasswordResult> ExecuteAsync(
            AnalyzePasswordRequest request,
            CancellationToken ct = default);
    }
}
