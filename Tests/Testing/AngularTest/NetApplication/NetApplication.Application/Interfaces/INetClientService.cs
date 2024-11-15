using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Intent.RoslynWeaver.Attributes;
using NetApplication.Application.Common.Pagination;
using NetApplication.Application.NetClient;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Contracts.ServiceContract", Version = "1.0")]

namespace NetApplication.Application.Interfaces
{
    public interface INetClientService : IDisposable
    {
        Task<Guid> CreateClient(ClientCreateDto dto, CancellationToken cancellationToken = default);
        Task<ClientDto> FindClientById(Guid id, CancellationToken cancellationToken = default);
        Task<List<ClientDto>> FindClients(CancellationToken cancellationToken = default);
        Task UpdateClient(Guid id, ClientUpdateDto dto, CancellationToken cancellationToken = default);
        Task DeleteClient(Guid id, CancellationToken cancellationToken = default);
        Task<PagedResult<ClientDto>> FindClients(int pageNo, int pageSize, CancellationToken cancellationToken = default);
    }
}