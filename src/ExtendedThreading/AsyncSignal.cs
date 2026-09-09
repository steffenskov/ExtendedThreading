namespace ExtendedThreading;

public class AsyncSignal
{
	private readonly object _lock = new();
	private readonly List<TaskCompletionSource<bool>> _waiters = [];

	public void Pulse()
	{
		lock (_lock)
		{
			if (_waiters.Count == 0)
			{
				return;
			}

			var completionSource = _waiters[0];
			_waiters.RemoveAt(0);

			completionSource.TrySetResult(true);
		}
	}

	public void PulseAll()
	{
		lock (_lock)
		{
			foreach (var completionSource in _waiters)
			{
				completionSource.TrySetResult(true);
			}

			_waiters.Clear();
		}
	}

	public Task WaitAsync(CancellationToken cancellationToken = default)
	{
		return WaitAsync(Timeout.InfiniteTimeSpan, cancellationToken);
	}

	public Task<bool> WaitAsync(int millisecondsTimeout, CancellationToken cancellationToken = default)
	{
		return WaitAsync(TimeSpan.FromMilliseconds(millisecondsTimeout), cancellationToken);
	}

	public async Task<bool> WaitAsync(TimeSpan timeout, CancellationToken cancellationToken = default)
	{
		var completionSource = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
		lock (_lock)
		{
			_waiters.Add(completionSource);
		}

		using var timeoutSource = timeout == Timeout.InfiniteTimeSpan ? null : new CancellationTokenSource(timeout);
		using var linkedSource = timeoutSource is null
			? null
			: CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, timeoutSource.Token);
		var token = linkedSource?.Token ?? cancellationToken;

		await using var registration = token.Register(static state =>
			((TaskCompletionSource<bool>)state!).TrySetCanceled(), completionSource);

		try
		{
			return await completionSource.Task.ConfigureAwait(false);
		}
		catch (TaskCanceledException)
		{
			cancellationToken.ThrowIfCancellationRequested(); // real cancellation → rethrow
			return false; // otherwise just timed out
		}
		finally
		{
			lock (_lock)
			{
				_waiters.Remove(completionSource);
			}
		}
	}
}