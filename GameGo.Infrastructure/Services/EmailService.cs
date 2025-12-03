using System.Threading;
using System.Threading.Tasks;
using GameGo.Application.Contracts.Infrastructure;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace GameGo.Infrastructure.Services;

public class EmailService : IEmailService
{
	private readonly IConfiguration _configuration;

	public EmailService(IConfiguration configuration)
	{
		_configuration = configuration;
	}

	public async Task SendEmailAsync(string to, string subject, string body, CancellationToken cancellationToken = default)
	{
		var emailSettings = _configuration.GetSection("Email");

		var message = new MimeMessage();
		message.From.Add(new MailboxAddress("GameGo", emailSettings["From"]));
		message.To.Add(new MailboxAddress("", to));
		message.Subject = subject;

		message.Body = new TextPart("html")
		{
			Text = body
		};

		using var client = new SmtpClient();
		await client.ConnectAsync(
			emailSettings["SmtpServer"],
			int.Parse(emailSettings["SmtpPort"]),
			MailKit.Security.SecureSocketOptions.StartTls,
			cancellationToken);

		await client.AuthenticateAsync(
			emailSettings["SmtpUsername"],
			emailSettings["SmtpPassword"],
			cancellationToken);

		await client.SendAsync(message, cancellationToken);
		await client.DisconnectAsync(true, cancellationToken);
	}

	public async Task SendVerificationEmailAsync(string to, string verificationCode, CancellationToken cancellationToken = default)
	{
		var subject = "Verify your email - GameGo";
		var body = $@"
            <h2>Email Verification</h2>
            <p>Your verification code is: <strong>{verificationCode}</strong></p>
            <p>This code will expire in 5 minutes.</p>
        ";

		await SendEmailAsync(to, subject, body, cancellationToken);
	}
}