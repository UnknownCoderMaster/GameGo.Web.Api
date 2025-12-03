using System.Threading;
using System.Threading.Tasks;

namespace GameGo.Application.Contracts.Infrastructure;

public interface IEmailService
{
	Task SendEmailAsync(string to, string subject, string body, CancellationToken cancellationToken = default);
	Task SendVerificationEmailAsync(string to, string verificationCode, CancellationToken cancellationToken = default);
}