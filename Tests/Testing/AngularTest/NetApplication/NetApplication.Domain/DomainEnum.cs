using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Entities.DomainEnum", Version = "1.0")]

namespace NetApplication.Domain
{
    public enum DomainEnum
    {
        Lit1,
        Lit2 = 2
    }
}