namespace ExtendedThreading.UnitTests;

public class TaskExtensionTests
{
	[Fact]
	public async Task WhenAll_NoExceptions_ReturnsResults()
	{
		// Arrange
		var task1 = EchoAsync("Hello");
		var task2 = EchoAsync("World");

		// Act
		var result = (await TaskExtension.WhenAll(task1, task2)).ToList();

		// Arrange
		Assert.Equal("Hello", result[0]);
		Assert.Equal("World", result[1]);
	}

	[Fact]
	public async Task BuiltInWhenAll_Throws_ThrowsFirstExceptionOnly() // This is the reason behind having TaskExtension
	{
		// Arrange
		var task1 = EchoAsync(null!);
		var task2 = EchoAsync(null!);

		// Act && Assert
		await Assert.ThrowsAsync<ArgumentNullException>(async () => await Task.WhenAll(task1, task2));
	}

	[Fact]
	public async Task WhenAll_Throws_ThrowsAggregateException()
	{
		// Arrange
		var task1 = EchoAsync(null!);
		var task2 = EchoAsync(null!);

		// Act && Assert
		var ex = await Assert.ThrowsAsync<AggregateException>(async () => await TaskExtension.WhenAll(task1, task2));
		Assert.Equal(2, ex.InnerExceptions.Count);
	}

	[Fact]
	public async Task WhenAll_MixedTypesThrows_ThrowsAggregateException()
	{
		// Arrange
		var task1 = EchoAsync(null!);
		var task2 = EchoAsync(0);

		// Act && Assert
		var ex = await Assert.ThrowsAsync<AggregateException>(async () => await TaskExtension.WhenAll(task1, task2));
		Assert.Equal(2, ex.InnerExceptions.Count);
	}

	private static async Task<string> EchoAsync(string message)
	{
		await Task.CompletedTask;
		if (message is null)
		{
			throw new ArgumentNullException(nameof(message));
		}

		return message;
	}

	private static async Task<int> EchoAsync(int value)
	{
		await Task.CompletedTask;
		if (value == 0)
		{
			throw new ArgumentOutOfRangeException(nameof(value));
		}

		return value;
	}
}