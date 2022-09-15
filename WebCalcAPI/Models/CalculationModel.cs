using System.ComponentModel.DataAnnotations;

namespace WebCalcAPI.Models
{
    public class CalculationModel
    {
        [Required]
        public double LeftOperand { get; set; }

        [Required]
        public double RightOperand { get; set; }

        [Required]
        public string Operator { get; set; } = null!;

        public double Result { get; set; }
    }
}
