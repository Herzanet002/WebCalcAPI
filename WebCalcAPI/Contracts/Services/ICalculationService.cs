using WebCalcAPI.Models;

namespace WebCalcAPI.Contracts.Services
{
    public interface ICalculationService
    {
        Task<CalculationModel> TwoOperandCalculate(double left, double right, string operation);
    }
}