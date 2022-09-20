using WebCalcAPI.Contracts.Services;
using WebCalcAPI.Models;
using WebCalcAPI.Services;

namespace WebCalcAPITests
{
    public class AsyncReplyRequestServiceTest
    {
        private IAsyncReplyRequestService<CalculationResultModel> _asyncReplyRequestService = null!;
        private Dictionary<Guid, Task<CalculationResultModel>> TasksContainer { get; set; }

        [SetUp]
        public void Setup()
        {
            _asyncReplyRequestService = new AsyncReplyRequestService<CalculationResultModel>();
            TasksContainer = new Dictionary<Guid, Task<CalculationResultModel>>();
        }

        //[Test]
        //[TestCaseSource(typeof(DataClass), nameof(DataClass.TestCalculationModelCasesInAsyncReplyRequestService))]
        //public void CreateNewTask(Guid guid, Task<CalculationResultModel> task)
        //{
        //    TasksContainer.Add(guid, task);
        //}
    }
}