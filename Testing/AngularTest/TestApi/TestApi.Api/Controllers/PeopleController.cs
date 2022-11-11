using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using Intent.RoslynWeaver.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestApi.Api.Controllers.ResponseTypes;
using TestApi.Application.Interfaces;
using TestApi.Application.People;
using TestApi.Domain.Common.Interfaces;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: DefaultIntentManaged(Mode.Fully, Targets = Targets.Usings)]
[assembly: IntentTemplate("Intent.AspNetCore.Controllers.Controller", Version = "1.0")]

namespace TestApi.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PeopleController : ControllerBase
    {
        private readonly IPeopleService _appService;

        private readonly IUnitOfWork _unitOfWork;

        public PeopleController(IPeopleService appService,
            IUnitOfWork unitOfWork)
        {
            _appService = appService ?? throw new ArgumentNullException(nameof(appService));

            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        /// <summary>
        /// </summary>
        /// <response code="201">Successfully created.</response>
        /// <response code="400">One or more validation errors have occurred.</response>
        [HttpPost]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Guid>> Create([FromBody] PersonCreateDTO dto, CancellationToken cancellationToken)
        {
            var result = default(Guid);

            using (var transaction = new TransactionScope(TransactionScopeOption.Required,
                new TransactionOptions() { IsolationLevel = IsolationLevel.ReadCommitted }, TransactionScopeAsyncFlowOption.Enabled))
            {

                result = await _appService.Create(dto);


                await _unitOfWork.SaveChangesAsync(cancellationToken);
                transaction.Complete();
            }
            return Created(string.Empty, result);
        }

        /// <summary>
        /// </summary>
        /// <response code="200">Returns the specified PersonDTO.</response>
        /// <response code="400">One or more validation errors have occurred.</response>
        /// <response code="404">Can't find an PersonDTO with the parameters provided.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PersonDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PersonDTO>> FindById([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var result = default(PersonDTO);

            using (var transaction = new TransactionScope(TransactionScopeOption.Required,
                new TransactionOptions() { IsolationLevel = IsolationLevel.ReadCommitted }, TransactionScopeAsyncFlowOption.Enabled))
            {

                result = await _appService.FindById(id);

            }
            return Ok(result);
        }

        /// <summary>
        /// </summary>
        /// <response code="200">Returns the specified List&lt;PersonDTO&gt;.</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<PersonDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<PersonDTO>>> FindAll(CancellationToken cancellationToken)
        {
            var result = default(List<PersonDTO>);

            using (var transaction = new TransactionScope(TransactionScopeOption.Required,
                new TransactionOptions() { IsolationLevel = IsolationLevel.ReadCommitted }, TransactionScopeAsyncFlowOption.Enabled))
            {

                result = await _appService.FindAll();

            }
            return Ok(result);
        }

        /// <summary>
        /// </summary>
        /// <response code="204">Successfully updated.</response>
        /// <response code="400">One or more validation errors have occurred.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Update([FromRoute] Guid id, [FromBody] PersonUpdateDTO dto, CancellationToken cancellationToken)
        {
            using (var transaction = new TransactionScope(TransactionScopeOption.Required,
                new TransactionOptions() { IsolationLevel = IsolationLevel.ReadCommitted }, TransactionScopeAsyncFlowOption.Enabled))
            {

                await _appService.Update(id, dto);

                await _unitOfWork.SaveChangesAsync(cancellationToken);
                transaction.Complete();
            }
            return NoContent();
        }

        /// <summary>
        /// </summary>
        /// <response code="200">Successfully deleted.</response>
        /// <response code="400">One or more validation errors have occurred.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            using (var transaction = new TransactionScope(TransactionScopeOption.Required,
                new TransactionOptions() { IsolationLevel = IsolationLevel.ReadCommitted }, TransactionScopeAsyncFlowOption.Enabled))
            {

                await _appService.Delete(id);

                await _unitOfWork.SaveChangesAsync(cancellationToken);
                transaction.Complete();
            }
            return Ok();
        }

        /// <summary>
        /// </summary>
        /// <response code="200">Returns the specified PersonDTO.</response>
        /// <response code="400">One or more validation errors have occurred.</response>
        /// <response code="404">Can't find an PersonDTO with the parameters provided.</response>
        [HttpGet("[action]")]
        [ProducesResponseType(typeof(PersonDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PersonDTO>> GetWithQueryParam([FromQuery] Guid idParam, CancellationToken cancellationToken)
        {
            var result = default(PersonDTO);

            using (var transaction = new TransactionScope(TransactionScopeOption.Required,
                new TransactionOptions() { IsolationLevel = IsolationLevel.ReadCommitted }, TransactionScopeAsyncFlowOption.Enabled))
            {

                result = await _appService.GetWithQueryParam(idParam);

            }
            return Ok(result);
        }

        /// <summary>
        /// </summary>
        /// <response code="200">Returns the specified PersonDTO.</response>
        /// <response code="400">One or more validation errors have occurred.</response>
        /// <response code="404">Can't find an PersonDTO with the parameters provided.</response>
        [HttpGet("[action]/{routeId}")]
        [ProducesResponseType(typeof(PersonDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PersonDTO>> GetWithRouteParam([FromRoute] Guid routeId, CancellationToken cancellationToken)
        {
            var result = default(PersonDTO);

            using (var transaction = new TransactionScope(TransactionScopeOption.Required,
                new TransactionOptions() { IsolationLevel = IsolationLevel.ReadCommitted }, TransactionScopeAsyncFlowOption.Enabled))
            {

                result = await _appService.GetWithRouteParam(routeId);

            }
            return Ok(result);
        }

        /// <summary>
        /// </summary>
        /// <response code="201">Successfully created.</response>
        /// <response code="400">One or more validation errors have occurred.</response>
        [HttpPost("[action]")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> PostWithFormParam([FromForm] string param1, [FromForm] string param2, CancellationToken cancellationToken)
        {
            using (var transaction = new TransactionScope(TransactionScopeOption.Required,
                new TransactionOptions() { IsolationLevel = IsolationLevel.ReadCommitted }, TransactionScopeAsyncFlowOption.Enabled))
            {

                await _appService.PostWithFormParam(param1, param2);

                await _unitOfWork.SaveChangesAsync(cancellationToken);
                transaction.Complete();
            }
            return Created(string.Empty, null);
        }

        /// <summary>
        /// </summary>
        /// <response code="201">Successfully created.</response>
        /// <response code="400">One or more validation errors have occurred.</response>
        [HttpPost("[action]")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> PostWithHeaderParam([FromHeader(Name = "MY_HEADER")] string param, CancellationToken cancellationToken)
        {
            using (var transaction = new TransactionScope(TransactionScopeOption.Required,
                new TransactionOptions() { IsolationLevel = IsolationLevel.ReadCommitted }, TransactionScopeAsyncFlowOption.Enabled))
            {

                await _appService.PostWithHeaderParam(param);

                await _unitOfWork.SaveChangesAsync(cancellationToken);
                transaction.Complete();
            }
            return Created(string.Empty, null);
        }

        /// <summary>
        /// </summary>
        /// <response code="201">Successfully created.</response>
        /// <response code="400">One or more validation errors have occurred.</response>
        [HttpPost("[action]")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> PostWithBodyParam([FromBody] PersonUpdateDTO param, CancellationToken cancellationToken)
        {
            using (var transaction = new TransactionScope(TransactionScopeOption.Required,
                new TransactionOptions() { IsolationLevel = IsolationLevel.ReadCommitted }, TransactionScopeAsyncFlowOption.Enabled))
            {

                await _appService.PostWithBodyParam(param);

                await _unitOfWork.SaveChangesAsync(cancellationToken);
                transaction.Complete();
            }
            return Created(string.Empty, null);
        }

        /// <summary>
        /// </summary>
        /// <response code="200">Returns the specified int.</response>
        /// <response code="404">Can't find an int with the parameters provided.</response>
        [HttpGet("[action]")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<int>> GetWithPrimitiveResultInt(CancellationToken cancellationToken)
        {
            var result = default(int);

            using (var transaction = new TransactionScope(TransactionScopeOption.Required,
                new TransactionOptions() { IsolationLevel = IsolationLevel.ReadCommitted }, TransactionScopeAsyncFlowOption.Enabled))
            {

                result = await _appService.GetWithPrimitiveResultInt();

            }
            return Ok(result);
        }

        /// <summary>
        /// </summary>
        /// <response code="200">Returns the specified int.</response>
        /// <response code="404">Can't find an int with the parameters provided.</response>
        [HttpGet("[action]")]
        [ProducesResponseType(typeof(JsonResponse<int>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<int>> GetWithPrimitiveResultWrapInt(CancellationToken cancellationToken)
        {
            var result = default(int);

            using (var transaction = new TransactionScope(TransactionScopeOption.Required,
                new TransactionOptions() { IsolationLevel = IsolationLevel.ReadCommitted }, TransactionScopeAsyncFlowOption.Enabled))
            {

                result = await _appService.GetWithPrimitiveResultWrapInt();

            }
            return Ok(new JsonResponse<int>(result));
        }

        /// <summary>
        /// </summary>
        /// <response code="200">Returns the specified bool.</response>
        /// <response code="404">Can't find an bool with the parameters provided.</response>
        [HttpGet("[action]")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> GetWithPrimitiveResultBool(CancellationToken cancellationToken)
        {
            var result = default(bool);

            using (var transaction = new TransactionScope(TransactionScopeOption.Required,
                new TransactionOptions() { IsolationLevel = IsolationLevel.ReadCommitted }, TransactionScopeAsyncFlowOption.Enabled))
            {

                result = await _appService.GetWithPrimitiveResultBool();

            }
            return Ok(result);
        }

        /// <summary>
        /// </summary>
        /// <response code="200">Returns the specified bool.</response>
        /// <response code="404">Can't find an bool with the parameters provided.</response>
        [HttpGet("[action]")]
        [ProducesResponseType(typeof(JsonResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> GetWithPrimitiveResultWrapBool(CancellationToken cancellationToken)
        {
            var result = default(bool);

            using (var transaction = new TransactionScope(TransactionScopeOption.Required,
                new TransactionOptions() { IsolationLevel = IsolationLevel.ReadCommitted }, TransactionScopeAsyncFlowOption.Enabled))
            {

                result = await _appService.GetWithPrimitiveResultWrapBool();

            }
            return Ok(new JsonResponse<bool>(result));
        }

        /// <summary>
        /// </summary>
        /// <response code="200">Returns the specified string.</response>
        /// <response code="404">Can't find an string with the parameters provided.</response>
        [HttpGet("[action]")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<string>> GetWithPrimitiveResultStr(CancellationToken cancellationToken)
        {
            var result = default(string);

            using (var transaction = new TransactionScope(TransactionScopeOption.Required,
                new TransactionOptions() { IsolationLevel = IsolationLevel.ReadCommitted }, TransactionScopeAsyncFlowOption.Enabled))
            {

                result = await _appService.GetWithPrimitiveResultStr();

            }
            return Ok(result);
        }

        /// <summary>
        /// </summary>
        /// <response code="200">Returns the specified string.</response>
        /// <response code="404">Can't find an string with the parameters provided.</response>
        [HttpGet("[action]")]
        [ProducesResponseType(typeof(JsonResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<string>> GetWithPrimitiveResultWrapStr(CancellationToken cancellationToken)
        {
            var result = default(string);

            using (var transaction = new TransactionScope(TransactionScopeOption.Required,
                new TransactionOptions() { IsolationLevel = IsolationLevel.ReadCommitted }, TransactionScopeAsyncFlowOption.Enabled))
            {

                result = await _appService.GetWithPrimitiveResultWrapStr();

            }
            return Ok(new JsonResponse<string>(result));
        }

        /// <summary>
        /// </summary>
        /// <response code="200">Returns the specified DateTime.</response>
        /// <response code="404">Can't find an DateTime with the parameters provided.</response>
        [HttpGet("[action]")]
        [ProducesResponseType(typeof(DateTime), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<DateTime>> GetWithPrimitiveResultDate(CancellationToken cancellationToken)
        {
            var result = default(DateTime);

            using (var transaction = new TransactionScope(TransactionScopeOption.Required,
                new TransactionOptions() { IsolationLevel = IsolationLevel.ReadCommitted }, TransactionScopeAsyncFlowOption.Enabled))
            {

                result = await _appService.GetWithPrimitiveResultDate();

            }
            return Ok(result);
        }

        /// <summary>
        /// </summary>
        /// <response code="200">Returns the specified DateTime.</response>
        /// <response code="404">Can't find an DateTime with the parameters provided.</response>
        [HttpGet("[action]")]
        [ProducesResponseType(typeof(JsonResponse<DateTime>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<DateTime>> GetWithPrimitiveResultWrapDate(CancellationToken cancellationToken)
        {
            var result = default(DateTime);

            using (var transaction = new TransactionScope(TransactionScopeOption.Required,
                new TransactionOptions() { IsolationLevel = IsolationLevel.ReadCommitted }, TransactionScopeAsyncFlowOption.Enabled))
            {

                result = await _appService.GetWithPrimitiveResultWrapDate();

            }
            return Ok(new JsonResponse<DateTime>(result));
        }


    }
}