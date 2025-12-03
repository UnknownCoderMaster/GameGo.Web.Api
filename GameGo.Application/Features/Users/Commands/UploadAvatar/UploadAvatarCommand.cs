using GameGo.Application.Common.Models;
using MediatR;
using System.IO;

namespace GameGo.Application.Features.Users.Commands.UploadAvatar;

public class UploadAvatarCommand : IRequest<Result<string>>
{
	public Stream ImageStream { get; set; }
	public string FileName { get; set; }
}
