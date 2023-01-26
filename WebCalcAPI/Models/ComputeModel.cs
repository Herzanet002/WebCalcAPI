using System.ComponentModel.DataAnnotations;

namespace WebCalcAPI.Models;

public class ComputeModel
{
    [Required(ErrorMessage = "Left operand cannot be empty!")]
    public double LeftOperand { get; set; }

    [Required(ErrorMessage = "Right operand cannot be empty!")]
    public double RightOperand { get; set; }

    [Required(ErrorMessage = "Operator cannot be empty!")]
    public string Operator { get; set; } = string.Empty;
}