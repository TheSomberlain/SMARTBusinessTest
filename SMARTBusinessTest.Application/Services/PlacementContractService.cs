using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SMARTBusinessTest.Application.Commands;
using SMARTBusinessTest.Application.DTOs;
using SMARTBusinessTest.Domain.Exceptions;
using SMARTBusinessTest.Application.Interfaces;
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
            if (!IsPlacementValid(newPlacementContract.TotalEquipmentArea, newPlacementContract.ProductionFacility.Area))
                throw new OutOfPlacementException("The specified amount of equipment exceeds the facility area.");

            _dbContext.EquipmentUnits.AddRange(newPlacementContract.EquipmentUnits);
            await _dbContext.PlacementContract.AddAsync(newPlacementContract, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        private bool IsPlacementValid(long requiredArea, long actualArea)
        {      
            return requiredArea < actualArea;
        }

        private async Task<bool> IsCommandInputValid(PlacementContractCreateCommand newPlacementContractCommand,
                                         CancellationToken cancellationToken)
        {
            var facilityExists = await _dbContext.ProductionFacilities
                .AnyAsync(x => x.Code == newPlacementContractCommand.FacilityCode, cancellationToken);

            var equipmentCodes = newPlacementContractCommand.EquipmentUnits.Select(x => x.Code).ToList();
            var existingEquipmentCodes = await _dbContext.ProcessEquipment
                .Where(x => equipmentCodes.Contains(x.Code))
                .Select(x => x.Code)
                .ToListAsync(cancellationToken);
            var equipmentExists = equipmentCodes.All(existingEquipmentCodes.Contains);
            var amountPositive = newPlacementContractCommand.EquipmentUnits.Select(x => x.Amount).All(x => x > 0);
            return facilityExists && equipmentExists && amountPositive;
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
                        EquipmentId = equipment.Id,
                        TotalArea = unit.Amount * equipment.Area
                    });
                }
            }

            var totalContractArea = equipmentUnits.Select(x => x.TotalArea).Sum();
            if(!IsTotalAreaValid(totalContractArea))
                throw new InvalidInputDataException("Invalid input data");

            return new PlacementContract() 
            {
                ProductionFacility = facility,
                EquipmentUnits = equipmentUnits,
                TotalEquipmentArea = (int)totalContractArea
            };
        }

        private bool IsTotalAreaValid(long totalArea)
        {
            return  totalArea < int.MaxValue;
        }
    }
}
