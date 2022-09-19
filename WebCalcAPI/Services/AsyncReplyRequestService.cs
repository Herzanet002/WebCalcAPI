using WebCalcAPI.Contracts.Services;

namespace WebCalcAPI.Services
{
    public class AsyncReplyRequestService<T> : IAsyncReplyRequestService<T>
    {
        public Dictionary<Guid, Task<T>> TasksContainer { get; set; }
        public AsyncReplyRequestService()
        {
            TasksContainer = new Dictionary<Guid, Task<T>>();
        }

        public void CreateNewTask(Guid guid, Task<T> task)
        {
            TasksContainer.Add(guid, task);
        }

        public void DeleteTask(Guid guid)
        {
            throw new NotImplementedException();
        }

        public Task<T>? GetTask(Guid guid)
        {
            TasksContainer.TryGetValue(guid, out var taskValue);
            return taskValue;
        }
    }
}
