using GameGo.Application.Common.Models;
using GameGo.Application.Contracts.Identity;
using GameGo.Application.Contracts.Persistence;
using GameGo.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace GameGo.Application.Features.Services.Commands.CreateService;

public class CreateServiceCommandHandler : IRequestHandler<CreateServiceCommand, Result<long>>
{
	private readonly IApplicationDbContext _context;
	private readonly ICurrentUserService _currentUser;

	public CreateServiceCommandHandler(
		IApplicationDbContext context,
		ICurrentUserService currentUser)
	{
		_context = context;
		_currentUser = currentUser;
	}

	public async Task<Result<long>> Handle(CreateServiceCommand request, CancellationToken cancellationToken)
	{
		if (!_currentUser.IsAuthenticated || !_currentUser.UserId.HasValue)
			return Result<long>.Failure("User must be authenticated");

		// Check if place exists and user is owner
		var place = await _context.Places
			.FirstOrDefaultAsync(p => p.Id == request.PlaceId, cancellationToken);

		if (place == null)
			return Result<long>.Failure("Place not found");

		if (place.OwnerId != _currentUser.UserId.Value)
			return Result<long>.Failure("Only place owner can create services");

		// Create service
		var service = Service.Create(
			request.PlaceId,
			request.Name,
			request.Price,
			request.Description,
			request.Currency,
			request.DurationMinutes,
			request.Capacity);

		_context.Services.Add(service);
		await _context.SaveChangesAsync(cancellationToken);

		return Result<long>.Success(service.Id);
	}
}
