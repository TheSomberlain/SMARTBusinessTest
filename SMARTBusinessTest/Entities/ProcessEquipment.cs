using System.ComponentModel.DataAnnotations.Schema;

namespace SMARTBusinessTest.Entities
{
    [Table("Equipment")]
    public class ProcessEquipment
    {
        [Column("Equipment_Id")]
        public Guid Id { get; set; }

        [Column("Equipment_Code")]
        public string Code { get; set; }
        
        [Column("Equipment_Name")]
        public string Name { get; set; }

        [Column("Equipment_Area")]
        public int Area { get; set; }
        public ICollection<EquipmentUnit> Units { get; set; }
    }
}
