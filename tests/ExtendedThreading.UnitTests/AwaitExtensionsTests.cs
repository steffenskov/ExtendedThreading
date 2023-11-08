namespace ExtendedThreading.UnitTests;

public class AwaitExtensionsTests
{
	[Fact]
	public async Task Await_TwoTasks_ReturnsBothValues()
	{
		// Arrange
		var task1 = EchoAsync("Hello");
		var task2 = EchoAsync("World");

		// Act
		var (result1, result2) = await (task1, task2);

		// Assert
		Assert.Equal("Hello", result1);
		Assert.Equal("World", result2);
	}

	[Fact]
	public async Task Await_ThreeTasks_ReturnsAllValues()
	{
		// Arrange
		var task1 = EchoAsync("Hello");
		var task2 = EchoAsync("World");
		var task3 = EchoAsync("!!!");

		// Act
		var (result1, result2, result3) = await (task1, task2, task3);

		// Assert
		Assert.Equal("Hello", result1);
		Assert.Equal("World", result2);
		Assert.Equal("!!!", result3);
	}

	[Fact]
	public async Task Await_FourTasks_ReturnsAllValues()
	{
		// Arrange
		var task1 = EchoAsync("Hello");
		var task2 = EchoAsync("World");
		var task3 = EchoAsync("!!!");
		var task4 = EchoAsync("How");

		// Act
		var (result1, result2, result3, result4) = await (task1, task2, task3, task4);

		// Assert
		Assert.Equal("Hello", result1);
		Assert.Equal("World", result2);
		Assert.Equal("!!!", result3);
		Assert.Equal("How", result4);
	}

	[Fact]
	public async Task Await_FiveTasks_ReturnsAllValues()
	{
		// Arrange
		var task1 = EchoAsync("Hello");
		var task2 = EchoAsync("World");
		var task3 = EchoAsync("!!!");
		var task4 = EchoAsync("How");
		var task5 = EchoAsync("Are");

		// Act
		var (result1, result2, result3, result4, result5) = await (task1, task2, task3, task4, task5);

		// Assert
		Assert.Equal("Hello", result1);
		Assert.Equal("World", result2);
		Assert.Equal("!!!", result3);
		Assert.Equal("How", result4);
		Assert.Equal("Are", result5);
	}

	[Fact]
	public async Task Await_SixTasks_ReturnsAllValues()
	{
		// Arrange
		var task1 = EchoAsync("Hello");
		var task2 = EchoAsync("World");
		var task3 = EchoAsync("!!!");
		var task4 = EchoAsync("How");
		var task5 = EchoAsync("Are");
		var task6 = EchoAsync("You");

		// Act
		var (result1, result2, result3, result4, result5, result6) = await (task1, task2, task3, task4, task5, task6);

		// Assert
		Assert.Equal("Hello", result1);
		Assert.Equal("World", result2);
		Assert.Equal("!!!", result3);
		Assert.Equal("How", result4);
		Assert.Equal("Are", result5);
		Assert.Equal("You", result6);
	}

	[Fact]
	public async Task Await_SevenTasks_ReturnsAllValues()
	{
		// Arrange
		var task1 = EchoAsync("Hello");
		var task2 = EchoAsync("World");
		var task3 = EchoAsync("!!!");
		var task4 = EchoAsync("How");
		var task5 = EchoAsync("Are");
		var task6 = EchoAsync("You");
		var task7 = EchoAsync("Today");

		// Act
		var (result1, result2, result3, result4, result5, result6, result7) = await (task1, task2, task3, task4, task5, task6, task7);

		// Assert
		Assert.Equal("Hello", result1);
		Assert.Equal("World", result2);
		Assert.Equal("!!!", result3);
		Assert.Equal("How", result4);
		Assert.Equal("Are", result5);
		Assert.Equal("You", result6);
		Assert.Equal("Today", result7);
	}

	[Fact]
	public async Task Await_EightTasks_ReturnsAllValues()
	{
		// Arrange
		var task1 = EchoAsync("Hello");
		var task2 = EchoAsync("World");
		var task3 = EchoAsync("!!!");
		var task4 = EchoAsync("How");
		var task5 = EchoAsync("Are");
		var task6 = EchoAsync("You");
		var task7 = EchoAsync("Today");
		var task8 = EchoAsync("?");

		// Act
		var (result1, result2, result3, result4, result5, result6, result7, result8) = await (task1, task2, task3, task4, task5, task6, task7, task8);

		// Assert
		Assert.Equal("Hello", result1);
		Assert.Equal("World", result2);
		Assert.Equal("!!!", result3);
		Assert.Equal("How", result4);
		Assert.Equal("Are", result5);
		Assert.Equal("You", result6);
		Assert.Equal("Today", result7);
		Assert.Equal("?", result8);
	}

	[Fact]
	public async Task Await_MixedResultTypes_ReturnsProperTypes()
	{
		// Arrange
		var guid = Guid.NewGuid();
		var task1 = EchoAsync("Hello world");
		var task2 = EchoAsync(42);
		var task3 = EchoAsync(guid);

		// Act
		var result = await (task1, task2, task3);

		// Assert
		Assert.Equal("Hello world", result.Item1);
		Assert.Equal(42, result.Item2);
		Assert.Equal(guid, result.Item3);
	}

	private static async Task<string> EchoAsync(string message)
	{
		await Task.CompletedTask;
		return message;
	}

	private static async Task<int> EchoAsync(int value)
	{
		await Task.CompletedTask;
		return value;
	}

	private static async Task<Guid> EchoAsync(Guid value)
	{
		await Task.CompletedTask;
		return value;
	}
}