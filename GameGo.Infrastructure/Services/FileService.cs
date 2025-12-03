using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using GameGo.Application.Contracts.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace GameGo.Infrastructure.Services;

public class FileService : IFileService
{
	private readonly IWebHostEnvironment _environment;
	private readonly IConfiguration _configuration;

	public FileService(IWebHostEnvironment environment, IConfiguration configuration)
	{
		_environment = environment;
		_configuration = configuration;
	}

	public async Task<string> UploadFileAsync(Stream fileStream, string fileName, string folder, CancellationToken cancellationToken = default)
	{
		var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", folder);

		if (!Directory.Exists(uploadsFolder))
		{
			Directory.CreateDirectory(uploadsFolder);
		}

		var uniqueFileName = $"{Guid.NewGuid()}_{fileName}";
		var filePath = Path.Combine(uploadsFolder, uniqueFileName);

		using (var fileStreamOutput = new FileStream(filePath, FileMode.Create))
		{
			await fileStream.CopyToAsync(fileStreamOutput, cancellationToken);
		}

		return $"/uploads/{folder}/{uniqueFileName}";
	}

	public Task<bool> DeleteFileAsync(string fileUrl, CancellationToken cancellationToken = default)
	{
		try
		{
			var filePath = Path.Combine(_environment.WebRootPath, fileUrl.TrimStart('/'));

			if (File.Exists(filePath))
			{
				File.Delete(filePath);
				return Task.FromResult(true);
			}

			return Task.FromResult(false);
		}
		catch
		{
			return Task.FromResult(false);
		}
	}

	public Task<Stream> DownloadFileAsync(string fileUrl, CancellationToken cancellationToken = default)
	{
		var filePath = Path.Combine(_environment.WebRootPath, fileUrl.TrimStart('/'));

		if (!File.Exists(filePath))
		{
			throw new FileNotFoundException("File not found", filePath);
		}

		var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
		return Task.FromResult<Stream>(stream);
	}
}