using WebCalcAPI.Contracts.Services;
using WebCalcAPI.Models;
using WebCalcAPI.Services;

namespace WebCalcAPITests
{
    public class Tests
    {
        private ICalculationService _calculationService = null!;

        [SetUp]
        public void Setup()
        {
            _calculationService = new CalculationService();
        }

        [Test]
        [TestCaseSource(typeof(DataClass), nameof(DataClass.TestCalculationModelCasesInCalculationService))]
        public async Task<CalculationResultModel> TwoOperandCalculateTest(ComputeModel computeModel)
        {
            return await _calculationService.TwoOperandCalculate(computeModel);
        }
    }
}