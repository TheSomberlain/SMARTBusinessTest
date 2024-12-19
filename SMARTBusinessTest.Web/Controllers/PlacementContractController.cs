using Microsoft.AspNetCore.Mvc;
using SMARTBusinessTest.Application.Commands;
using SMARTBusinessTest.Application.Interfaces;
using SMARTBusinessTest.Web.Filters;

namespace SMARTBusinessTest.Web.Controllers
{
    [Route("api/v1/contract")]
    [ApiController]
    [ServiceFilter(typeof(ContractExceptionFilter))]
    [ServiceFilter(typeof(ApiAuthFilter))]
    public class PlacementContractController : ControllerBase
    {
        private readonly IPlacementContractService _placementContractService;

        public PlacementContractController(IPlacementContractService placementContractService)
        {
            _placementContractService = placementContractService;
        }

        [HttpGet]
 
        public async Task<IActionResult> Get(CancellationToken token)
        {
            var contracts = await _placementContractService.GetPlacementContractsAsync(token);
            return Ok(contracts);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PlacementContractCreateCommand newContract, CancellationToken token)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            await _placementContractService.CreatePlacementContractAsync(newContract, token);
            return StatusCode(StatusCodes.Status201Created);
        }

    }
}
