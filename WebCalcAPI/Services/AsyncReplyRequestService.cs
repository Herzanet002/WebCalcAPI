using WebCalcAPI.Contracts.Services;

namespace WebCalcAPI.Services
{
    public class AsyncReplyRequestService<T> : IAsyncReplyRequestService<T>
    {
        public Dictionary<Guid, Task<object>> TasksContainer { get; set; }

        public AsyncReplyRequestService()
        {
            TasksContainer = new Dictionary<Guid, Task<object>>();
        }

        public void CreateNewTask(Guid guid, Task<object> task)
        {
            TasksContainer.Add(guid, task);
        }

        public async Task<object> GetTaskResult(Guid guid)
        {
            TasksContainer.TryGetValue(guid, out var taskValue);
            return await taskValue!;
        }

        public TaskStatus GetTaskStatus(Guid guid)
        {
            TasksContainer.TryGetValue(guid, out var taskValue);
            return taskValue?.Status ?? TaskStatus.Faulted;
        }
    }
}