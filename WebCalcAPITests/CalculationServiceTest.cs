using System.Collections;
using Moq;
using WebCalcAPI;
using WebCalcAPI.Contracts.Services;
using WebCalcAPI.Controllers;
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
        [TestCaseSource(typeof(DataClass), nameof(DataClass.TestCalculationModelCases))]

        public async Task<CalculationModel> TwoOperandCalculateTest(double left, double right, string operation)
        {
            return await _calculationService.TwoOperandCalculate(left, right, operation);
        }

    }

    public class DataClass
    {
        public static IEnumerable TestCalculationModelCases
        {
            get
            {
                yield return new TestCaseData(1, 2, "+").Returns(new CalculationModel
                {
                    LeftOperand = 1,
                    RightOperand = 2,
                    Operator = "+",
                    Result = 3.0
                });
                yield return new TestCaseData(2, 5, "-").Returns(new CalculationModel
                {
                    LeftOperand = 2,
                    RightOperand = 5,
                    Operator = "-",
                    Result = -3.0
                });
                yield return new TestCaseData(3, 2, "*").Returns(new CalculationModel
                {
                    LeftOperand = 3,
                    RightOperand = 2,
                    Operator = "*",
                    Result = 6
                });
                yield return new TestCaseData(4, 2, "/").Returns(new CalculationModel
                {
                    LeftOperand = 4,
                    RightOperand = 2,
                    Operator = "/",
                    Result = 2
                });

            }
        }

    }
}