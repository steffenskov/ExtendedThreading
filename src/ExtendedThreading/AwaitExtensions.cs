using System.Runtime.CompilerServices;

namespace ExtendedThreading;

public static class AwaitExtensions
{
	#region 2 arguments

	public static TaskAwaiter<(T1, T2)> GetAwaiter<T1, T2>(this (Task<T1>, Task<T2>) tasks)
	{
		return WrapTasks(tasks).GetAwaiter();
	}

	private static async Task<(T1, T2)> WrapTasks<T1, T2>(this (Task<T1>, Task<T2>) tasks)
	{
		await TaskExtension.WhenAll(tasks.Item1, tasks.Item2);
		return (tasks.Item1.Result, tasks.Item2.Result);
	}

	#endregion

	#region 3 arguments

	public static TaskAwaiter<(T1, T2, T3)> GetAwaiter<T1, T2, T3>(this (Task<T1>, Task<T2>, Task<T3>) tasks)
	{
		return WrapTasks(tasks).GetAwaiter();
	}

	private static async Task<(T1, T2, T3)> WrapTasks<T1, T2, T3>(this (Task<T1>, Task<T2>, Task<T3>) tasks)
	{
		await TaskExtension.WhenAll(tasks.Item1, tasks.Item2);
		return (tasks.Item1.Result, tasks.Item2.Result, tasks.Item3.Result);
	}

	#endregion

	#region 4 arguments

	public static TaskAwaiter<(T1, T2, T3, T4)> GetAwaiter<T1, T2, T3, T4>(this (Task<T1>, Task<T2>, Task<T3>, Task<T4>) tasks)
	{
		return WrapTasks(tasks).GetAwaiter();
	}

	private static async Task<(T1, T2, T3, T4)> WrapTasks<T1, T2, T3, T4>(this (Task<T1>, Task<T2>, Task<T3>, Task<T4>) tasks)
	{
		await TaskExtension.WhenAll(tasks.Item1, tasks.Item2);
		return (tasks.Item1.Result, tasks.Item2.Result, tasks.Item3.Result, tasks.Item4.Result);
	}

	#endregion

	#region 5 arguments

	public static TaskAwaiter<(T1, T2, T3, T4, T5)> GetAwaiter<T1, T2, T3, T4, T5>(this (Task<T1>, Task<T2>, Task<T3>, Task<T4>, Task<T5>) tasks)
	{
		return WrapTasks(tasks).GetAwaiter();
	}

	private static async Task<(T1, T2, T3, T4, T5)> WrapTasks<T1, T2, T3, T4, T5>(this (Task<T1>, Task<T2>, Task<T3>, Task<T4>, Task<T5>) tasks)
	{
		await TaskExtension.WhenAll(tasks.Item1, tasks.Item2);
		return (tasks.Item1.Result, tasks.Item2.Result, tasks.Item3.Result, tasks.Item4.Result, tasks.Item5.Result);
	}

	#endregion

	#region 6 arguments

	public static TaskAwaiter<(T1, T2, T3, T4, T5, T6)> GetAwaiter<T1, T2, T3, T4, T5, T6>(this (Task<T1>, Task<T2>, Task<T3>, Task<T4>, Task<T5>, Task<T6>) tasks)
	{
		return WrapTasks(tasks).GetAwaiter();
	}

	private static async Task<(T1, T2, T3, T4, T5, T6)> WrapTasks<T1, T2, T3, T4, T5, T6>(this (Task<T1>, Task<T2>, Task<T3>, Task<T4>, Task<T5>, Task<T6>) tasks)
	{
		await TaskExtension.WhenAll(tasks.Item1, tasks.Item2);
		return (tasks.Item1.Result, tasks.Item2.Result, tasks.Item3.Result, tasks.Item4.Result, tasks.Item5.Result, tasks.Item6.Result);
	}

	#endregion

	#region 7 arguments

	public static TaskAwaiter<(T1, T2, T3, T4, T5, T6, T7)> GetAwaiter<T1, T2, T3, T4, T5, T6, T7>(
		this (Task<T1>, Task<T2>, Task<T3>, Task<T4>, Task<T5>, Task<T6>, Task<T7>) tasks)
	{
		return WrapTasks(tasks).GetAwaiter();
	}

	private static async Task<(T1, T2, T3, T4, T5, T6, T7)> WrapTasks<T1, T2, T3, T4, T5, T6, T7>(this (Task<T1>, Task<T2>, Task<T3>, Task<T4>, Task<T5>, Task<T6>, Task<T7>) tasks)
	{
		await TaskExtension.WhenAll(tasks.Item1, tasks.Item2);
		return (tasks.Item1.Result, tasks.Item2.Result, tasks.Item3.Result, tasks.Item4.Result, tasks.Item5.Result, tasks.Item6.Result, tasks.Item7.Result);
	}

	#endregion

	#region 8 arguments

	public static TaskAwaiter<(T1, T2, T3, T4, T5, T6, T7, T8)> GetAwaiter<T1, T2, T3, T4, T5, T6, T7, T8>(
		this (Task<T1>, Task<T2>, Task<T3>, Task<T4>, Task<T5>, Task<T6>, Task<T7>, Task<T8>) tasks)
	{
		return WrapTasks(tasks).GetAwaiter();
	}

	private static async Task<(T1, T2, T3, T4, T5, T6, T7, T8)> WrapTasks<T1, T2, T3, T4, T5, T6, T7, T8>(
		this (Task<T1>, Task<T2>, Task<T3>, Task<T4>, Task<T5>, Task<T6>, Task<T7>, Task<T8>) tasks)
	{
		await TaskExtension.WhenAll(tasks.Item1, tasks.Item2);
		return (tasks.Item1.Result, tasks.Item2.Result, tasks.Item3.Result, tasks.Item4.Result, tasks.Item5.Result, tasks.Item6.Result, tasks.Item7.Result, tasks.Item8.Result);
	}

	#endregion
}