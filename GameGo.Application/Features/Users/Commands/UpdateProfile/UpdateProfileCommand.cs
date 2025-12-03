using GameGo.Application.Common.Models;
using MediatR;
using System;

namespace GameGo.Application.Features.Users.Commands.UpdateProfile;

public class UpdateProfileCommand : IRequest<Result<bool>>
{
	public string FirstName { get; set; }
	public string LastName { get; set; }
	public DateTime? DateOfBirth { get; set; }
	public string Gender { get; set; } // "Male", "Female"
	public string PhoneNumber { get; set; }
}