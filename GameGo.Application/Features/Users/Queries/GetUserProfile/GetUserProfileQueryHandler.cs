using GameGo.Application.Common.Models;
using GameGo.Application.Contracts.Identity;
using GameGo.Application.Contracts.Persistence;
using GameGo.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GameGo.Application.Features.Users.Queries.GetUserProfile;

public class GetUserProfileQueryHandler : IRequestHandler<GetUserProfileQuery, Result<UserProfileDto>>
{
	private readonly IApplicationDbContext _context;
	private readonly ICurrentUserService _currentUser;

	public GetUserProfileQueryHandler(
		IApplicationDbContext context,
		ICurrentUserService currentUser)
	{
		_context = context;
		_currentUser = currentUser;
	}

	public async Task<Result<UserProfileDto>> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
	{
		if (!_currentUser.UserId.HasValue)
			return Result<UserProfileDto>.Failure("Foydalanuvchi topilmadi");

		var user = await _context.Users
			.Where(u => u.Id == _currentUser.UserId.Value)
			.Select(u => new UserProfileDto
			{
				Id = u.Id,
				Email = u.Email,
				PhoneNumber = u.PhoneNumber,
				FirstName = u.FirstName,
				LastName = u.LastName,
				DateOfBirth = u.DateOfBirth,
				Gender = u.Gender.HasValue ? u.Gender.Value.ToString() : null,
				AvatarUrl = u.AvatarUrl,
				IsEmailVerified = u.IsEmailVerified,
				IsPhoneVerified = u.IsPhoneVerified,
				CreatedAt = u.CreatedAt,

				// Statistics
				TotalBookings = u.Bookings.Count,
				CompletedBookings = u.Bookings.Count(b => b.Status == BookingStatus.Completed),
				CancelledBookings = u.Bookings.Count(b => b.Status == BookingStatus.Cancelled),
				TotalRatings = u.Ratings.Count,
				FavouritePlacesCount = u.Favourites.Count
			})
			.FirstOrDefaultAsync(cancellationToken);

		if (user == null)
			return Result<UserProfileDto>.Failure("Foydalanuvchi topilmadi");

		return Result<UserProfileDto>.Success(user);
	}
}