using GameGo.Application.Common.Models;
using GameGo.Domain.Enums;
using MediatR;
using System;

namespace GameGo.Application.Features.WorkingHours.Commands.CreateWorkingHours;

public record CreateWorkingHoursCommand : IRequest<Result<long>>
{
    public long PlaceId { get; init; }
    public GameGo.Domain.Enums.DayOfWeek DayOfWeek { get; init; }
    public TimeSpan OpenTime { get; init; }
    public TimeSpan CloseTime { get; init; }
    public bool IsClosed { get; init; }
}
