using GameGo.Application.Common.Models;
using GameGo.Application.Contracts.Identity;
using GameGo.Application.Contracts.Infrastructure;
using GameGo.Application.Contracts.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GameGo.Application.Features.Users.Commands.UploadAvatar;

public class UploadAvatarCommandHandler : IRequestHandler<UploadAvatarCommand, Result<string>>
{
	private readonly IApplicationDbContext _context;
	private readonly IFileService _fileService;
	private readonly ICurrentUserService _currentUser;

	public UploadAvatarCommandHandler(
		IApplicationDbContext context,
		IFileService fileService,
		ICurrentUserService currentUser)
	{
		_context = context;
		_fileService = fileService;
		_currentUser = currentUser;
	}

	public async Task<Result<string>> Handle(UploadAvatarCommand request, CancellationToken cancellationToken)
	{
		if (!_currentUser.UserId.HasValue)
			return Result<string>.Failure("Foydalanuvchi topilmadi");

		var user = await _context.Users
			.FirstOrDefaultAsync(u => u.Id == _currentUser.UserId.Value, cancellationToken);

		if (user == null)
			return Result<string>.Failure("Foydalanuvchi topilmadi");

		// Eski avatarni o'chirish
		if (!string.IsNullOrEmpty(user.AvatarUrl))
		{
			await _fileService.DeleteFileAsync(user.AvatarUrl, cancellationToken);
		}

		// Yangi avatarni yuklash
		var avatarUrl = await _fileService.UploadFileAsync(
			request.ImageStream,
			request.FileName,
			"avatars",
			cancellationToken);

		// Database'ni yangilash
		user.AvatarUrl = avatarUrl;
		await _context.SaveChangesAsync(cancellationToken);

		return Result<string>.Success(avatarUrl);
	}
}