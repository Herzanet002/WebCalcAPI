// ReSharper disable NonReadonlyMemberInGetHashCode
namespace WebCalcAPI.Models;

public class CalculationResultModel : ComputeModel
{
    public double Result { get; set; }

    public override bool Equals(object? obj)
    {
        return obj is CalculationResultModel other &&
               other.LeftOperand.Equals(LeftOperand) &&
               other.RightOperand.Equals(RightOperand) &&
               other.Operator.Equals(Operator) &&
               other.Result.Equals(Result);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(LeftOperand, RightOperand, Result, Operator);
    }
}