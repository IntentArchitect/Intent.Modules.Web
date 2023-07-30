using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.DtoModel", Version = "1.0")]

namespace NetApplication.Application.Integration
{
    public class CustomDTO
    {
        public CustomDTO()
        {
            ReferenceNumber = null!;
        }

        public string ReferenceNumber { get; set; }

        public static CustomDTO Create(string referenceNumber)
        {
            return new CustomDTO
            {
                ReferenceNumber = referenceNumber
            };
        }
    }
}