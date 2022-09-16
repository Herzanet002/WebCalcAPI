using Microsoft.AspNetCore.Mvc;
using WebCalcAPI.Contracts.Services;
using WebCalcAPI.Models;

namespace WebCalcAPI.Controllers
{
    [Route("/api/[controller]")]
    public class CalculationController : Controller
    {
        private readonly ICalculationService _calculationService;

        public CalculationController(ICalculationService calculationService)
        {
            _calculationService = calculationService;
        }


        [HttpGet("{left} {operation} {right}", Name = "Calculate")]
        public CalculationModel Calculate(double left, double right, string operation)
        {
            return _calculationService.TwoOperandCalculate(left, right, operation);
        }

        

    }
}
