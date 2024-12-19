using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace SMARTBusinessTest.Domain.Entities
{
    [Table("Contract")]
    public class PlacementContract
    {
        [Column("Contract_Id")]
        public Guid Id { get; set; }

        [Column("Contract_FacilityId")]
        public Guid FacilityId { get; set; }
        public ProductionFacility ProductionFacility { get; set; }
        public ICollection<EquipmentUnit> EquipmentUnits { get; set; }
        public int TotalEquipmentArea {  get; set; }
    }
}
