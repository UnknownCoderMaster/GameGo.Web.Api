using GameGo.Application.Common.Models;
using GameGo.Application.Contracts.Identity;
using GameGo.Application.Contracts.Infrastructure;
using GameGo.Application.Contracts.Persistence;
using GameGo.Application.Features.Places.Commands.UploadPlaceImage;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GameGo.Application.Features.Places.Commands.UpdatePlace;

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
		// 1. Place mavjudligini tekshirish
		var place = await _context.Places
			.Include(p => p.PlaceImages)
			.FirstOrDefaultAsync(p => p.Id == request.PlaceId, cancellationToken);

		if (place == null)
			return Result<long>.Failure("Joy topilmadi");

		// 2. Faqat joy egasi rasm qo'sha oladi
		if (place.OwnerId != _currentUser.UserId)
			return Result<long>.Failure("Siz bu joyga rasm qo'sha olmaysiz");

		// 3. Rasmni upload qilish
		var imageUrl = await _fileService.UploadFileAsync(
			request.ImageStream,
			request.FileName,
			"places",
			cancellationToken);

		// 4. Agar bu primary rasm bo'lsa, boshqa primary rasmlarni o'chirish
		if (request.IsPrimary)
		{
			foreach (var image in place.PlaceImages.Where(i => i.IsPrimary))
			{
				image.IsPrimary = false;
				image.UpdatedAt = DateTime.UtcNow;
			}
		}

		// 5. Database'ga qo'shish
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