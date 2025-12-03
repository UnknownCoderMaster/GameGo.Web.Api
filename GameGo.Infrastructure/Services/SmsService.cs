using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using GameGo.Application.Contracts.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace GameGo.Infrastructure.Services;

public class SmsService : ISmsService
{
	private readonly IConfiguration _configuration;
	private readonly HttpClient _httpClient;
	private readonly ILogger<SmsService> _logger;
	private string _token;
	private DateTime _tokenExpiry;

	public SmsService(
		IConfiguration configuration,
		HttpClient httpClient,
		ILogger<SmsService> logger)
	{
		_configuration = configuration;
		_httpClient = httpClient;
		_logger = logger;
	}

	public async Task SendSmsAsync(string phoneNumber, string message, CancellationToken cancellationToken = default)
	{
		try
		{
			await EnsureAuthenticatedAsync(cancellationToken);

			var smsSettings = _configuration.GetSection("Sms:Eskiz");
			var baseUrl = smsSettings["BaseUrl"];

			// MUHIM: Eskiz.uz API form-data qabul qiladi, JSON emas!
			var formData = new MultipartFormDataContent
			{
				{ new StringContent(phoneNumber), "mobile_phone" },
				{ new StringContent(message), "message" },
				{ new StringContent("4546"), "from" }
			};

			// Token header'ga qo'shish
			_httpClient.DefaultRequestHeaders.Clear();
			_httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_token}");

			_logger.LogInformation("Sending SMS to {PhoneNumber}", phoneNumber);

			var response = await _httpClient.PostAsync(
				$"{baseUrl}/message/sms/send",
				formData,
				cancellationToken);

			var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);

			if (!response.IsSuccessStatusCode)
			{
				_logger.LogError("SMS API Error: Status {StatusCode}, Response: {Response}",
					response.StatusCode, responseContent);

				// Agar token muammosi bo'lsa, qayta authenticate qilish
				if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
				{
					_token = null;
					_logger.LogWarning("Token expired, re-authenticating...");
					await EnsureAuthenticatedAsync(cancellationToken);

					// Qayta urinish
					_httpClient.DefaultRequestHeaders.Clear();
					_httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_token}");

					response = await _httpClient.PostAsync(
						$"{baseUrl}/message/sms/send",
						formData,
						cancellationToken);

					responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
				}
			}

			response.EnsureSuccessStatusCode();
			_logger.LogInformation("SMS sent successfully to {PhoneNumber}", phoneNumber);
		}
		catch (HttpRequestException ex)
		{
			_logger.LogError(ex, "Failed to send SMS to {PhoneNumber}", phoneNumber);

			// Production'da SMS xatosi registration'ni to'xtatmasligi kerak
			// Shuning uchun exception'ni yutamiz, faqat log qilamiz
			// throw; // Agar SMS majburiy bo'lsa, bu qatorni uncomment qiling
		}
	}

	public async Task SendVerificationCodeAsync(string phoneNumber, string code, CancellationToken cancellationToken = default)
	{
		//var message = $"GameGo tasdiqlash kodi: {code}. Kod 5 daqiqa amal qiladi.";
		var message = $"Kod: {code}. Bu Eskiz dan test";
		await SendSmsAsync(phoneNumber, message, cancellationToken);
	}

	private async Task EnsureAuthenticatedAsync(CancellationToken cancellationToken = default)
	{
		// Agar token mavjud va muddati tugamagan bo'lsa, qayta authenticate qilmaymiz
		if (!string.IsNullOrEmpty(_token) && _tokenExpiry > DateTime.UtcNow)
		{
			return;
		}

		var smsSettings = _configuration.GetSection("Sms:Eskiz");
		var baseUrl = smsSettings["BaseUrl"];
		var email = smsSettings["Email"];
		var password = smsSettings["Password"];

		_logger.LogInformation("Authenticating with Eskiz.uz API...");

		// Eskiz.uz login uchun form-data ishlatadi
		var formData = new MultipartFormDataContent
		{
			{ new StringContent(email), "email" },
			{ new StringContent(password), "password" }
		};

		try
		{
			var response = await _httpClient.PostAsync(
				$"{baseUrl}/auth/login",
				formData,
				cancellationToken);

			var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);

			if (!response.IsSuccessStatusCode)
			{
				_logger.LogError("Eskiz.uz authentication failed: {StatusCode}, Response: {Response}",
					response.StatusCode, responseContent);
				throw new Exception($"Failed to authenticate with Eskiz.uz: {responseContent}");
			}

			var result = JsonSerializer.Deserialize<EskizAuthResponse>(responseContent, new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = true
			});

			if (result == null || result.Data == null || string.IsNullOrEmpty(result.Data.Token))
			{
				_logger.LogError("Invalid authentication response from Eskiz.uz: {Response}", responseContent);
				throw new Exception("Invalid authentication response from Eskiz.uz");
			}

			_token = result.Data.Token;
			_tokenExpiry = DateTime.UtcNow.AddDays(29); // Eskiz token 30 kun amal qiladi

			_logger.LogInformation("Successfully authenticated with Eskiz.uz API");
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Error during Eskiz.uz authentication");
			throw;
		}
	}

	// Response models
	private class EskizAuthResponse
	{
		public string Message { get; set; }
		public EskizData Data { get; set; }
		public string TokenType { get; set; }
	}

	private class EskizData
	{
		public string Token { get; set; }
	}
}



