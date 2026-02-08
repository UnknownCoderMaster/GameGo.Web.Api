using GameGo.Application.Common.Models;
using GameGo.Application.Contracts.Infrastructure;
using GameGo.Application.Contracts.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace GameGo.Application.Features.Places.Commands.UploadPlaceTypeImage;

public class UploadPlaceTypeImageCommandHandler : IRequestHandler<UploadPlaceTypeImageCommand, Result<string>>
{
	private readonly IApplicationDbContext _context;
	private readonly IFileService _fileService;

	public UploadPlaceTypeImageCommandHandler(
		IApplicationDbContext context,
		IFileService fileService)
	{
		_context = context;
		_fileService = fileService;
	}

	public async Task<Result<string>> Handle(UploadPlaceTypeImageCommand request, CancellationToken cancellationToken)
	{
		var placeType = await _context.PlaceTypes
			.FirstOrDefaultAsync(pt => pt.Id == request.PlaceTypeId, cancellationToken);

		if (placeType == null)
			return Result<string>.Failure("PlaceType not found");

		// Delete old image if exists
		if (!string.IsNullOrEmpty(placeType.ImageUrl))
		{
			await _fileService.DeleteFileAsync(placeType.ImageUrl, cancellationToken);
		}

		// Upload new image
		var imageUrl = await _fileService.UploadFileAsync(
			request.ImageStream,
			request.FileName,
			"place-types",
			cancellationToken);

		placeType.UpdateImage(imageUrl);
		await _context.SaveChangesAsync(cancellationToken);

		return Result<string>.Success(imageUrl);
	}
}
