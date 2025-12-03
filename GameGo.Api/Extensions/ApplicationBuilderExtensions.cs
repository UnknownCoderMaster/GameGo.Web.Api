using GameGo.Api.Middleware;
using Microsoft.AspNetCore.Builder;

namespace GameGo.Api.Extensions;

public static class ApplicationBuilderExtensions
{
	public static IApplicationBuilder UseApiMiddleware(this IApplicationBuilder app)
	{
		app.UseMiddleware<ExceptionHandlingMiddleware>();
		app.UseMiddleware<RequestLoggingMiddleware>();

		return app;
	}
}