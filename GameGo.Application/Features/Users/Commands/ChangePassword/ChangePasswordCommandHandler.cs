using GameGo.Application.Common.Models;
using GameGo.Application.Contracts.Identity;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace GameGo.Application.Features.Users.Commands.ChangePassword;

public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, Result<bool>>
{
	private readonly IIdentityService _identityService;
	private readonly ICurrentUserService _currentUser;

	public ChangePasswordCommandHandler(
		IIdentityService identityService,
		ICurrentUserService currentUser)
	{
		_identityService = identityService;
		_currentUser = currentUser;
	}

	public async Task<Result<bool>> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
	{
		if (!_currentUser.UserId.HasValue)
			return Result<bool>.Failure("Foydalanuvchi topilmadi");

		return Result<bool>.Success(true);
	}
}