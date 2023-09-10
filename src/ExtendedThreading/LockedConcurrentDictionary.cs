using System.Collections.Concurrent;

namespace ExtendedThreading;

public class LockedConcurrentDictionary<TKey, TValue>
where TKey : notnull
{
	private readonly ConcurrentDictionary<TKey, TValue> _dictionary = new();
	private readonly object _lock = new();

	public TValue GetOrAdd(TKey key, Func<TKey, TValue> valueFactory)
	{
		if (_dictionary.TryGetValue(key, out var value))
		{
			return value;
		}

		lock (_lock)
		{
			if (_dictionary.TryGetValue(key, out value))
			{
				return value;
			}
			_dictionary[key] = value = valueFactory(key);
			return value;
		}
	}

	public TValue this[TKey key]
	{
		set => _dictionary[key] = value;
	}

	public bool TryGetValue(TKey key, out TValue? value)
	{
		return _dictionary.TryGetValue(key, out value);
	}

	public bool TryRemove(TKey key, out TValue? value)
	{
		return _dictionary.TryRemove(key, out value);
	}
}