using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Intent.RoslynWeaver.Attributes;
using TestApi.Application.People;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Contracts.ServiceContract", Version = "1.0")]

namespace TestApi.Application.Interfaces
{

    public interface IPeopleService : IDisposable
    {

        Task<Guid> Create(PersonCreateDTO dto);

        Task<PersonDTO> FindById(Guid id);

        Task<List<PersonDTO>> FindAll();

        Task Update(Guid id, PersonUpdateDTO dto);

        Task Delete(Guid id);

        Task<PersonDTO> GetWithQueryParam(Guid idParam);

        Task<PersonDTO> GetWithRouteParam(Guid routeId);

        Task PostWithFormParam(string param1, string param2);

        Task PostWithHeaderParam(string param);

        Task PostWithBodyParam(PersonUpdateDTO param);

        Task<int> GetWithPrimitiveResultInt();

        Task<int> GetWithPrimitiveResultWrapInt();

        Task<bool> GetWithPrimitiveResultBool();

        Task<bool> GetWithPrimitiveResultWrapBool();

        Task<string> GetWithPrimitiveResultStr();

        Task<string> GetWithPrimitiveResultWrapStr();

    }
}