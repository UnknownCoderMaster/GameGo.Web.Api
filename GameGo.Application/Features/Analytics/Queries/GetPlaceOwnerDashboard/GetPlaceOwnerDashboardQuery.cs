using GameGo.Application.Common.Models;
using MediatR;

namespace GameGo.Application.Features.Analytics.Queries.GetPlaceOwnerDashboard;

public class GetPlaceOwnerDashboardQuery : IRequest<Result<PlaceOwnerDashboardDto>>
{
}
