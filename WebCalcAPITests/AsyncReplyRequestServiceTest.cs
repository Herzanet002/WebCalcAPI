using WebCalcAPI.Contracts.Services;
using WebCalcAPI.Models;
using WebCalcAPI.Services;

namespace WebCalcAPITests
{
    public class AsyncReplyRequestServiceTest
    {
        private IAsyncReplyRequestService<CalculationModel> _asyncReplyRequestService = null!;
        private Dictionary<Guid, Task<CalculationModel>> TasksContainer { get; set; }
        [SetUp]
        public void Setup()
        {
            _asyncReplyRequestService = new AsyncReplyRequestService<CalculationModel>();
            TasksContainer = new Dictionary<Guid, Task<CalculationModel>>();
        }


        //[Test]
        //[TestCaseSource(typeof(DataClass), nameof(DataClass.TestCalculationModelCasesInAsyncReplyRequestService))]
        //public void CreateNewTask(Guid guid, Task<CalculationModel> task)
        //{
        //    TasksContainer.Add(guid, task);
        //}
    }
}
