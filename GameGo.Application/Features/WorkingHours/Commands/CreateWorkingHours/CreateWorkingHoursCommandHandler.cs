using GameGo.Application.Common.Models;
using GameGo.Application.Contracts.Identity;
using GameGo.Application.Contracts.Persistence;
using GameGo.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace GameGo.Application.Features.WorkingHours.Commands.CreateWorkingHours;

public class CreateWorkingHoursCommandHandler : IRequestHandler<CreateWorkingHoursCommand, Result<long>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public CreateWorkingHoursCommandHandler(IApplicationDbContext context, ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<Result<long>> Handle(CreateWorkingHoursCommand request, CancellationToken cancellationToken)
    {
        if (!_currentUser.IsAuthenticated || !_currentUser.UserId.HasValue)
            return Result<long>.Failure("User must be authenticated");

        var place = await _context.Places
            .FirstOrDefaultAsync(p => p.Id == request.PlaceId, cancellationToken);

        if (place == null)
        {
            return Result<long>.Failure("Place not found");
        }

        if (place.OwnerId != _currentUser.UserId.Value)
            return Result<long>.Failure("Only place owner can manage working hours");

        var existingWorkingHours = await _context.WorkingHours
            .FirstOrDefaultAsync(w => w.PlaceId == request.PlaceId && w.DayOfWeek == request.DayOfWeek, cancellationToken);

        if (existingWorkingHours != null)
        {
            return Result<long>.Failure("Working hours for this day already exist");
        }

        var workingHours = Domain.Entities.WorkingHours.Create(
            request.PlaceId,
            request.DayOfWeek,
            request.OpenTime,
            request.CloseTime,
            request.IsClosed);

        _context.WorkingHours.Add(workingHours);
        await _context.SaveChangesAsync(cancellationToken);

        return Result<long>.Success(workingHours.Id);
    }
}
