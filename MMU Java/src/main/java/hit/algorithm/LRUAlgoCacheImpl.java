package hit.algorithm;

import java.util.*;
import java.util.Map.Entry;

public class LRUAlgoCacheImpl<K,V> implements IAlgoCache<K,V> 
{	
	private Map<K,V> cache;
	private int capacity;
	
	public LRUAlgoCacheImpl(int size) 
	{
		cache = new LinkedHashMap<K,V>(size);
		capacity=size;
		
	}
	
	@Override
	public V getElement(K key) 
	{
		if(cache.containsKey(key))
		{
			V value=cache.get(key);
			removeElement(key);
			putElement(key, value);
			return value;
		}
		
		return null;
	}
	
	/**
	 * least recently used is the first element!!!
	 */
	@Override
	public V putElement(K key, V value) 
	{
		Entry<K, V> firsElement = null;
		V valueReturned=null;
		if(capacity == cache.size())
		{
			firsElement=cache.entrySet().iterator().next();
			removeElement(firsElement.getKey());
			valueReturned=firsElement.getValue();
		}
		
		cache.put(key, value);
		return valueReturned;
	}

	@Override
	public void removeElement(K key) 
	{
		if(cache.containsKey(key))
		{
			cache.remove(key);
		}	
	}
	
	@Override
	public String toString()
	{
		return cache.toString();
	}
}
