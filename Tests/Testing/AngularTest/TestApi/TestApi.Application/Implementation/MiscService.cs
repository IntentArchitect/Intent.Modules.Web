using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Intent.RoslynWeaver.Attributes;
using TestApi.Application.Interfaces;
using TestApi.Application.Misc;
using TestApi.Application.People;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.ServiceImplementations.ServiceImplementation", Version = "1.0")]

namespace TestApi.Application.Implementation
{
    [IntentManaged(Mode.Merge)]
    public class MiscService : IMiscService
    {

        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public MiscService()
        {
        }

        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public async Task<PersonDTO> GetWithQueryParam(Guid idParam)
        {
            return PersonDTO.Create(Guid.NewGuid(), "Test " + Guid.NewGuid());
        }

        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public async Task<PersonDTO> GetWithRouteParam(Guid routeId)
        {
            return PersonDTO.Create(Guid.NewGuid(), "Test " + Guid.NewGuid());
        }

        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public async Task PostWithHeaderParam(string param)
        {
            
        }

        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public async Task PostWithBodyParam(PersonUpdateDTO param)
        {
            
        }

        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public async Task<int> GetWithPrimitiveResultInt()
        {
            return 12;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public async Task<int> GetWithPrimitiveResultWrapInt()
        {
            return 13;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public async Task<bool> GetWithPrimitiveResultBool()
        {
            return true;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public async Task<bool> GetWithPrimitiveResultWrapBool()
        {
            return false;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public async Task<string> GetWithPrimitiveResultStr()
        {
            return "Primitive string value";
        }

        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public async Task<string> GetWithPrimitiveResultWrapStr()
        {
            return "Wrapped Primitive string value";
        }

        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public async Task<DateTime> GetWithPrimitiveResultDate()
        {
            return DateTime.Now;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public async Task<DateTime> GetWithPrimitiveResultWrapDate()
        {
            return DateTime.UtcNow;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public async Task PostDateParam(DateTime date, DateTime datetime)
        {
            
        }

        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public async Task PostDateParamDto(DateDTO dto)
        {
            
        }

        public void Dispose()
        {
        }
    }
}