using System.Collections;
using System.Reflection;

namespace ExtendedThreading.UnitTests;

public class AsyncSignalTests
{
	[Fact]
	public void Pulse_NoWaiters_DoesNotThrow()
	{
		// Arrange
		var signal = new AsyncSignal();

		// Act
		var exception = Record.Exception(signal.Pulse);

		// Assert
		Assert.Null(exception);
	}

	[Fact]
	public void PulseAll_NoWaiters_DoesNotThrow()
	{
		// Arrange
		var signal = new AsyncSignal();

		// Act
		var exception = Record.Exception(signal.PulseAll);

		// Assert
		Assert.Null(exception);
	}

	[Fact]
	public async Task WaitAsync_ParameterlessPulsedBeforeCompletion_CompletesSuccessfully()
	{
		// Arrange
		var signal = new AsyncSignal();

		// Act
		var waitTask = signal.WaitAsync(TestContext.Current.CancellationToken);
		await WaitUntilWaiterRegisteredAsync(signal);
		signal.Pulse();
		var completedTask = await Task.WhenAny(waitTask, Task.Delay(TimeSpan.FromSeconds(5), TestContext.Current.CancellationToken));

		// Assert
		Assert.Same(waitTask, completedTask);
	}

	[Fact]
	public async Task WaitAsync_MillisecondsTimeoutPulsedBeforeTimeout_ReturnsTrue()
	{
		// Arrange
		var signal = new AsyncSignal();
		var waitTask = signal.WaitAsync(5000, TestContext.Current.CancellationToken);
		await WaitUntilWaiterRegisteredAsync(signal);

		// Act
		signal.Pulse();
		var result = await waitTask;

		// Assert
		Assert.True(result);
	}

	[Fact]
	public async Task WaitAsync_MillisecondsTimeoutElapses_ReturnsFalse()
	{
		// Arrange
		var signal = new AsyncSignal();

		// Act
		var result = await signal.WaitAsync(50, TestContext.Current.CancellationToken);

		// Assert
		Assert.False(result);
	}

	[Fact]
	public async Task WaitAsync_TimeSpanTimeoutPulsedBeforeTimeout_ReturnsTrue()
	{
		// Arrange
		var signal = new AsyncSignal();
		var waitTask = signal.WaitAsync(TimeSpan.FromSeconds(5), TestContext.Current.CancellationToken);
		await WaitUntilWaiterRegisteredAsync(signal);

		// Act
		signal.Pulse();
		var result = await waitTask;

		// Assert
		Assert.True(result);
	}

	[Fact]
	public async Task WaitAsync_TimeSpanTimeoutElapses_ReturnsFalse()
	{
		// Arrange
		var signal = new AsyncSignal();

		// Act
		var result = await signal.WaitAsync(TimeSpan.FromMilliseconds(50), TestContext.Current.CancellationToken);

		// Assert
		Assert.False(result);
	}

	[Fact]
	public async Task WaitAsync_ExternalTokenCancelledBeforeTimeout_ThrowsOperationCanceledException()
	{
		// Arrange
		var signal = new AsyncSignal();
		using var cts = new CancellationTokenSource();

		// Act
		var waitTask = signal.WaitAsync(TimeSpan.FromSeconds(5), cts.Token);
		await WaitUntilWaiterRegisteredAsync(signal);
		await cts.CancelAsync();

		// Assert
		await Assert.ThrowsAsync<OperationCanceledException>(() => waitTask);
	}

	[Fact]
	public async Task WaitAsync_ExternalTokenAlreadyCancelled_ThrowsOperationCanceledException()
	{
		// Arrange
		var signal = new AsyncSignal();
		using var cts = new CancellationTokenSource();
		await cts.CancelAsync();

		// Act & Assert
		await Assert.ThrowsAsync<OperationCanceledException>(() => signal.WaitAsync(TimeSpan.FromSeconds(5), cts.Token));
	}

	[Fact]
	public async Task Pulse_SingleWaiter_WakesThatWaiter()
	{
		// Arrange
		var signal = new AsyncSignal();
		var waitTask = signal.WaitAsync(TimeSpan.FromSeconds(5), TestContext.Current.CancellationToken);
		await WaitUntilWaiterRegisteredAsync(signal);

		// Act
		signal.Pulse();
		var result = await waitTask;

		// Assert
		Assert.True(result);
	}

	[Fact]
	public async Task Pulse_MultipleWaiters_WakesExactlyOne()
	{
		// Arrange
		var signal = new AsyncSignal();
		var waitTask1 = signal.WaitAsync(TimeSpan.FromSeconds(5), TestContext.Current.CancellationToken);
		var waitTask2 = signal.WaitAsync(TimeSpan.FromSeconds(5), TestContext.Current.CancellationToken);
		await WaitUntilWaiterRegisteredAsync(signal, 2);

		// Act
		signal.Pulse();
		var firstCompleted = await Task.WhenAny(waitTask1, waitTask2);
		var stillPending = firstCompleted == waitTask1 ? waitTask2 : waitTask1;

		// Assert
		Assert.True(await firstCompleted);
		Assert.False(stillPending.IsCompleted);
	}

	[Fact]
	public async Task PulseAll_MultipleWaiters_WakesAllOfThem()
	{
		// Arrange
		var signal = new AsyncSignal();
		var waitTask1 = signal.WaitAsync(TimeSpan.FromSeconds(5), TestContext.Current.CancellationToken);
		var waitTask2 = signal.WaitAsync(TimeSpan.FromSeconds(5), TestContext.Current.CancellationToken);
		var waitTask3 = signal.WaitAsync(TimeSpan.FromSeconds(5), TestContext.Current.CancellationToken);
		await WaitUntilWaiterRegisteredAsync(signal, 3);

		// Act
		signal.PulseAll();
		var results = await Task.WhenAll(waitTask1, waitTask2, waitTask3);

		// Assert
		Assert.All(results, Assert.True);
	}

	[Fact]
	public async Task WaitAsync_TimedOutWaiterFollowedByPulse_DoesNotConsumeStaleWaiter()
	{
		// Arrange: first waiter times out and should be pruned from the internal list,
		// so a later Pulse() must reach the second, still-live waiter instead of
		// silently completing against the already-cancelled first one.
		var signal = new AsyncSignal();
		var timedOutTask = signal.WaitAsync(50, TestContext.Current.CancellationToken);
		await WaitUntilWaiterRegisteredAsync(signal);
		var timedOutResult = await timedOutTask;

		var liveWaitTask = signal.WaitAsync(TimeSpan.FromSeconds(5), TestContext.Current.CancellationToken);
		await WaitUntilWaiterRegisteredAsync(signal);

		// Act
		signal.Pulse();
		var liveResult = await liveWaitTask;

		// Assert
		Assert.False(timedOutResult);
		Assert.True(liveResult);
	}

	// Polls the internal waiter count via reflection so tests don't race the
	// lock(_lock) { _waiters.Add(tcs); } line inside WaitAsyncCore.
	private static async Task WaitUntilWaiterRegisteredAsync(AsyncSignal signal, int expectedCount = 1)
	{
		var field = typeof(AsyncSignal)
			.GetField("_waiters", BindingFlags.NonPublic | BindingFlags.Instance)!;

		for (var i = 0; i < 100; i++)
		{
			var waiters = (IList)field.GetValue(signal)!;
			if (waiters.Count >= expectedCount)
			{
				return;
			}

			await Task.Delay(10);
		}

		throw new TimeoutException("Waiter was not registered in time.");
	}
}