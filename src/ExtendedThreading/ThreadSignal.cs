namespace ExtendedThreading;
public class ThreadSignal
{
	private object _lockObject;

	public ThreadSignal()
	{
		_lockObject = new object();
	}

	public void Pulse()
	{
		Monitor.Enter(_lockObject);
		Monitor.Pulse(_lockObject);
		Monitor.Exit(_lockObject);
	}

	public void PulseAll()
	{
		Monitor.Enter(_lockObject);
		Monitor.PulseAll(_lockObject);
		Monitor.Exit(_lockObject);
	}

	public void Wait()
	{
		Monitor.Enter(_lockObject);
		Monitor.Wait(_lockObject);
		Monitor.Exit(_lockObject);
	}

	public void Wait(int timeout)
	{
		Monitor.Enter(_lockObject);
		Monitor.Wait(_lockObject, timeout);
		Monitor.Exit(_lockObject);
	}

	public void Wait(TimeSpan timeout)
	{
		Monitor.Enter(_lockObject);
		Monitor.Wait(_lockObject, timeout);
		Monitor.Exit(_lockObject);
	}
}
