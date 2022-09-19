using System;
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

        public async Task<T> GetTaskResult(Guid guid)
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
