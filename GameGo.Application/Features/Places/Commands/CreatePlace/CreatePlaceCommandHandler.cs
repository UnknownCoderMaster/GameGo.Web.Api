using GameGo.Application.Common.Models;
using GameGo.Application.Contracts.Identity;
using GameGo.Application.Contracts.Persistence;
using GameGo.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GameGo.Application.Features.Places.Commands.CreatePlace;

public class CreatePlaceCommandHandler : IRequestHandler<CreatePlaceCommand, Result<long>>
{
	private readonly IApplicationDbContext _context;
	private readonly ICurrentUserService _currentUser;

	public CreatePlaceCommandHandler(
		IApplicationDbContext context,
		ICurrentUserService currentUser)
	{
		_context = context;
		_currentUser = currentUser;
	}

	public async Task<Result<long>> Handle(CreatePlaceCommand request, CancellationToken cancellationToken)
	{
		if (!_currentUser.IsAuthenticated || !_currentUser.UserId.HasValue)
			return Result<long>.Failure("User must be authenticated");

		// Check if place type exists
		var placeTypeExists = await _context.PlaceTypes
			.AnyAsync(pt => pt.Id == request.PlaceTypeId && pt.IsActive, cancellationToken);

		if (!placeTypeExists)
			return Result<long>.Failure("Place type not found or inactive");

		// Check if user is already a place owner
		var placeOwner = await _context.PlaceOwners
			.FirstOrDefaultAsync(po => po.UserId == _currentUser.UserId.Value, cancellationToken);

		if (placeOwner == null)
		{
			placeOwner = new PlaceOwner
			{
				UserId = _currentUser.UserId.Value,
				IsVerified = false
			};

			_context.PlaceOwners.Add(placeOwner);
			await _context.SaveChangesAsync(cancellationToken);
		}

		// Generate slug
		var slug = GenerateSlug(request.Name);

		// Create place
		var place = Place.Create(
			request.PlaceTypeId,
			_currentUser.UserId.Value,
			request.Name,
			slug,
			request.Address,
			request.Latitude,
			request.Longitude,
			request.PhoneNumber,
			request.Description);

		_context.Places.Add(place);
		await _context.SaveChangesAsync(cancellationToken);

		return Result<long>.Success(place.Id);
	}

	private string GenerateSlug(string name)
	{
		var slug = name.ToLower()
			.Replace(" ", "-")
			.Replace("'", "")
			.Replace("\"", "");

		return slug + "-" + Guid.NewGuid().ToString().Substring(0, 8);
	}
}