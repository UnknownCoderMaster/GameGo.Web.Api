using GameGo.Application.Common.Models;
using MediatR;

namespace GameGo.Application.Features.WorkingHours.Commands.DeleteWorkingHours;

public record DeleteWorkingHoursCommand : IRequest<Result<bool>>
{
    public long Id { get; init; }
}
