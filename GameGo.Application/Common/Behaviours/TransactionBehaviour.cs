using GameGo.Application.Contracts.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GameGo.Application.Common.Behaviours;

public class TransactionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
	where TRequest : IRequest<TResponse>
{
	private readonly IApplicationDbContext _context;
	private readonly ILogger<TransactionBehaviour<TRequest, TResponse>> _logger;

	public TransactionBehaviour(IApplicationDbContext context, ILogger<TransactionBehaviour<TRequest, TResponse>> logger)
	{
		_context = context;
		_logger = logger;
	}

	public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
	{
		var requestName = typeof(TRequest).Name;

		// Faqat Command'lar uchun transaction
		if (requestName.EndsWith("Query"))
		{
			return await next();
		}

		try
		{
			var dbContext = _context as DbContext;
			await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);

			_logger.LogInformation("Begin transaction for {RequestName}", requestName);

			var response = await next();

			await transaction.CommitAsync(cancellationToken);

			_logger.LogInformation("Committed transaction for {RequestName}", requestName);

			return response;
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Error occurred during transaction for {RequestName}", requestName);
			throw;
		}
	}
}