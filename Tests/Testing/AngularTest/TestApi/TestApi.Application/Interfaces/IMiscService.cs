using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Intent.RoslynWeaver.Attributes;
using TestApi.Application.Misc;
using TestApi.Application.People;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Contracts.ServiceContract", Version = "1.0")]

namespace TestApi.Application.Interfaces
{

    public interface IMiscService : IDisposable
    {

        Task<PersonDTO> GetWithQueryParam(Guid idParam);

        Task<PersonDTO> GetWithRouteParam(Guid routeId);

        Task PostWithHeaderParam(string param);

        Task PostWithBodyParam(PersonUpdateDTO param);

        Task<int> GetWithPrimitiveResultInt();

        Task<int> GetWithPrimitiveResultWrapInt();

        Task<bool> GetWithPrimitiveResultBool();

        Task<bool> GetWithPrimitiveResultWrapBool();

        Task<string> GetWithPrimitiveResultStr();

        Task<string> GetWithPrimitiveResultWrapStr();

        Task<DateTime> GetWithPrimitiveResultDate();

        Task<DateTime> GetWithPrimitiveResultWrapDate();

        Task PostDateParam(DateTime date, DateTime datetime);

        Task PostDateParamDto(DateDTO dto);

    }
}