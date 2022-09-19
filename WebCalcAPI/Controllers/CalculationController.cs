using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebCalcAPI.Contracts.Services;
using WebCalcAPI.Models;
using WebCalcAPI.Models.Users;

namespace WebCalcAPI.Controllers
{
    [Route("/api/[controller]")]
    public class CalculationController : Controller
    {
        private readonly ICalculationService _calculationService;
        private readonly IAsyncReplyRequestService<CalculationModel> _asyncReplyRequestService;
        private readonly ILogger<CalculationController> _logger;
        private readonly IAuthenticateService _authenticateService;

        public CalculationController(ICalculationService calculationService,
            IAsyncReplyRequestService<CalculationModel> asyncReplyRequestService,
            ILogger<CalculationController> logger, IAuthenticateService authenticateService)
        {
            _calculationService = calculationService;
            _asyncReplyRequestService = asyncReplyRequestService;
            _logger = logger;
            _authenticateService = authenticateService;
        }

        private async Task<object> WaitTill(double left, double right, string operation)
        {
            return await _calculationService.TwoOperandCalculate(left, right, operation);
        }

        private async Task<object> ParamWait(string param)
        {
            await Task.Delay(15000);
            return param + "HELLO";
        }

        [HttpPost("{param1}")]
        public Guid Acceptor(string param)
        {
            var guid = Guid.NewGuid();
            _logger.LogInformation($"Assigned new GUID: {guid}");
            _asyncReplyRequestService.CreateNewTask(guid, ParamWait(param));
            Response.StatusCode = 201;
            return guid;
        }

        [Authorize(Roles = "Admin")]
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
            var taskStatus = _asyncReplyRequestService.GetTaskStatus(uniqId);

            switch (taskStatus)
            {
                case TaskStatus.WaitingForActivation:
                    _logger.LogWarning($"The task {uniqId} is working yet");
                    return Accepted($"The task {uniqId} is working yet");

                case TaskStatus.RanToCompletion:
                    return Ok(await _asyncReplyRequestService.GetTaskResult(uniqId));

                default:
                    _logger.LogInformation($"The task {uniqId} was not completed \n Status code: {taskStatus}");
                    return BadRequest($"The task {uniqId} was not completed \n Status code: {taskStatus}");
            }
        }

        [HttpPost, Route("/api/[controller]/login")]
        public IActionResult Login([FromBody] UserLogin userLogin)
        {
            var user = _authenticateService.Authenticate(userLogin);
            if (user is null) return NotFound("User not found");
            var token = _authenticateService.GenerateJwtToken(user);
            return Ok(token);
        }
    }
}