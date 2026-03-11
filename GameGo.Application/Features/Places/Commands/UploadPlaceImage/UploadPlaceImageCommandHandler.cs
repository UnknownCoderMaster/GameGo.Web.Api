using GameGo.Application.Common.Models;
using GameGo.Application.Contracts.Identity;
using GameGo.Application.Contracts.Infrastructure;
using GameGo.Application.Contracts.Persistence;
using GameGo.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GameGo.Application.Features.Places.Commands.UploadPlaceImage;

public class UploadPlaceImageCommandHandler : IRequestHandler<UploadPlaceImageCommand, Result<long>>
{
	private readonly IApplicationDbContext _context;
	private readonly IFileService _fileService;
	private readonly ICurrentUserService _currentUser;

	public UploadPlaceImageCommandHandler(
		IApplicationDbContext context,
		IFileService fileService,
		ICurrentUserService currentUser)
	{
		_context = context;
		_fileService = fileService;
		_currentUser = currentUser;
	}

	public async Task<Result<long>> Handle(UploadPlaceImageCommand request, CancellationToken cancellationToken)
	{
		var place = await _context.Places
			.Include(p => p.PlaceImages)
			.FirstOrDefaultAsync(p => p.Id == request.PlaceId, cancellationToken);

		if (place == null)
			return Result<long>.Failure("Joy topilmadi");

		if (place.OwnerId != _currentUser.UserId)
			return Result<long>.Failure("Siz bu joyga rasm qo'sha olmaysiz");

		var imageUrl = await _fileService.UploadFileAsync(
			request.ImageStream,
			request.FileName,
			"places",
			cancellationToken);

		if (request.IsPrimary)
		{
			foreach (var image in place.PlaceImages.Where(i => i.IsPrimary))
			{
				image.IsPrimary = false;
				image.UpdatedAt = DateTime.UtcNow;
			}
		}

		var placeImage = new PlaceImage
		{
			PlaceId = request.PlaceId,
			ImageUrl = imageUrl,
			IsPrimary = request.IsPrimary,
			DisplayOrder = request.DisplayOrder,
			CreatedAt = DateTime.UtcNow,
			UpdatedAt = DateTime.UtcNow
		};

		_context.PlaceImages.Add(placeImage);
		await _context.SaveChangesAsync(cancellationToken);

		return Result<long>.Success(placeImage.Id);
	}
}
