using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SMARTBusinessTest.Domain.Commands;
using SMARTBusinessTest.Domain.DTOs;
using SMARTBusinessTest.Domain.Exceptions;
using SMARTBusinessTest.Domain.Interfaces;
using SMARTBusinessTest.Domain.Entities;
using SMARTBusinessTest.Infrastructure;


namespace SMARTBusinessTest.Application.Services
{
    public class PlacementContractService : IPlacementContractService
    {
        private EquipmentContractsDbContext _dbContext;
        private IMapper _mapper;
        public PlacementContractService(EquipmentContractsDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async  Task<IEnumerable<PlacementContractDTO>> GetPlacementContractsAsync(CancellationToken cancellationToken)
        {
            var contracts = await _dbContext.PlacementContract.AsNoTracking()
                            .Include(x => x.ProductionFacility)
                            .Include(x => x.EquipmentUnits)
                            .ThenInclude(x => x.Equipment)
                            .ToListAsync(cancellationToken);
            return _mapper.Map<IEnumerable<PlacementContractDTO>>(contracts);
        }

        public async Task CreatePlacementContractAsync(PlacementContractCreateCommand newPlacementContractCommand, 
                                                       CancellationToken cancellationToken)
        {
            if (!await IsCommandInputValid(newPlacementContractCommand, cancellationToken))
                throw new InvalidInputDataException("Invalid input data");

            var newPlacementContract = await AdjectContractFromCommand(newPlacementContractCommand, cancellationToken);
            if (!IsPlacementContractValid(newPlacementContract))
                throw new OutOfPlacementException("The specified amount of equipment exceeds the facility area.");

            _dbContext.EquipmentUnits.AddRange(newPlacementContract.EquipmentUnits);
            await _dbContext.PlacementContract.AddAsync(newPlacementContract, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        private bool IsPlacementContractValid(PlacementContract newPlacementContract)
        {
            var totalArea = CalculateTotalArea(newPlacementContract);
            return totalArea < newPlacementContract.ProductionFacility.Area;
        }

        private async Task<bool> IsCommandInputValid(PlacementContractCreateCommand newPlacementContractCommand,
                                         CancellationToken cancellationToken)
        {
            var facilityExists = await _dbContext.ProductionFacilities
                .AnyAsync(x => x.Code == newPlacementContractCommand.FacilityCode, cancellationToken);
            if (!facilityExists) return false;

            var equipmentCodes = newPlacementContractCommand.EquipmentUnits.Select(x => x.Code).ToList();
            var existingEquipmentCodes = await _dbContext.ProcessEquipment
                .Where(x => equipmentCodes.Contains(x.Code))
                .Select(x => x.Code)
                .ToListAsync(cancellationToken);

            return equipmentCodes.All(existingEquipmentCodes.Contains);
        }

        private async Task<PlacementContract> AdjectContractFromCommand(PlacementContractCreateCommand newPlacementContractCommand,
                                                               CancellationToken cancellationToken)
        {
            var facility = await _dbContext.ProductionFacilities
               .FirstOrDefaultAsync( x => x.Code == newPlacementContractCommand.FacilityCode, cancellationToken);

            var equipmentUnits = new List<EquipmentUnit>();
            foreach (var unit in newPlacementContractCommand.EquipmentUnits)
            {
                var equipment = await _dbContext.ProcessEquipment
                    .FirstOrDefaultAsync(x => x.Code == unit.Code, cancellationToken);

                if (equipment != null)
                {
                    equipmentUnits.Add(new EquipmentUnit
                    {
                        Amount = unit.Amount,
                        Equipment = equipment,
                        EquipmentId = equipment.Id
                    });
                }
            }
            return new PlacementContract() 
            {
                ProductionFacility = facility,
                EquipmentUnits = equipmentUnits,
            };
        }

        private static long CalculateTotalArea(PlacementContract placementContract)
        {
            long totalArea = 0;
            foreach (var unit in placementContract.EquipmentUnits)
            {
                totalArea += unit.Equipment.Area * unit.Amount;
            }
            return totalArea;
        }
    }
}
