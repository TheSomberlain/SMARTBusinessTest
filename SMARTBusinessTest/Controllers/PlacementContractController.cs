using Microsoft.AspNetCore.Mvc;
using SMARTBusinessTest.Application.Commands;
using SMARTBusinessTest.Application.Filters;
using SMARTBusinessTest.Application.Interfaces;


namespace SMARTBusinessTest.Controllers
{
    [Route("api/v1/contract")]
    [ApiController]
    public class PlacementContractController : ControllerBase
    {
        private readonly IPlacementContractService _placementContractService;

        public PlacementContractController(IPlacementContractService placementContractService)
        {
            _placementContractService = placementContractService;
        }

        [HttpGet]
        [ServiceFilter(typeof(ContractExceptionFilter))]
        public async Task<IActionResult> Get(CancellationToken token)
        {
            var contracts = await _placementContractService.GetPlacementContractsAsync(token);
            return Ok(contracts);
        }

        [HttpPost]
        [ServiceFilter(typeof(ContractExceptionFilter))]
        public async Task<IActionResult> Post([FromBody] PlacementContractCreateCommand newContract, CancellationToken token)
        {
            await _placementContractService.CreatePlacementContractAsync(newContract, token);
            return StatusCode(StatusCodes.Status201Created);
        }

    }
}
