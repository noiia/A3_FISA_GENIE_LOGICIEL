using System.Collections.Concurrent;

namespace Job.Services;

public class ThreadPoolManager
{
    private readonly SemaphoreSlim _semaphore;
    private readonly ConcurrentQueue<Action> _tasks = new();

    public ThreadPoolManager(int maxThreads)
    {
        _semaphore = new SemaphoreSlim(maxThreads);
    }

    public Task<(int, string)> QueueTask(Func<Task<(int, string)>> task)
    {
        var tcs = new TaskCompletionSource<(int, string)>();
        _tasks.Enqueue(async () =>
        {
            try
            {
                var result = await task();
                tcs.SetResult(result);
            }
            catch (Exception ex)
            {
                tcs.SetException(ex);
            }
        });

        Task.Run(ExecuteTask);
        return tcs.Task;
    }

    private async Task ExecuteTask()
    {
        if (_tasks.TryDequeue(out var task))
        {
            await _semaphore.WaitAsync();
            try
            {
                task();
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }
}