using System.IO;
using GameGo.Application.Common.Models;
using MediatR;

namespace GameGo.Application.Features.Places.Commands.UploadPlaceImage;

public class UploadPlaceImageCommand : IRequest<Result<long>>
{
	public long PlaceId { get; set; }
	public Stream ImageStream { get; set; }
	public string FileName { get; set; }
	public bool IsPrimary { get; set; }
	public int DisplayOrder { get; set; }
}