using Microsoft.AspNetCore.Mvc;
using WebCalcAPI.Contracts.Services;
using WebCalcAPI.Models;

namespace WebCalcAPI.Controllers
{
    [Route("/api/[controller]")]
    public class CalculationController : Controller
    {
        private readonly ICalculationService _calculationService;
        private readonly IAsyncReplyRequestService _asyncReplyRequestService;
        public CalculationController(ICalculationService calculationService, IAsyncReplyRequestService asyncReplyRequestService)
        {
            _calculationService = calculationService;
            _asyncReplyRequestService = asyncReplyRequestService;
        }

        private async Task WaitTill(double left, double right, string operation)
        {
            await _calculationService.TwoOperandCalculate(left, right, operation);
        }

        [HttpPost("{left} {operation} {right}")]
        public Guid AsyncProcessingWorkAcceptor(double left, double right, string operation)
        {
            var guid = Guid.NewGuid();
            _asyncReplyRequestService.CreateNewTask(guid, WaitTill(left, right, operation));
            //CreateNewTask(guid, left, right, operation);
            return guid;
            //return _calculationService.TwoOperandCalculate(left, right, operation);
        }

        [HttpGet("RequestStatus/{uniqId}")]
        public IActionResult AsyncProcessingBackgroundWorker(Guid uniqId)
        {
            var result = _asyncReplyRequestService.GetTask(uniqId);
            if (result == null) return BadRequest();
            
            if (result.IsCompleted)
                return Ok();
            if (result.Status == TaskStatus.WaitingForActivation)
                return Accepted("Waiting for result...");

            return BadRequest();


        }


        
    }
}
