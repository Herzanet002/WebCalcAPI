namespace WebCalcAPI.Contracts.Services
{
    public interface IAsyncReplyRequestService
    {
        Dictionary<Guid, Task> TasksContainer { get; set; }
        void CreateNewTask(Guid guid, Task task);
        void DeleteTask(Guid guid);

        Task? GetTask(Guid guid);
    }
}
