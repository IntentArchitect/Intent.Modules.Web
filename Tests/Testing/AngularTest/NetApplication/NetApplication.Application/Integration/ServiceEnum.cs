using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.ContractEnumModel", Version = "1.0")]

namespace NetApplication.Application.Integration
{
    public enum ServiceEnum
    {
        ServiceLit1 = 1,
        ServiceLit2
    }
}