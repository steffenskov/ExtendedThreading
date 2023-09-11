namespace ExtendedThreading;

public class KeyedMutexSynchronizer<TKey>
where TKey : notnull
{
	private struct MutexInfo
	{
		private readonly MutexSlim _mutex;
		private int _referenceCount;

		public MutexInfo()
		{
			_referenceCount = 0;
			_mutex = new();
		}

		public readonly async Task WaitAsync(CancellationToken cancellationToken)
		{
			await _mutex.WaitAsync(cancellationToken);
		}

		public readonly void Wait()
		{
			_mutex.Wait();
		}

		internal void IncrementReferenceCount()
		{
			Interlocked.Increment(ref _referenceCount);
		}

		public int DecrementReferenceCount()
		{
			return Interlocked.Decrement(ref _referenceCount);
		}

		public readonly void Release()
		{
			_mutex.Release();
		}
	}

	private readonly LockedConcurrentDictionary<TKey, MutexInfo> _mutexes = new();
	private readonly object _lock = new();

	public void InvokeSynchronizedAction(TKey key, Action action)
	{
		var mutexInfo = AcquireMutex(key);

		try
		{
			mutexInfo.Wait();
			action();
		}
		finally
		{
			ReleaseMutex(key, mutexInfo);
		}
	}


	public T InvokeSynchronizedFunc<T>(TKey key, Func<T> func)
	{
		var mutexInfo = AcquireMutex(key);

		try
		{
			mutexInfo.Wait();
			return func();
		}
		finally
		{
			ReleaseMutex(key, mutexInfo);
		}
	}

	public async Task InvokeSynchronizedActionAsync(TKey key, Func<Task> action, CancellationToken cancellationToken)
	{
		var mutexInfo = AcquireMutex(key);

		try
		{
			await mutexInfo.WaitAsync(cancellationToken);
			await action();
		}
		finally
		{
			ReleaseMutex(key, mutexInfo);
		}
	}

	public async Task<T> InvokeSynchronizedFuncAsync<T>(TKey key, Func<Task<T>> func, CancellationToken cancellationToken)
	{
		var mutexInfo = AcquireMutex(key);

		try
		{
			await mutexInfo.WaitAsync(cancellationToken);
			return await func();
		}
		finally
		{
			ReleaseMutex(key, mutexInfo);
		}
	}

	private MutexInfo AcquireMutex(TKey key)
	{
		MutexInfo mutexInfo;
		lock (_lock)
		{
			mutexInfo = _mutexes.GetOrAdd(key, _ => new());
			mutexInfo.IncrementReferenceCount();
		}

		return mutexInfo;
	}

	private void ReleaseMutex(TKey key, KeyedMutexSynchronizer<TKey>.MutexInfo mutexInfo)
	{
		lock (_lock)
		{
			mutexInfo.Release();
			var referenceCount = mutexInfo.DecrementReferenceCount();
			if (referenceCount == 0)
			{
				_mutexes.TryRemove(key, out _);
			}
		}
	}
}