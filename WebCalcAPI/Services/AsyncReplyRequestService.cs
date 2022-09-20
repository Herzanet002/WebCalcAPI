using WebCalcAPI.Contracts.Services;

namespace WebCalcAPI.Services
{
    public class AsyncReplyRequestService<T> : IAsyncReplyRequestService<T>
    {
        private Dictionary<Guid, Task<object>> TasksContainer { get; }

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

        public bool IsTaskReady(Guid guid)
        {
            TasksContainer.TryGetValue(guid, out var taskValue);
            return taskValue is { IsCompleted: true };
        }
    }
}