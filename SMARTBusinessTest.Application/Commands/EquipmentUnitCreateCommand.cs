using SMARTBusinessTest.Application.Constants;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SMARTBusinessTest.Application.Commands
{
    public class EquipmentUnitCreateCommand
    {
        public string Code { get; set; }

        [Range(EquipmentUnitCommandConstants.lowBound, 
         EquipmentUnitCommandConstants.hihgBound, 
         ErrorMessage = ExceptionConstants.InvalidRange)]
        [DefaultValue(0)]
        public int Amount { get; set; }
    }
}
