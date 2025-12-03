using GameGo.Application.Common.Models;
using GameGo.Application.Contracts.Identity;
using GameGo.Application.Contracts.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GameGo.Application.Features.Places.Commands.SetPrimaryImage;

public class SetPrimaryImageCommandHandler : IRequestHandler<SetPrimaryImageCommand, Result<bool>>
{
	private readonly IApplicationDbContext _context;
	private readonly ICurrentUserService _currentUser;

	public SetPrimaryImageCommandHandler(
		IApplicationDbContext context,
		ICurrentUserService currentUser)
	{
		_context = context;
		_currentUser = currentUser;
	}

	public async Task<Result<bool>> Handle(SetPrimaryImageCommand request, CancellationToken cancellationToken)
	{
		var place = await _context.Places
			.Include(p => p.PlaceImages)
			.FirstOrDefaultAsync(p => p.Id == request.PlaceId, cancellationToken);

		if (place == null)
			return Result<bool>.Failure("Joy topilmadi");

		if (place.OwnerId != _currentUser.UserId)
			return Result<bool>.Failure("Ruxsat yo'q");

		var image = place.PlaceImages.FirstOrDefault(i => i.Id == request.ImageId);
		if (image == null)
			return Result<bool>.Failure("Rasm topilmadi");

		// Set all images to not primary
		foreach (var img in place.PlaceImages)
		{
			img.IsPrimary = false;
		}

		// Set the selected image as primary
		image.IsPrimary = true;

		await _context.SaveChangesAsync(cancellationToken);

		return Result<bool>.Success(true);
	}
}