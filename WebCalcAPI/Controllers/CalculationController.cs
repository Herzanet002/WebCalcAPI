using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebCalcAPI.Contracts.Services;
using WebCalcAPI.Models;

namespace WebCalcAPI.Controllers
{
    //[Authorize]
    [Route("/api/[controller]")]
    public class CalculationController : Controller
    {
        private readonly ICalculationService _calculationService;
        private readonly IAsyncReplyRequestService<CalculationModel> _asyncReplyRequestService;
        private readonly ILogger<CalculationController> _logger;


        public CalculationController(ICalculationService calculationService, IAsyncReplyRequestService<CalculationModel> asyncReplyRequestService, 
            ILogger<CalculationController> logger)
        {
            _calculationService = calculationService;
            _asyncReplyRequestService = asyncReplyRequestService;
            _logger = logger;
        }

        private async Task<CalculationModel> WaitTill(double left, double right, string operation)
        {
            return await _calculationService.TwoOperandCalculate(left, right, operation);
        }

        [HttpPost("{left} {operation} {right}")]
        public Guid ProcessingWorkAcceptor(double left, double right, string operation)
        {
            var guid = Guid.NewGuid();
            _logger.LogInformation($"Assigned new GUID: {guid}");
            _asyncReplyRequestService.CreateNewTask(guid, WaitTill(left, right, operation));
            Response.StatusCode = 201;
            return guid;
        }

        [HttpGet("{uniqId}")]
        public async Task<IActionResult> ProcessingBackgroundWorker(Guid uniqId)
        {
           
            var calculationResultTask = _asyncReplyRequestService.GetTask(uniqId);
            if (calculationResultTask is null)
            {
                _logger.LogCritical("Calculation Task in null");
                return BadRequest();
            }

            if (calculationResultTask.IsCompleted)
            {
                _logger.LogInformation($"The task {uniqId} was completed successfully");
                return Ok(await calculationResultTask);
            }

            if (calculationResultTask.Status == TaskStatus.WaitingForActivation)
            {
                _logger.LogWarning($"The task {uniqId} is working yet");
                return Accepted("Waiting for result...");
            }
            

            return BadRequest();


        }


        
    }
}
