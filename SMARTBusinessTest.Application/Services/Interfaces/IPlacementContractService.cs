using SMARTBusinessTest.Application.Commands;
using SMARTBusinessTest.Application.DTOs;

namespace SMARTBusinessTest.Application.Interfaces
{
    public interface IPlacementContractService
    {
        Task<IEnumerable<PlacementContractDTO>> GetPlacementContractsAsync(CancellationToken cancellationToken);
        Task CreatePlacementContractAsync(PlacementContractCreateCommand newPlacementContract, CancellationToken cancellationToken);
    }
}
