namespace Sundroid.Homework.IntegrationTests.Infrastructure;

public static class ParallelTestHelper
{
    /// <summary>
    /// This semaphore is used for limiting the number of concurrently running docker-based tests,
    /// because too many concurrent docker-based tests fail with timeout exceptions.
    /// </summary>
    public static readonly SemaphoreSlim ConcurrentDockerContainersSemaphore = new(initialCount: 4);
}