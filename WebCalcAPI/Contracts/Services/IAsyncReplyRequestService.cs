namespace WebCalcAPI.Contracts.Services
{
    public interface IAsyncReplyRequestService<T>
    {
        void CreateNewTask(Guid guid, Task<object> task);

        bool IsTaskReady(Guid guid);

        Task<object> GetTaskResult(Guid guid);
    }
}