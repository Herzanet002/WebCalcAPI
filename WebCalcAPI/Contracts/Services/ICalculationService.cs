using WebCalcAPI.Models;

namespace WebCalcAPI.Contracts.Services;

public interface ICalculationService
{
    Task<CalculationResultModel> TwoOperandCalculate(ComputeModel computeModel);
}