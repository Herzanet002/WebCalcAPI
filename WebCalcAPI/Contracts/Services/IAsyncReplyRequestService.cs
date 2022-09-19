namespace WebCalcAPI.Contracts.Services
{
    public interface IAsyncReplyRequestService<T>
    {
        void CreateNewTask(Guid guid, Task<T> task);
        TaskStatus GetTaskStatus(Guid guid);
        public Task<T> GetTaskResult(Guid guid);
    }
}
