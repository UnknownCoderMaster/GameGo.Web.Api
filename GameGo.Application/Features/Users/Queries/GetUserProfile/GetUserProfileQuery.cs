using GameGo.Application.Common.Models;
using GameGo.Application.Common.Models.Dtos;
using MediatR;

namespace GameGo.Application.Features.Users.Queries.GetUserProfile;

public class GetUserProfileQuery : IRequest<Result<UserProfileDto>>
{
}