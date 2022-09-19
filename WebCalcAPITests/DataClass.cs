using System.Collections;
using WebCalcAPI.Models;

namespace WebCalcAPITests;

public class DataClass
{
    public static IEnumerable TestCalculationModelCasesInCalculationService
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