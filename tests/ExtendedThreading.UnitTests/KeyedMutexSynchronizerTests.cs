namespace ExtendedThreading.UnitTests;

public class KeyedMutexSynchronizerTests
{
	[Fact]
	public async Task InvokeSynchronizedActionAsync_ActionIsSynchronizedByKey()
	{
		// Arrange
		var synchronizer = new KeyedMutexSynchronizer<Guid>();
		var key = Guid.NewGuid();
		var counter = 0;

		// Act
		Task Action()
		{
			return Task.Factory.StartNew(() =>
			{
				synchronizer.InvokeSynchronizedAction(key, () =>
				{
					Thread.Sleep(100); // Simulate work
					counter++;
				});
			});
		}

		async Task ExecuteActions()
		{
			await Task.WhenAll(
				Action(),
				Action(),
				Action()
			);
		}

		// Assert
		await ExecuteActions();
		Assert.Equal(3, counter);
	}

	[Fact]
	public async Task InvokeSynchronizedActionAsync_AsyncActionIsSynchronizedByKey()
	{
		// Arrange
		var synchronizer = new KeyedMutexSynchronizer<Guid>();
		var key = Guid.NewGuid();
		var counter = 0;

		// Act
		async Task AsyncAction(CancellationToken cancellationToken)
		{
			await synchronizer.InvokeSynchronizedActionAsync(key, async () =>
			{
				await Task.Delay(100); // Simulate asynchronous work
				counter++;
			}, cancellationToken);
		}

		async Task ExecuteAsyncActions()
		{
			await Task.WhenAll(
				AsyncAction(CancellationToken.None),
				AsyncAction(CancellationToken.None),
				AsyncAction(CancellationToken.None)
			);
		}

		// Assert
		await ExecuteAsyncActions();
		Assert.Equal(3, counter);
	}
}
