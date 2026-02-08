using GameGo.Application.Common.Models;
using GameGo.Application.Contracts.Persistence;
using GameGo.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace GameGo.Application.Features.Places.Commands.CreatePlaceType;

public class CreatePlaceTypeCommandHandler : IRequestHandler<CreatePlaceTypeCommand, Result<long>>
{
	private readonly IApplicationDbContext _context;

	public CreatePlaceTypeCommandHandler(IApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<Result<long>> Handle(CreatePlaceTypeCommand request, CancellationToken cancellationToken)
	{
		var slugExists = await _context.PlaceTypes
			.AnyAsync(pt => pt.Slug == request.Slug.ToLower(), cancellationToken);

		if (slugExists)
			return Result<long>.Failure("PlaceType with this slug already exists");

		var placeType = PlaceType.Create(request.Name, request.Slug, request.Icon, request.Description);

		_context.PlaceTypes.Add(placeType);
		await _context.SaveChangesAsync(cancellationToken);

		return Result<long>.Success(placeType.Id);
	}
}
