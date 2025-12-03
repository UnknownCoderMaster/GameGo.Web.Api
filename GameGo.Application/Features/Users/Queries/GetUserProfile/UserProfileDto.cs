using System;

namespace GameGo.Application.Features.Users.Queries.GetUserProfile;

public class UserProfileDto
{
	public long Id { get; set; }
	public string Email { get; set; }
	public string PhoneNumber { get; set; }
	public string FirstName { get; set; }
	public string LastName { get; set; }
	public DateTime? DateOfBirth { get; set; }
	public string Gender { get; set; }
	public string AvatarUrl { get; set; }
	public bool IsEmailVerified { get; set; }
	public bool IsPhoneVerified { get; set; }
	public DateTime CreatedAt { get; set; }

	// Statistics
	public int TotalBookings { get; set; }
	public int CompletedBookings { get; set; }
	public int CancelledBookings { get; set; }
	public int TotalRatings { get; set; }
	public int FavouritePlacesCount { get; set; }
}