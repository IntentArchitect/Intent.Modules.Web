using Intent.RoslynWeaver.Attributes;
using NetApplication.Domain;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.DtoModel", Version = "1.0")]

namespace NetApplication.Application.Integration
{
    public class EnumTestDto
    {
        public EnumTestDto()
        {
        }

        public ServiceEnum Enum1 { get; set; }
        public DomainEnum Enum2 { get; set; }

        public static EnumTestDto Create(ServiceEnum enum1, DomainEnum enum2)
        {
            return new EnumTestDto
            {
                Enum1 = enum1,
                Enum2 = enum2
            };
        }
    }
}