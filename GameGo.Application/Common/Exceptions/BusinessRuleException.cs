using System;

namespace GameGo.Application.Common.Exceptions;

public class BusinessRuleException : Exception
{
	public BusinessRuleException(string message) : base(message)
	{
	}
}