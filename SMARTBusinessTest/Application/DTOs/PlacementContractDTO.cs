namespace SMARTBusinessTest.Application.DTOs
{
    public class PlacementContractDTO
    {
        public Guid ContractId { get; set; }
        public IEnumerable<EquipmentUnitDTO> EquipmentUnits { get; set; }
        public ProductionFacilityDTO ProductionFacility { get; set; }
    }
}
