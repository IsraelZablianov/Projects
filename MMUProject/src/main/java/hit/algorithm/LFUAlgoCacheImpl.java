package hit.algorithm;

import java.util.*;
import java.util.Map.Entry;

public class LFUAlgoCacheImpl<K,V> implements IAlgoCache<K,V>
{
	private Map<K, V> cache;	
	private Map<K, Integer> cacheCounter;
	private int capacity;
	
	public LFUAlgoCacheImpl(int capacity)
	{
		cache = new HashMap<>(capacity);
		cacheCounter = new HashMap<>();
		this.capacity = capacity;
	}
	
	@Override
	public V getElement(K key) 
	{
		if(cache.containsKey(key))
		{
			Integer count = cacheCounter.get(key);
			cacheCounter.put(key, ++count);
			return cache.get(key);
		}
		return null;
	}

	@Override
	public V putElement(K key, V value) 
	{
		V valueReturned = null;
		if(capacity == cache.size())
		{
			K minKey = findMin();
			valueReturned = getElement(minKey);
			removeElement(minKey);
		}
		
		cache.put(key, value);
		cacheCounter.put(key, 0);
		return valueReturned;
	}

	@Override
	public void removeElement(K key) 
	{
		if(cache.containsKey(key))
		{
			cache.remove(key);
			cacheCounter.remove(key);
		}
	}
	
	private K findMin() 
	{
		Integer minValueInMap=(Collections.min(cacheCounter.values())); 
        for (Entry<K, Integer> entry : cacheCounter.entrySet()) 
        {  
            if (entry.getValue()==minValueInMap) 
            {                
                return entry.getKey();
            }
        }
        
        return null;
	}
	
	@Override
	public String toString()
	{
		return cache.toString();
	}
}
