using System.Threading;
using System.Threading.Tasks;

namespace GameGo.Application.Contracts.Infrastructure;

public interface ISmsService
{
	Task SendSmsAsync(string phoneNumber, string message, CancellationToken cancellationToken = default);
	Task SendVerificationCodeAsync(string phoneNumber, string code, CancellationToken cancellationToken = default);
}