using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Intent.RoslynWeaver.Attributes;
using NetApplication.Application.Integration;
using NetApplication.Application.Interfaces;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.ServiceImplementations.ServiceImplementation", Version = "1.0")]

namespace NetApplication.Application.Implementation
{
    [IntentManaged(Mode.Merge)]
    public class IntegrationService : IIntegrationService
    {
        public const string ReferenceNumber = "refnumber_1234";
        public const string DefaultString = "string value";
        public const int DefaultInt = 55;
        public const string ExceptionMessage = "Some exception message";
        public static readonly Guid DefaultGuid = Guid.Parse("b7698947-5237-4686-9571-442335426771");
        public const string Param1Value = "param 1";
        public const int Param2Value = 42;

        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public IntegrationService()
        {
        }

        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public async Task<CustomDTO> QueryParamOp(string param1, int param2, CancellationToken cancellationToken = default)
        {
            if (param1 != Param1Value)
            {
                throw new ArgumentException($@"{nameof(param1)} is not ""{Param1Value}"" but is ""{param1}""");
            }
            if (param2 != Param2Value)
            {
                throw new ArgumentException($@"{nameof(param2)} is not ""{Param2Value}"" but is ""{param2}""");
            }
            return CustomDTO.Create(ReferenceNumber);
        }

        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public async Task HeaderParamOp(string param1, CancellationToken cancellationToken = default)
        {
            if (param1 != Param1Value)
            {
                throw new ArgumentException($@"{nameof(param1)} is not ""{Param1Value}"" but is ""{param1}""");
            }
        }

        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public async Task RouteParamOp(string param1, CancellationToken cancellationToken = default)
        {
            if (param1 != Param1Value)
            {
                throw new ArgumentException($@"{nameof(param1)} is not ""{Param1Value}"" but is ""{param1}""");
            }
        }

        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public async Task BodyParamOp(CustomDTO param1, CancellationToken cancellationToken = default)
        {
            if (param1.ReferenceNumber != ReferenceNumber)
            {
                throw new ArgumentException($@"{nameof(param1.ReferenceNumber)} is not ""{ReferenceNumber}"" but is ""{param1.ReferenceNumber}""");
            }
        }

        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public async Task ThrowsException(CancellationToken cancellationToken = default)
        {
            throw new Exception(ExceptionMessage);
        }

        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public async Task<Guid> GetWrappedPrimitiveGuid(CancellationToken cancellationToken = default)
        {
            return DefaultGuid;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public async Task<string> GetWrappedPrimitiveString(CancellationToken cancellationToken = default)
        {
            return DefaultString;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public async Task<int> GetWrappedPrimitiveInt(CancellationToken cancellationToken = default)
        {
            return DefaultInt;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public async Task<Guid> GetPrimitiveGuid(CancellationToken cancellationToken = default)
        {
            return DefaultGuid;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public async Task<string> GetPrimitiveString(CancellationToken cancellationToken = default)
        {
            return DefaultString;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public async Task<int> GetPrimitiveInt(CancellationToken cancellationToken = default)
        {
            return DefaultInt;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public async Task<List<string>> GetPrimitiveStringList(CancellationToken cancellationToken = default)
        {
            return new List<string> { DefaultString };
        }

        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public async Task NonHttpSettingsOperation(CancellationToken cancellationToken = default)
        {
        }

        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public async Task<CustomDTO> GetInvoiceOpWithReturnTypeWrapped(CancellationToken cancellationToken = default)
        {
            return CustomDTO.Create(ReferenceNumber);
        }

        public void Dispose()
        {
        }
    }
}