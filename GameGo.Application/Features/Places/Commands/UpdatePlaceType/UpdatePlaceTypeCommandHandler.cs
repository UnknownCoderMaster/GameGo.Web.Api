using GameGo.Application.Common.Models;
using GameGo.Application.Contracts.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace GameGo.Application.Features.Places.Commands.UpdatePlaceType;

public class UpdatePlaceTypeCommandHandler : IRequestHandler<UpdatePlaceTypeCommand, Result<bool>>
{
	private readonly IApplicationDbContext _context;

	public UpdatePlaceTypeCommandHandler(IApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<Result<bool>> Handle(UpdatePlaceTypeCommand request, CancellationToken cancellationToken)
	{
		var placeType = await _context.PlaceTypes
			.FirstOrDefaultAsync(pt => pt.Id == request.Id, cancellationToken);

		if (placeType == null)
			return Result<bool>.Failure("PlaceType not found");

		// Check slug uniqueness (excluding current)
		var slugExists = await _context.PlaceTypes
			.AnyAsync(pt => pt.Slug == request.Slug.ToLower() && pt.Id != request.Id, cancellationToken);

		if (slugExists)
			return Result<bool>.Failure("PlaceType with this slug already exists");

		placeType.Update(request.Name, request.Slug, request.Icon, request.Description);
		await _context.SaveChangesAsync(cancellationToken);

		return Result<bool>.Success(true);
	}
}
