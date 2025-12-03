using System;

namespace GameGo.Application.Common.Exceptions;

public class ForbiddenException : Exception
{
	public ForbiddenException() : base("Access forbidden.")
	{
	}

	public ForbiddenException(string message) : base(message)
	{
	}
}