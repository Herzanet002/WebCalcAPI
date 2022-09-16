using WebCalcAPI.Contracts.Services;
using WebCalcAPI.Models;

namespace WebCalcAPI
{
    public class CalculationService : ICalculationService
    {
        public CalculationModel TwoOperandCalculate(double left, double right, string operation)
        {
            var result = operation switch
            {
                Operators.ADD => left + right,
                Operators.SUB => left - right,
                Operators.MUL => left * right,
                Operators.DIV => left / right,
                _ => .0
            };
            return new CalculationModel
            {
                LeftOperand = left,
                RightOperand = right,
                Result = result,
                Operator = operation
            };
        }
    }
}
