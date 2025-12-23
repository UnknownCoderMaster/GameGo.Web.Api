using GameGo.Application.Common.Models;
using GameGo.Application.Contracts.Identity;
using GameGo.Application.Contracts.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace GameGo.Application.Features.Services.Commands.DeleteService;

public class DeleteServiceCommandHandler : IRequestHandler<DeleteServiceCommand, Result>
{
	private readonly IApplicationDbContext _context;
	private readonly ICurrentUserService _currentUser;

	public DeleteServiceCommandHandler(
		IApplicationDbContext context,
		ICurrentUserService currentUser)
	{
		_context = context;
		_currentUser = currentUser;
	}

	public async Task<Result> Handle(DeleteServiceCommand request, CancellationToken cancellationToken)
	{
		if (!_currentUser.IsAuthenticated || !_currentUser.UserId.HasValue)
			return Result.Failure("User must be authenticated");

		// Find service with place
		var service = await _context.Services
			.Include(s => s.Place)
			.FirstOrDefaultAsync(s => s.Id == request.ServiceId, cancellationToken);

		if (service == null)
			return Result.Failure("Service not found");

		// Check ownership
		if (service.Place.OwnerId != _currentUser.UserId.Value)
			return Result.Failure("Only place owner can delete services");

		// Delete service
		_context.Services.Remove(service);
		await _context.SaveChangesAsync(cancellationToken);

		return Result.Success();
	}
}
