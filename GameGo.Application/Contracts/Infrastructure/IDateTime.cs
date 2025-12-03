using System;

namespace GameGo.Application.Contracts.Infrastructure;

public interface IDateTime
{
	DateTime Now { get; }
	DateTime UtcNow { get; }
}