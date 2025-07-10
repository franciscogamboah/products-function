namespace Application.Common.Infrastructure
{
    public interface IValidateTokenService
    {
        Task<bool> ValidateTokenWithRemoteAsync(string token);
    }
}
