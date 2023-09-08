namespace ExtendedThreading;

public class MutexSlim : IDisposable
{
	private readonly SemaphoreSlim _semaphore = new(1, 1);

	public void Wait()
	{
		_semaphore.Wait();
	}

	public void Wait(CancellationToken cancellationToken)
	{
		_semaphore.Wait(cancellationToken);
	}

	public void Wait(TimeSpan timeout)
	{
		_semaphore.Wait(timeout);
	}

	public void Wait(TimeSpan timeout, CancellationToken cancellationToken)
	{
		_semaphore.Wait(timeout, cancellationToken);
	}

	public async Task WaitAsync()
	{
		await _semaphore.WaitAsync();
	}

	public async Task WaitAsync(CancellationToken cancellationToken)
	{
		await _semaphore.WaitAsync(cancellationToken);
	}

	public async Task WaitAsync(TimeSpan timeout)
	{
		await _semaphore.WaitAsync(timeout);
	}

	public async Task WaitAsync(TimeSpan timeout, CancellationToken cancellationToken)
	{
		await _semaphore.WaitAsync(timeout, cancellationToken);
	}

	public void Release()
	{
		_semaphore.Release();
	}

	public void Dispose()
	{
		_semaphore.Dispose();
	}
}