using GameGo.Application.Common.Models;
using MediatR;
using System;

namespace GameGo.Application.Features.Analytics.Queries.GetBookingStatistics;

public class GetBookingStatisticsQuery : IRequest<Result<BookingStatisticsDto>>
{
	public long? PlaceId { get; set; }
	public DateTime? StartDate { get; set; }
	public DateTime? EndDate { get; set; }
}
