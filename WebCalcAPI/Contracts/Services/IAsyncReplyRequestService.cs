namespace WebCalcAPI.Contracts.Services
{
    public interface IAsyncReplyRequestService<T>
    {
        void CreateNewTask(Guid guid, Task<object> task);

        TaskStatus GetTaskStatus(Guid guid);

        public Task<object> GetTaskResult(Guid guid);
    }
}