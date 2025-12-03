using System;

namespace GameGo.Application.Common.Exceptions;

public class UnauthorizedException : Exception
{
	public UnauthorizedException() : base("Unauthorized access.")
	{
	}

	public UnauthorizedException(string message) : base(message)
	{
	}
}