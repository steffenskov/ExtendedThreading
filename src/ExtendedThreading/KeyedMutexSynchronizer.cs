namespace ExtendedThreading;

public class KeyedMutexSynchronizer<TKey>
where TKey : notnull
{
	private readonly LockedConcurrentDictionary<TKey, MutexSlim> _mutexes = new();
	private readonly LockedConcurrentDictionary<TKey, int> _referenceCounts = new();
	private readonly object _lock = new();

	public async Task InvokeSynchronizedActionAsync(TKey key, Action action, CancellationToken cancellationToken)
	{
		MutexSlim mutex;
		lock (_lock)
		{
			mutex = _mutexes.GetOrAdd(key, _ => new MutexSlim());
			var referenceCount = _referenceCounts.GetOrAdd(key, _ => 0);
			_referenceCounts[key] = referenceCount + 1;
		}
		try
		{
			await mutex.WaitAsync(cancellationToken);
			action();
		}
		finally
		{
			lock (_lock)
			{
				mutex.Release();
				var referenceCount = _referenceCounts.GetOrAdd(key, _ => 1) - 1;
				if (referenceCount == 0)
				{

				}
				else
					_referenceCounts[key] = referenceCount;
			}
		}
	}

	public async Task InvokeSynchronizedActionAsync(TKey key, Func<Task> action, CancellationToken cancellationToken)
	{
		MutexSlim mutex;
		lock (_lock)
		{
			mutex = _mutexes.GetOrAdd(key, _ => new MutexSlim());
			var referenceCount = _referenceCounts.GetOrAdd(key, _ => 0);
			_referenceCounts[key] = referenceCount + 1;
		}
		try
		{
			await mutex.WaitAsync(cancellationToken);
			await action();
		}
		finally
		{
			lock (_lock)
			{
				mutex.Release();
				var referenceCount = _referenceCounts.GetOrAdd(key, _ => 1) - 1;
				if (referenceCount == 0)
				{
					_mutexes.TryRemove(key, out _);
					_referenceCounts.TryRemove(key, out _);
				}
				else
				{
					_referenceCounts[key] = referenceCount;
				}
			}
		}
	}
}