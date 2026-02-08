using System.IO;
using GameGo.Application.Common.Models;
using MediatR;

namespace GameGo.Application.Features.Places.Commands.UploadPlaceTypeImage;

public class UploadPlaceTypeImageCommand : IRequest<Result<string>>
{
	public long PlaceTypeId { get; set; }
	public Stream ImageStream { get; set; }
	public string FileName { get; set; }
}
