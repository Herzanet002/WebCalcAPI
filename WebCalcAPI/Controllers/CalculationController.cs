using Microsoft.AspNetCore.Mvc;
using WebCalcAPI.Models;

namespace WebCalcAPI.Controllers
{
    [Route("/api/[controller]")]
    public class CalculationController : Controller
    {
        [HttpGet("{left} {operation} {right}", Name = "Calculate")]
        public CalculationModel Calculate(double left, double right, string operation)
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
