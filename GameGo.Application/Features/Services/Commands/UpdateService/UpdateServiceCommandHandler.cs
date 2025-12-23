using GameGo.Application.Common.Models;
using GameGo.Application.Contracts.Identity;
using GameGo.Application.Contracts.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace GameGo.Application.Features.Services.Commands.UpdateService;

public class UpdateServiceCommandHandler : IRequestHandler<UpdateServiceCommand, Result>
{
	private readonly IApplicationDbContext _context;
	private readonly ICurrentUserService _currentUser;

	public UpdateServiceCommandHandler(
		IApplicationDbContext context,
		ICurrentUserService currentUser)
	{
		_context = context;
		_currentUser = currentUser;
	}

	public async Task<Result> Handle(UpdateServiceCommand request, CancellationToken cancellationToken)
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
			return Result.Failure("Only place owner can update services");

		// Update service
		service.Update(
			request.Name,
			request.Price,
			request.Description,
			request.DurationMinutes,
			request.Capacity);

		await _context.SaveChangesAsync(cancellationToken);

		return Result.Success();
	}
}
