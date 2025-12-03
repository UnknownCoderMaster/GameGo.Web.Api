using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace GameGo.Application.Contracts.Infrastructure;

public interface IFileService
{
	Task<string> UploadFileAsync(Stream fileStream, string fileName, string folder, CancellationToken cancellationToken = default);
	Task<bool> DeleteFileAsync(string fileUrl, CancellationToken cancellationToken = default);
	Task<Stream> DownloadFileAsync(string fileUrl, CancellationToken cancellationToken = default);
}