namespace WebCalcAPI.Extensions;

public static class TaskExtension
{
    public static async Task<bool> CanTaskBeCompletedWithinTime<T>(this Task<T> task, int timeout)
    {
        using var timeoutCancellationTokenSource = new CancellationTokenSource();
        var completedTask = await Task.WhenAny(task,
            Task.Delay(TimeSpan.FromMilliseconds(timeout), timeoutCancellationTokenSource.Token));
        if (completedTask == task)
            timeoutCancellationTokenSource.Cancel();

        return completedTask == task;
    }
}