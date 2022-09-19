using System.ComponentModel.DataAnnotations;

namespace WebCalcAPI.Models
{
    public class CalculationModel
    {
        [Required(ErrorMessage = "Left cannot be empty!")]
        public double LeftOperand { get; set; }

        [Required(ErrorMessage = "Right cannot be empty!")]
        public double RightOperand { get; set; }

        [Required(ErrorMessage = "Operation cannot be empty!")]
        public string Operator { get; set; } = null!;

        public double Result { get; set; }

        public override bool Equals(object? obj)
        {
            var other = obj as CalculationModel;

            return other != null &&
                   other.LeftOperand == LeftOperand &&
                   other.RightOperand == RightOperand &&
                   other.Operator == Operator &&
                   other.Result == Result;
        }
    }
}