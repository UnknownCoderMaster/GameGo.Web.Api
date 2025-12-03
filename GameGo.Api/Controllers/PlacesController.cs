using GameGo.Application.Features.Places.Commands.CreatePlace;
using GameGo.Application.Features.Places.Commands.DeletePlaceImage;
using GameGo.Application.Features.Places.Commands.SetPrimaryImage;
using GameGo.Application.Features.Places.Commands.UploadPlaceImage;
using GameGo.Application.Features.Places.Queries.GetPlaceById;
using GameGo.Application.Features.Places.Queries.GetPlaceTypes;
using GameGo.Application.Features.Places.Queries.SearchPlaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GameGo.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlacesController : ControllerBase
{
	private readonly IMediator _mediator;

	public PlacesController(IMediator mediator)
	{
		_mediator = mediator;
	}

	[HttpGet("{id}")]
	public async Task<IActionResult> GetById(long id)
	{
		var result = await _mediator.Send(new GetPlaceByIdQuery { Id = id });

		if (!result.IsSuccess)
			return NotFound(new { error = result.Error });

		return Ok(result.Data);
	}

	[HttpGet("search")]
	public async Task<IActionResult> Search([FromQuery] SearchPlacesQuery query)
	{
		var result = await _mediator.Send(query);

		if (!result.IsSuccess)
			return BadRequest(new { error = result.Error });

		return Ok(result.Data);
	}

	[HttpGet("types")]
	public async Task<IActionResult> GetTypes()
	{
		var result = await _mediator.Send(new GetPlaceTypesQuery());

		if (!result.IsSuccess)
			return BadRequest(new { error = result.Error });

		return Ok(result.Data);
	}

	[HttpPost]
	[Authorize]
	public async Task<IActionResult> Create([FromBody] CreatePlaceCommand command)
	{
		var result = await _mediator.Send(command);

		if (!result.IsSuccess)
			return BadRequest(new { error = result.Error });

		return CreatedAtAction(nameof(GetById), new { id = result.Data }, new { id = result.Data });
	}


	[HttpPost("{id}/images")]
	[Authorize]
	[RequestSizeLimit(10_485_760)] // 10 MB limit
	public async Task<IActionResult> UploadImage(long id, IFormFile file, [FromForm] bool isPrimary = false, [FromForm] int displayOrder = 0)
	{
		if (file == null || file.Length == 0)
			return BadRequest(new { error = "Fayl tanlanmadi" });

		// Max 10 MB
		if (file.Length > 10_485_760)
			return BadRequest(new { error = "Fayl hajmi 10 MB dan katta bo'lmasligi kerak" });

		using var stream = file.OpenReadStream();

		var command = new UploadPlaceImageCommand
		{
			PlaceId = id,
			ImageStream = stream,
			FileName = file.FileName,
			IsPrimary = isPrimary,
			DisplayOrder = displayOrder
		};

		var result = await _mediator.Send(command);

		if (!result.IsSuccess)
			return BadRequest(new { error = result.Error });

		return Ok(new { imageId = result.Data, message = "Rasm muvaffaqiyatli yuklandi" });
	}

	[HttpDelete("{placeId}/images/{imageId}")]
	[Authorize]
	public async Task<IActionResult> DeleteImage(long placeId, long imageId)
	{
		var result = await _mediator.Send(new DeletePlaceImageCommand
		{
			PlaceId = placeId,
			ImageId = imageId
		});

		if (!result.IsSuccess)
			return BadRequest(new { error = result.Error });

		return Ok(new { message = "Rasm o'chirildi" });
	}

	[HttpPut("{placeId}/images/{imageId}/set-primary")]
	[Authorize]
	public async Task<IActionResult> SetPrimaryImage(long placeId, long imageId)
	{
		var result = await _mediator.Send(new SetPrimaryImageCommand
		{
			PlaceId = placeId,
			ImageId = imageId
		});

		if (!result.IsSuccess)
			return BadRequest(new { error = result.Error });

		return Ok(new { message = "Asosiy rasm o'zgartirildi" });
	}
}