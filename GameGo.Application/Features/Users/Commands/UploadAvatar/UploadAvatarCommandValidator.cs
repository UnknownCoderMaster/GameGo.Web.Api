using System.IO;
using System.Linq;
using FluentValidation;

namespace GameGo.Application.Features.Users.Commands.UploadAvatar;

public class UploadAvatarCommandValidator : AbstractValidator<UploadAvatarCommand>
{
	private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png", ".webp" };

	public UploadAvatarCommandValidator()
	{
		RuleFor(x => x.ImageStream)
			.NotNull().WithMessage("Rasm kiritilishi shart");

		RuleFor(x => x.FileName)
			.NotEmpty().WithMessage("Fayl nomi kiritilishi shart")
			.Must(BeValidExtension).WithMessage("Faqat jpg, jpeg, png, webp formatdagi rasmlar qabul qilinadi");
	}

	private bool BeValidExtension(string fileName)
	{
		var extension = Path.GetExtension(fileName).ToLower();
		return AllowedExtensions.Contains(extension);
	}
}
