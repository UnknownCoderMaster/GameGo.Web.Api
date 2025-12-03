using GameGo.Domain.Common;
using GameGo.Domain.Enums;
using System;
using System.Collections.Generic;

namespace GameGo.Domain.Entities;

public class User : AuditableEntity
{
	public string Email { get; set; }
	public string PasswordHash { get; set; } = null!;
	public string PhoneNumber { get; set; } = null!;
	public string FirstName { get; set; } = null!;
	public string LastName { get; set; } = null!;
	public DateTime? DateOfBirth { get; set; }
	public Gender? Gender { get; set; }
	public string AvatarUrl { get; set; }
	public bool IsActive { get; set; }
	public bool IsEmailVerified { get; set; }
	public bool IsPhoneVerified { get; set; }

	// Navigation Properties
	public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
	public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();
	public virtual ICollection<Favourite> Favourites { get; set; } = new List<Favourite>();
	public virtual PlaceOwner PlaceOwner { get; set; }

	private User() { }

	public static User Create(
		string email,
		string passwordHash,
		string phoneNumber,
		string firstName,
		string lastName)
	{
		if (string.IsNullOrWhiteSpace(email))
			throw new ArgumentException("Email cannot be empty", nameof(email));

		if (string.IsNullOrWhiteSpace(passwordHash))
			throw new ArgumentException("Password hash cannot be empty", nameof(passwordHash));

		if (string.IsNullOrWhiteSpace(phoneNumber))
			throw new ArgumentException("Phone number cannot be empty", nameof(phoneNumber));

		return new User
		{
			Email = email.ToLower(),
			PasswordHash = passwordHash,
			PhoneNumber = phoneNumber,
			FirstName = firstName,
			LastName = lastName,
			IsActive = true,
			IsEmailVerified = false,
			IsPhoneVerified = false
		};
	}

	public void UpdateProfile(string firstName, string lastName, DateTime? dateOfBirth, Gender? gender, string PhoneNumber)
	{
		FirstName = firstName;
		LastName = lastName;
		DateOfBirth = dateOfBirth;
		Gender = gender;
		PhoneNumber = PhoneNumber ?? this.PhoneNumber;
	}

	public void VerifyEmail() => IsEmailVerified = true;

	public void VerifyPhone() => IsPhoneVerified = true;

	public void UpdatePassword(string newPasswordHash)
	{
		if (string.IsNullOrWhiteSpace(newPasswordHash))
			throw new ArgumentException("Password hash cannot be empty", nameof(newPasswordHash));

		PasswordHash = newPasswordHash;
	}

	public void Deactivate() => IsActive = false;

	public void Activate() => IsActive = true;
}