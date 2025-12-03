using GameGo.Domain.Enums;
using System;

namespace GameGo.Application.Common.Models.Dtos;

public class UserProfileDto
{
	public long Id { get; set; }
	public string Email { get; set; }
	public string PhoneNumber { get; set; }
	public string FirstName { get; set; }
	public string LastName { get; set; }
	public DateTime? DateOfBirth { get; set; }
	public Gender? Gender { get; set; }
	public string AvatarUrl { get; set; }
	public bool IsEmailVerified { get; set; }
	public bool IsPhoneVerified { get; set; }
}