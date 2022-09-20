namespace WebCalcAPI.Models;

public class CalculationResultModel : ComputeModel
{
    public double Result { get; set; }

    public override bool Equals(object? obj)
    {
        var other = obj as CalculationResultModel;

        return other != null &&
               other.LeftOperand == LeftOperand &&
               other.RightOperand == RightOperand &&
               other.Operator == Operator &&
               other.Result == Result;
    }
}