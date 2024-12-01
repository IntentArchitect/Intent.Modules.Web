using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Intent.RoslynWeaver.Attributes;
using NetApplication.Application.Common;
using NetApplication.Application.Integration;
using NetApplication.Application.Interfaces;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.ServiceImplementations.ServiceImplementation", Version = "1.0")]

namespace NetApplication.Application.Implementation
{
    /// <summary>
    /// For Angular service proxy test cases
    /// </summary>
    [IntentManaged(Mode.Merge)]
    public class TestCaseService : ITestCaseService
    {
        [IntentManaged(Mode.Merge)]
        public TestCaseService()
        {
        }

        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public async Task<EnumTestDto> EnumTest(CancellationToken cancellationToken = default)
        {
            // TODO: Implement EnumTest (TestCaseService) functionality
            throw new NotImplementedException("Write your implementation for this service here...");
        }

        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public async Task<FileDownloadDto> Download(CancellationToken cancellationToken = default)
        {
            // TODO: Implement Download (TestCaseService) functionality
            throw new NotImplementedException("Write your implementation for this service here...");
        }

        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public async Task Upload(
            Stream content,
            string? filename,
            string? contentType,
            long? contentLength,
            CancellationToken cancellationToken = default)
        {
            // TODO: Implement Upload (TestCaseService) functionality
            throw new NotImplementedException("Write your implementation for this service here...");
        }
    }
}