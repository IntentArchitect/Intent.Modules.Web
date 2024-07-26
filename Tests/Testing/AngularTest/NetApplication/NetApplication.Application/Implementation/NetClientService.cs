using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Intent.RoslynWeaver.Attributes;
using NetApplication.Application.Common.Pagination;
using NetApplication.Application.Interfaces;
using NetApplication.Application.NetClient;
using NetApplication.Domain.Common.Exceptions;
using NetApplication.Domain.Entities;
using NetApplication.Domain.Repositories;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.ServiceImplementations.ServiceImplementation", Version = "1.0")]

namespace NetApplication.Application.Implementation
{
    [IntentManaged(Mode.Merge)]
    public class NetClientService : INetClientService
    {
        private readonly IClientRepository _clientRepository;
        private readonly IMapper _mapper;

        [IntentManaged(Mode.Merge)]
        public NetClientService(IClientRepository clientRepository, IMapper mapper)
        {
            _clientRepository = clientRepository;
            _mapper = mapper;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Fully)]
        public async Task<Guid> CreateClient(ClientCreateDto dto, CancellationToken cancellationToken = default)
        {
            var newClient = new Client
            {
                Name = dto.Name,
                StatusCode = dto.StatusCode,
            };
            _clientRepository.Add(newClient);
            await _clientRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            return newClient.Id;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Fully)]
        public async Task<ClientDto> FindClientById(Guid id, CancellationToken cancellationToken = default)
        {
            var element = await _clientRepository.FindByIdAsync(id, cancellationToken);

            if (element is null)
            {
                throw new NotFoundException($"Could not find Client {id}");
            }
            return element.MapToClientDto(_mapper);
        }

        [IntentManaged(Mode.Fully, Body = Mode.Fully)]
        public async Task<List<ClientDto>> FindClients(CancellationToken cancellationToken = default)
        {
            var elements = await _clientRepository.FindAllAsync(cancellationToken);
            return elements.MapToClientDtoList(_mapper);
        }

        [IntentManaged(Mode.Fully, Body = Mode.Fully)]
        public async Task UpdateClient(Guid id, ClientUpdateDto dto, CancellationToken cancellationToken = default)
        {
            var existingClient = await _clientRepository.FindByIdAsync(id, cancellationToken);

            if (existingClient is null)
            {
                throw new NotFoundException($"Could not find Client {id}");
            }
            existingClient.Name = dto.Name;
            existingClient.StatusCode = dto.StatusCode;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public async Task DeleteClient(Guid id, CancellationToken cancellationToken = default)
        {
            // TODO: Implement DeleteClient (NetClientService) functionality
            throw new NotImplementedException("Write your implementation for this service here...");
        }

        [IntentManaged(Mode.Fully, Body = Mode.Fully)]
        public async Task<PagedResult<ClientDto>> FindClients(
            int pageNo,
            int pageSize,
            CancellationToken cancellationToken = default)
        {
            var results = await _clientRepository.FindAllAsync(
                pageNo: pageNo,
                pageSize: pageSize,
                cancellationToken: cancellationToken);
            return results.MapToPagedResult(x => x.MapToClientDto(_mapper));
        }

        public void Dispose()
        {
        }
    }
}