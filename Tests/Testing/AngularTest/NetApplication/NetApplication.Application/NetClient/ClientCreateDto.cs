using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.DtoModel", Version = "1.0")]

namespace NetApplication.Application.NetClient
{
    public class ClientCreateDto
    {
        public ClientCreateDto()
        {
            Name = null!;
        }

        public string Name { get; set; }
        public char StatusCode { get; set; }

        public static ClientCreateDto Create(string name, char statusCode)
        {
            return new ClientCreateDto
            {
                Name = name,
                StatusCode = statusCode
            };
        }
    }
}