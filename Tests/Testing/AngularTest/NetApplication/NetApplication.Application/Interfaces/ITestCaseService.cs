using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Intent.RoslynWeaver.Attributes;
using NetApplication.Application.Common;
using NetApplication.Application.Integration;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Contracts.ServiceContract", Version = "1.0")]

namespace NetApplication.Application.Interfaces
{
    /// <summary>
    /// For Angular service proxy test cases
    /// </summary>
    public interface ITestCaseService
    {
        Task<EnumTestDto> EnumTest(CancellationToken cancellationToken = default);
        Task<FileDownloadDto> Download(CancellationToken cancellationToken = default);
        Task Upload(Stream content, string? filename, string? contentType, long? contentLength, CancellationToken cancellationToken = default);
    }
}