//using System.Net.Http;
//using System.Text;
//using System.Text.Json;
//using System.Threading;
//using System.Threading.Tasks;
//using GameGo.Application.Contracts.Infrastructure;
//using Microsoft.Extensions.Configuration;

//namespace GameGo.Infrastructure.Services;

//public class SmsService : ISmsService
//{
//	private readonly IConfiguration _configuration;
//	private readonly HttpClient _httpClient;
//	private string _token;

//	public SmsService(IConfiguration configuration, HttpClient httpClient)
//	{
//		_configuration = configuration;
//		_httpClient = httpClient;
//	}

//	public async Task SendSmsAsync(string phoneNumber, string message, CancellationToken cancellationToken = default)
//	{
//		await EnsureAuthenticatedAsync(cancellationToken);

//		var smsSettings = _configuration.GetSection("Sms:Eskiz");
//		var baseUrl = smsSettings["BaseUrl"];

//		var payload = new
//		{
//			mobile_phone = phoneNumber,
//			message = message,
//			from = "4546"
//		};

//		var content = new StringContent(
//			JsonSerializer.Serialize(payload),
//			Encoding.UTF8,
//			"application/json");

//		_httpClient.DefaultRequestHeaders.Authorization =
//			new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token);

//		var response = await _httpClient.PostAsync($"{baseUrl}/message/sms/send", content, cancellationToken);
//		response.EnsureSuccessStatusCode();
//	}

//	public async Task SendVerificationCodeAsync(string phoneNumber, string code, CancellationToken cancellationToken = default)
//	{
//		var message = $"GameGo tasdiqlash kodi: {code}. Kod 5 daqiqa amal qiladi.";
//		await SendSmsAsync(phoneNumber, message, cancellationToken);
//	}

//	private async Task EnsureAuthenticatedAsync(CancellationToken cancellationToken = default)
//	{
//		if (!string.IsNullOrEmpty(_token))
//			return;

//		var smsSettings = _configuration.GetSection("Sms:Eskiz");
//		var baseUrl = smsSettings["BaseUrl"];

//		var payload = new
//		{
//			email = smsSettings["Email"],
//			password = smsSettings["Password"]
//		};

//		var content = new StringContent(
//			JsonSerializer.Serialize(payload),
//			Encoding.UTF8,
//			"application/json");

//		var response = await _httpClient.PostAsync($"{baseUrl}/auth/login", content, cancellationToken);
//		response.EnsureSuccessStatusCode();

//		var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
//		var result = JsonSerializer.Deserialize<EskizAuthResponse>(responseContent);

//		_token = result.data.token;
//	}

//	private class EskizAuthResponse
//	{
//		public EskizData data { get; set; }
//	}

//	private class EskizData
//	{
//		public string token { get; set; }
//	}
//}