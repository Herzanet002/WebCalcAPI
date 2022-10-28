using System.Collections;
using WebCalcAPI.Models;

namespace WebCalcAPITests;

public class DataClass
{
    public static IEnumerable TestCalculationModelCasesInCalculationService
    {
        get
        {
            yield return new TestCaseData(new ComputeModel
            {
                LeftOperand = 1,
                RightOperand = 2,
                Operator = "+"
            }).Returns(new CalculationResultModel
            {
                LeftOperand = 1,
                RightOperand = 2,
                Operator = "+",
                Result = 3.0
            });

            yield return new TestCaseData(new ComputeModel
            {
                LeftOperand = 2,
                RightOperand = 5,
                Operator = "-"
            }).Returns(new CalculationResultModel
            {
                LeftOperand = 2,
                RightOperand = 5,
                Operator = "-",
                Result = -3.0
            });

            yield return new TestCaseData(new ComputeModel
            {
                LeftOperand = -3,
                RightOperand = 2,
                Operator = "*"
            }).Returns(new CalculationResultModel
            {
                LeftOperand = -3,
                RightOperand = 2,
                Operator = "*",
                Result = -6
            });

            yield return new TestCaseData(new ComputeModel
            {
                LeftOperand = 4,
                RightOperand = 2,
                Operator = "/"
            }).Returns(new CalculationResultModel
            {
                LeftOperand = 4,
                RightOperand = 2,
                Operator = "/",
                Result = 2
            });
        }
    }
}