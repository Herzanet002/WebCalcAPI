using WebCalcAPI.Models;

namespace WebCalcAPI.Contracts.Services
{
    public interface ICalculationService
    {
        ValueTask<CalculationModel> TwoOperandCalculate(double left, double right, string operation);
    }
}
