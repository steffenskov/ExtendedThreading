namespace ExtendedThreading.UnitTests;

public class MutexSlimTests
{
	[Fact]
	public void Wait_NotTaken_Continues()
	{
		// Arrange
		using var mutex = new MutexSlim();

		// Act
		mutex.Wait();

		// Assert
		Assert.True(true); // Just testing wait didn't block
	}

	[Fact]
	public async Task Wait_Taken_Waits()
	{
		// Arrange
		using var mutex = new MutexSlim();
		var concurrencyCounter = 0;

		var tasks = Enumerable.Range(0, 8).Select(_ => Task.Factory.StartNew(() =>
				{
					mutex.Wait();
					var currentCount = Interlocked.Increment(ref concurrencyCounter);
					Assert.Equal(1, currentCount);
					Thread.Sleep(50);
					Interlocked.Decrement(ref concurrencyCounter);
					mutex.Release();
				}));

		// Act
		await Task.WhenAll(tasks);

		// Assert
		Assert.Equal(0, concurrencyCounter);
	}
}