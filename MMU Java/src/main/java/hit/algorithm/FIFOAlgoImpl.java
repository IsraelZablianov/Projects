package hit.algorithm;

import java.util.LinkedHashMap;
import java.util.Map;
import java.util.Map.Entry;
import hit.memoryunits.Page;

public class FIFOAlgoImpl<K,V> implements IAlgoCache<K,V>
{

	private Map<K,V> queue;

	private int capacity;
	
	public FIFOAlgoImpl(int size) 
	{
		queue = new LinkedHashMap<K,V>(size);
		capacity=size;
		
	}
	
	@Override
	public V getElement(K key) 
	{		
		if(queue.containsKey(key))
		{
			return  queue.get(key);
		}
		
		return null;
	}

	@Override
	public V putElement(K key, V value) 
	{
		Entry<K, V> firsElement = null;
		V valueReturned=null;
		if(capacity == queue.size())
		{
			firsElement=queue.entrySet().iterator().next();
			valueReturned=firsElement.getValue();
			removeElement(firsElement.getKey());
		}
		
		queue.put(key, value);
		return valueReturned;
	}

	@Override
	public void removeElement(K key) 
	{
		if(queue.containsKey(key))
		{
			queue.remove(key);
		}		
	}

	@Override
	public String toString()
	{
		return queue.toString();
	}

}
