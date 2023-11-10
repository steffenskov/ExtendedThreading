using System.Diagnostics;

namespace ExtendedThreading;

public class TaskExtension
{
	public static async Task<IEnumerable<T>> WhenAll<T>(params Task<T>[] tasks)
	{
		return await WhenAll<T>((IEnumerable<Task<T>>)tasks);
	}
	
	public static async Task<IEnumerable<T>> WhenAll<T>(IEnumerable<Task<T>> tasks)
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
		await WhenAll((IEnumerable<Task>)tasks);
	}
	
	public static async Task WhenAll( IEnumerable<Task> tasks)
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