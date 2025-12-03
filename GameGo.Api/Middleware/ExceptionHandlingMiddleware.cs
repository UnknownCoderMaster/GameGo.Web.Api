using GameGo.Application.Common.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace GameGo.Api.Middleware;

public class ExceptionHandlingMiddleware
{
	private readonly RequestDelegate _next;
	private readonly ILogger<ExceptionHandlingMiddleware> _logger;

	public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
	{
		_next = next;
		_logger = logger;
	}

	public async Task InvokeAsync(HttpContext context)
	{
		try
		{
			await _next(context);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "An error occurred: {Message}", ex.Message);
			await HandleExceptionAsync(context, ex);
		}
	}

	private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
	{
		var response = context.Response;
		response.ContentType = "application/json";

		var errorResponse = new ErrorResponse
		{
			Message = exception.Message,
			StatusCode = (int)HttpStatusCode.InternalServerError
		};

		switch (exception)
		{
			case ValidationException validationEx:
				response.StatusCode = (int)HttpStatusCode.BadRequest;
				errorResponse.StatusCode = (int)HttpStatusCode.BadRequest;
				errorResponse.Errors = validationEx.Errors;
				break;

			case NotFoundException:
				response.StatusCode = (int)HttpStatusCode.NotFound;
				errorResponse.StatusCode = (int)HttpStatusCode.NotFound;
				break;

			case UnauthorizedException:
				response.StatusCode = (int)HttpStatusCode.Unauthorized;
				errorResponse.StatusCode = (int)HttpStatusCode.Unauthorized;
				break;

			case ForbiddenException:
				response.StatusCode = (int)HttpStatusCode.Forbidden;
				errorResponse.StatusCode = (int)HttpStatusCode.Forbidden;
				break;

			case BusinessRuleException:
				response.StatusCode = (int)HttpStatusCode.BadRequest;
				errorResponse.StatusCode = (int)HttpStatusCode.BadRequest;
				break;

			default:
				response.StatusCode = (int)HttpStatusCode.InternalServerError;
				errorResponse.Message = "An error occurred while processing your request.";
				break;
		}

		var result = JsonSerializer.Serialize(errorResponse);
		await response.WriteAsync(result);
	}
}

public class ErrorResponse
{
	public int StatusCode { get; set; }
	public string Message { get; set; }
	public List<string> Errors { get; set; } = new List<string>();
}