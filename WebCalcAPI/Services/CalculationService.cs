using WebCalcAPI.Contracts.Services;
using WebCalcAPI.Models;

namespace WebCalcAPI.Services;

public class CalculationService : ICalculationService
{
    public async Task<CalculationResultModel> TwoOperandCalculate(ComputeModel computeModel)
    {
        var result = HandleResult(computeModel.LeftOperand, computeModel.RightOperand, computeModel.Operator);
        await Task.Delay(5000);
        return await new ValueTask<CalculationResultModel>(new CalculationResultModel
        {
            LeftOperand = computeModel.LeftOperand,
            RightOperand = computeModel.RightOperand,
            Result = result,
            Operator = computeModel.Operator
        });
    }

    private double HandleResult(double left, double right, string operation)
    {
        return operation switch
        {
            Operators.ADD => left + right,
            Operators.SUB => left - right,
            Operators.MUL => left * right,
            Operators.DIV => left / right,
            _ => .0
        };
    }
}