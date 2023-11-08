using System.Diagnostics;

namespace ExtendedThreading;

public class TaskExtension
{
	public static async Task<IEnumerable<T>> WhenAll<T>(params Task<T>[] tasks)
	{
		var allTasks = Task.WhenAll(tasks);
		try
		{
			return await allTasks;
		}
		catch
		{
			// Ignore as this would throw first exception only			
		}

		throw allTasks.Exception ?? throw new UnreachableException("Should never be reached");
	}

	public static async Task WhenAll(params Task[] tasks)
	{
		var allTasks = Task.WhenAll(tasks);
		try
		{
			await allTasks;
			return;
		}
		catch
		{
			// Ignore as this would throw first exception only			
		}

		throw allTasks.Exception ?? throw new UnreachableException("Should never be reached");
	}
}