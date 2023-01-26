using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebCalcAPI.Contracts.Services;
using WebCalcAPI.Extensions;
using WebCalcAPI.Models;
using WebCalcAPI.Models.Users;

namespace WebCalcAPI.Controllers;

[Route("/api/[controller]")]
public class CalculationController : Controller
{
    private readonly IAsyncReplyRequestService<CalculationResultModel> _asyncReplyRequestService;
    private readonly IAuthenticateService _authenticateService;
    private readonly ICalculationService _calculationService;
    private readonly ILogger<CalculationController> _logger;

    public CalculationController(ICalculationService calculationService,
        IAsyncReplyRequestService<CalculationResultModel> asyncReplyRequestService,
        ILogger<CalculationController> logger, IAuthenticateService authenticateService)
    {
        _calculationService = calculationService;
        _asyncReplyRequestService = asyncReplyRequestService;
        _logger = logger;
        _authenticateService = authenticateService;
    }

    private async Task<object> WaitTill(ComputeModel computeModel)
    {
        return await _calculationService.TwoOperandCalculate(computeModel);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    [Route("calc")]
    public async Task<object> ProcessingWorkAcceptor(ComputeModel computeModel, int timeout)
    {
        var calculationTask = WaitTill(computeModel);

        if (await calculationTask.CanTaskBeCompletedWithinTime(timeout).ConfigureAwait(false))
            return await calculationTask;

        var guid = Guid.NewGuid();
        _logger.LogInformation($"Assigned new GUID: {guid}");
        _asyncReplyRequestService.CreateNewTask(guid, calculationTask);
        Response.StatusCode = 201;
        return guid;
    }

    [HttpGet("{uniqId}")]
    public async Task<IActionResult> ProcessingBackgroundWorker(Guid uniqId)
    {
        var taskStatus = _asyncReplyRequestService.IsTaskReady(uniqId);

        if (taskStatus)
            return Ok(await _asyncReplyRequestService.GetTaskResult(uniqId));
        return Accepted($"The task {uniqId} is working yet");
    }

    [HttpPost]
    [Route("login")]
    public IActionResult Login([FromBody] UserLogin userLogin)
    {
        var user = _authenticateService.Authenticate(userLogin);
        if (user is null) return NotFound("User not found");
        var token = _authenticateService.GenerateJwtToken(user);
        return Ok(token);
    }
}