using SMARTBusinessTest.Domain.Commands;
using SMARTBusinessTest.Domain.DTOs;

namespace SMARTBusinessTest.Domain.Interfaces
{
    public interface IPlacementContractService
    {
        Task<IEnumerable<PlacementContractDTO>> GetPlacementContractsAsync(CancellationToken cancellationToken);
        Task CreatePlacementContractAsync(PlacementContractCreateCommand newPlacementContract, CancellationToken cancellationToken);
    }
}
