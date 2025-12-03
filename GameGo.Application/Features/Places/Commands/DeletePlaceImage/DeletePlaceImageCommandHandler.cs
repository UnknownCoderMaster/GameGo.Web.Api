using GameGo.Application.Common.Models;
using GameGo.Application.Contracts.Identity;
using GameGo.Application.Contracts.Infrastructure;
using GameGo.Application.Contracts.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace GameGo.Application.Features.Places.Commands.DeletePlaceImage;

public class DeletePlaceImageCommandHandler : IRequestHandler<DeletePlaceImageCommand, Result<bool>>
{
	private readonly IApplicationDbContext _context;
	private readonly IFileService _fileService;
	private readonly ICurrentUserService _currentUser;

	public DeletePlaceImageCommandHandler(
		IApplicationDbContext context,
		IFileService fileService,
		ICurrentUserService currentUser)
	{
		_context = context;
		_fileService = fileService;
		_currentUser = currentUser;
	}

	public async Task<Result<bool>> Handle(DeletePlaceImageCommand request, CancellationToken cancellationToken)
	{
		// 1. Image'ni topish
		var image = await _context.PlaceImages
			.Include(i => i.Place)
			.FirstOrDefaultAsync(i => i.Id == request.ImageId && i.PlaceId == request.PlaceId, cancellationToken);

		if (image == null)
			return Result<bool>.Failure("Rasm topilmadi");

		// 2. Faqat joy egasi o'chira oladi
		if (image.Place.OwnerId != _currentUser.UserId)
			return Result<bool>.Failure("Siz bu rasmni o'chira olmaysiz");

		// 3. File system'dan o'chirish
		await _fileService.DeleteFileAsync(image.ImageUrl, cancellationToken);

		// 4. Database'dan o'chirish
		_context.PlaceImages.Remove(image);
		await _context.SaveChangesAsync(cancellationToken);

		return Result<bool>.Success(true);
	}
}