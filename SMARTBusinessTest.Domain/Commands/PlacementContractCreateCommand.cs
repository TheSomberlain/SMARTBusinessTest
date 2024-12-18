namespace SMARTBusinessTest.Domain.Commands
{
    public class PlacementContractCreateCommand
    {
        public string FacilityCode { get; set; }
        public IEnumerable<EquipmentUnitCreateCommand> EquipmentUnits { get; set; }
    }
}
