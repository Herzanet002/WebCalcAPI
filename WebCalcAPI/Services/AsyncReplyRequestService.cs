using System.Xml;
using WebCalcAPI.Contracts.Services;

namespace WebCalcAPI.Services
{
    public class AsyncReplyRequestService: IAsyncReplyRequestService
    {
        public Dictionary<Guid, Task> TasksContainer { get; set; }
        public AsyncReplyRequestService()
        {
            TasksContainer = new();
        }

        public void CreateNewTask(Guid guid, Task task)
        {
            TasksContainer.Add(guid, task);
        }

        public void DeleteTask(Guid guid)
        {
            throw new NotImplementedException();
        }

        public Task? GetTask(Guid guid)
        {
            TasksContainer.TryGetValue(guid, out var taskValue);
            return taskValue;
        }
    }
}
