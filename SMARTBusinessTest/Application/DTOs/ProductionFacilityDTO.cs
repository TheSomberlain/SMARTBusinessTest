using System.ComponentModel.DataAnnotations.Schema;

namespace SMARTBusinessTest.Application.DTOs
{
    public class ProductionFacilityDTO
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int Area { get; set; }
    }
}
