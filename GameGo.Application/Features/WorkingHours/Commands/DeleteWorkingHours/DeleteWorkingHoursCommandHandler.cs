using GameGo.Application.Common.Models;
using GameGo.Application.Contracts.Identity;
using GameGo.Application.Contracts.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace GameGo.Application.Features.WorkingHours.Commands.DeleteWorkingHours;

public class DeleteWorkingHoursCommandHandler : IRequestHandler<DeleteWorkingHoursCommand, Result<bool>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public DeleteWorkingHoursCommandHandler(IApplicationDbContext context, ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<Result<bool>> Handle(DeleteWorkingHoursCommand request, CancellationToken cancellationToken)
    {
        if (!_currentUser.IsAuthenticated || !_currentUser.UserId.HasValue)
            return Result<bool>.Failure("User must be authenticated");

        var workingHours = await _context.WorkingHours
            .Include(w => w.Place)
            .FirstOrDefaultAsync(w => w.Id == request.Id, cancellationToken);

        if (workingHours == null)
        {
            return Result<bool>.Failure("Working hours not found");
        }

        if (workingHours.Place.OwnerId != _currentUser.UserId.Value)
            return Result<bool>.Failure("Only place owner can delete working hours");

        _context.WorkingHours.Remove(workingHours);
        await _context.SaveChangesAsync(cancellationToken);

        return Result<bool>.Success(true);
    }
}
