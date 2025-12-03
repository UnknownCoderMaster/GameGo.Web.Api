using GameGo.Application.Contracts.Identity;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace GameGo.Infrastructure.Services;

public class CurrentUserService : ICurrentUserService
{
	private readonly IHttpContextAccessor _httpContextAccessor;

	public CurrentUserService(IHttpContextAccessor httpContextAccessor)
	{
		_httpContextAccessor = httpContextAccessor;
	}

	public long? UserId
	{
		get
		{
			var userIdClaim = _httpContextAccessor.HttpContext?.User?
				.FindFirstValue("userId");

			if (long.TryParse(userIdClaim, out long userId))
			{
				return userId;
			}

			return null;
		}
	}

	public string Email => _httpContextAccessor.HttpContext?.User?
		.FindFirstValue(ClaimTypes.Email) ?? string.Empty;

	public bool IsAuthenticated => _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
}