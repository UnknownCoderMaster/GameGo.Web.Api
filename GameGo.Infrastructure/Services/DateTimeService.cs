using System;
using GameGo.Application.Contracts.Infrastructure;

namespace GameGo.Infrastructure.Services;

public class DateTimeService : IDateTime
{
	public DateTime Now => DateTime.Now;
	public DateTime UtcNow => DateTime.UtcNow;
}