# ExtendedThreading

This package provides extended Threading functionality, built on top of the built-in Threading capabilities of .Net.

# Installation

I recommend using the NuGet package: [ExtendedThreading](https://www.nuget.org/packages/ExtendedThreading) however feel free to clone the source instead if that suits your needs
better.

# Usage

## ThreadSignal

This is used to simplify signalling between threads, e.g. when building the Producer/Consumer pattern:

```
public class ProducerConsumer<T>
{
	private ThreadSignal _signal = new();
	
	public void Produce(T item){
		// produce
		_signal.Pulse(); // Inform consumers that a new item is available
	}

	public void Consume()
	{
		_signal.Wait(); // Will block until an item becomes available
		// consume
	}
}
```

## KeyedMutexSynchronizer

This is used to ensure mutual exclusion based on keys. E.g. for an API where you want to grant only a single thread access to do PUT requests on a per-id basis to prevent race
conditions on a per entity basis:

```
public class OrderController
{
	private readonly KeyedMutexSynchronizer<OrderId> _synchronizer;
	private readonly IOrderService _orderService;

	public OrderController(IOrderService orderService, KeyedMutexSynchronizer<OrderId> synchronizer)
	{
		_synchronizer = synchronizer;
		_orderService = orderService;
	}

	[HttpPut("{id})]
	public async Task PutAsync(OrderId id, OrderModel model, CancellationToken cancellationToken)
	{
		// The code in the lambda expression will be protected by a mutex based on OrderId, no two requests with the same OrderId will execute simultaneously, however two requests with different OrderIds will execute simultaneously like normal
		await _synchronizer.InvokeSynchronizedActionAsync(id, async () => { 
			var order = _orderService.GetOrderAsync(id, cancellationToken);
			order.Update(model)
			await _orderService.PersistOrderAsync(order, cancellationToken);
		}, cancellationToken);
	}
}
```

## TaskExtension

This class only offers one method: `WhenAll`. It functions similarly to the built-in `Task.WhenAll` in .Net, except for how it handles Exceptions.

This version throws an `AggregateException` in case any exceptions occur, to allow you the full picture of all exceptions, instead of just the first one (which is what the
built-in `Task.WhenAll` will throw)

## AwaitExtensions

This isn't actually called directly, rather it supports awaiting tuples, even of mixed types, e.g.

```
public Task<string> HelloWorldAsync(); // Returns "Hello world"
public Task<int> MeaningOfLifeAsync(); // Returns 42

var task1 = HelloWorldAsync();
var task2 = MeaningOfLifeAsync();

var results = await (task1, task2);
// results will be "Hello world" and 42 respectively
```

# Documentation
Auto generated documentation via [DocFx](https://github.com/dotnet/docfx) is available here: https://steffenskov.github.io/ExtendedThreading/