using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using WebCalcAPI.Contracts.Services;
using WebCalcAPI.Models;

namespace WebCalcAPI.Controllers
{
    
    [Route("/api/[controller]")]
    public class CalculationController : Controller
    {
        private readonly ICalculationService _calculationService;
        private readonly IAsyncReplyRequestService<CalculationModel> _asyncReplyRequestService;
        private readonly ILogger<CalculationController> _logger;
        private readonly IConfiguration _configuration;

        public CalculationController(ICalculationService calculationService, IAsyncReplyRequestService<CalculationModel> asyncReplyRequestService, 
            ILogger<CalculationController> logger, IConfiguration configuration)
        {
            _calculationService = calculationService;
            _asyncReplyRequestService = asyncReplyRequestService;
            _logger = logger;
            _configuration = configuration;
        }




        private async Task<CalculationModel> WaitTill(double left, double right, string operation)
        {   
            return await _calculationService.TwoOperandCalculate(left, right, operation);
        }
        //[Authorize(Roles = "Admin")]
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

            if (!calculationResultTask.IsCompleted)
            {
                _logger.LogWarning($"The task {uniqId} is working yet");
                return Accepted($"The task {uniqId} is working yet");
            }

            var status = calculationResultTask.Status;
            if (status == TaskStatus.RanToCompletion) return Ok(await calculationResultTask);

            _logger.LogInformation($"The task {uniqId} was not completed successfully\n Status code: {status}");
            return BadRequest($"The task {uniqId} was not completed successfully\n Status code: {status}");

        }

        [AllowAnonymous]
        [HttpPost, Route("/api/[controller]/login")]
        public IActionResult Login([FromBody] UserLogin userLogin)
        {
            var user = Authenticate(userLogin);
            if (user is null) return NotFound("User not found");
            var token = GenerateJwtToken(user);
            return Ok(token);
        }

        private string GenerateJwtToken(UserModel user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtOptions:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserName),
                new Claim(ClaimTypes.Role, user.Role)
            };
            var token = new JwtSecurityToken(
                issuer: _configuration["JwtOptions:Issuer"],
                audience: _configuration["JwtOptions:Audience"],
                claims: claims,
                expires:DateTime.Now.AddMinutes(15),
                signingCredentials:credentials
            );
            //HttpContext.Session.SetString("Token", token.ToString());
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private UserModel? Authenticate(UserLogin? userLogin)
        {
            var validUsers = _configuration.GetSection("ValidUsers").Get<List<UserModel>>();
            if (userLogin is null) return null;
            return validUsers.FirstOrDefault(user =>
                user.UserName == userLogin.UserName && user.Password == userLogin.Password);
        }
    }
}
