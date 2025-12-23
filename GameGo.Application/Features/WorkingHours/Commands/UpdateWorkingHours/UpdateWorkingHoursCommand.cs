using GameGo.Application.Common.Models;
using GameGo.Domain.Enums;
using MediatR;
using System;

namespace GameGo.Application.Features.WorkingHours.Commands.UpdateWorkingHours;

public record UpdateWorkingHoursCommand : IRequest<Result<bool>>
{
    public long Id { get; init; }
    public TimeSpan OpenTime { get; init; }
    public TimeSpan CloseTime { get; init; }
    public bool IsClosed { get; init; }
}
