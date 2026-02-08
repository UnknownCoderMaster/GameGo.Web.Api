using FluentValidation;
using System.IO;
using System.Linq;

namespace GameGo.Application.Features.Places.Commands.UploadPlaceTypeImage;

public class UploadPlaceTypeImageCommandValidator : AbstractValidator<UploadPlaceTypeImageCommand>
{
	private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png", ".webp" };

	public UploadPlaceTypeImageCommandValidator()
	{
		RuleFor(x => x.PlaceTypeId)
			.GreaterThan(0).WithMessage("PlaceType ID noto'g'ri");

		RuleFor(x => x.ImageStream)
			.NotNull().WithMessage("Rasm kiritilishi shart");

		RuleFor(x => x.FileName)
			.NotEmpty().WithMessage("Fayl nomi kiritilishi shart")
			.Must(BeValidExtension).WithMessage("Faqat jpg, jpeg, png, webp formatdagi rasmlar qabul qilinadi");
	}

	private bool BeValidExtension(string fileName)
	{
		if (string.IsNullOrEmpty(fileName)) return false;
		var extension = Path.GetExtension(fileName).ToLower();
		return AllowedExtensions.Contains(extension);
	}
}
