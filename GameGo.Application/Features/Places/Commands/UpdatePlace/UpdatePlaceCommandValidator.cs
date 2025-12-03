using FluentValidation;
using GameGo.Application.Features.Places.Commands.UploadPlaceImage;
using System.IO;
using System.Linq;

namespace GameGo.Application.Features.Places.Commands.UpdatePlace;

public class UploadPlaceImageCommandValidator : AbstractValidator<UploadPlaceImageCommand>
{
	private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png", ".webp" };

	public UploadPlaceImageCommandValidator()
	{
		RuleFor(x => x.PlaceId)
			.GreaterThan(0).WithMessage("Place ID noto'g'ri");

		RuleFor(x => x.ImageStream)
			.NotNull().WithMessage("Rasm kiritilishi shart");

		RuleFor(x => x.FileName)
			.NotEmpty().WithMessage("Fayl nomi kiritilishi shart")
			.Must(BeValidExtension).WithMessage("Faqat jpg, jpeg, png, webp formatdagi rasmlar qabul qilinadi");

		RuleFor(x => x.DisplayOrder)
			.GreaterThanOrEqualTo(0).WithMessage("Display order 0 yoki katta bo'lishi kerak");
	}

	private bool BeValidExtension(string fileName)
	{
		var extension = Path.GetExtension(fileName).ToLower();
		return AllowedExtensions.Contains(extension);
	}
}