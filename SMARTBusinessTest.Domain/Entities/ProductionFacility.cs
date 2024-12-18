using System.ComponentModel.DataAnnotations.Schema;

namespace SMARTBusinessTest.Domain.Entities
{
    [Table("Facility")]
    public class ProductionFacility
    {
        [Column("Facility_Id")]
        public Guid Id { get; set; }

        [Column("Facility_Code")]
        public string Code { get; set; }

        [Column("Facility_Name")]
        public string Name { get; set; }

        [Column("Facility_Area")]
        public int Area { get; set; }
        public ICollection<PlacementContract> Contracts { get; set; }
    }
}
