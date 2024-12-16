using System.ComponentModel.DataAnnotations.Schema;

namespace SMARTBusinessTest.Entities
{
    [Table("Unit")]
    public class EquipmentUnit
    {
        [Column("Unit_Id")]
        public Guid Id { get; set; }

        [Column("Unit_EquipmentId")]
        public Guid EquipmentId { get; set; }
        public ProcessEquipment Equipment { get; set; }

        [Column("Unit_Amount")]
        public int Amount { get; set; }

        [Column("Unit_ContractId")]
        public Guid ContractId { get; set; }
        public PlacementContract Contract { get; set; }

    }
}
