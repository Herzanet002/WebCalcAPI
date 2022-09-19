namespace WebCalcAPI.Contracts.Services
{
    public interface IAsyncReplyRequestService<T>
    {
        Dictionary<Guid, Task<T>> TasksContainer { get; set; }
        void CreateNewTask(Guid guid, Task<T> task);
        void DeleteTask(Guid guid);

        Task<T>? GetTask(Guid guid);
    }
}
