using System;
using System.Collections.Generic;

namespace GameGo.Application.Common.Exceptions;

public class ValidationException : Exception
{
	public List<string> Errors { get; }

	public ValidationException() : base("One or more validation failures have occurred.")
	{
		Errors = new List<string>();
	}

	public ValidationException(IEnumerable<string> errors) : this()
	{
		Errors = new List<string>(errors);
	}
}