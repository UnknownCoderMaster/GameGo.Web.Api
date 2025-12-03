using GameGo.Application.Contracts.Identity;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace GameGo.Application.Common.Behaviours;

public class LoggingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
	where TRequest : IRequest<TResponse>
{
	private readonly ILogger<LoggingBehaviour<TRequest, TResponse>> _logger;
	private readonly ICurrentUserService _currentUserService;

	public LoggingBehaviour(ILogger<LoggingBehaviour<TRequest, TResponse>> logger, ICurrentUserService currentUserService)
	{
		_logger = logger;
		_currentUserService = currentUserService;
	}

	public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
	{
		var requestName = typeof(TRequest).Name;
		var userId = _currentUserService.UserId;

		_logger.LogInformation("GameGo Request: {Name} {@UserId} {@Request}",
			requestName, userId, request);

		var response = await next();

		_logger.LogInformation("GameGo Response: {Name} {@Response}",
			requestName, response);

		return response;
	}
}