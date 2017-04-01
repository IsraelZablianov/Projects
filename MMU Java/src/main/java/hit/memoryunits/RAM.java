package hit.memoryunits;

import java.io.Serializable;
import java.util.*;

public class RAM implements Serializable
{
	private int initialCapacity;
	private Map<Long,Page<byte[]>> pages;
	
	public RAM()
	{
	}
	
	public RAM(int capacity) 
	{
		pages = new LinkedHashMap<Long,Page<byte[]>>(capacity);
		initialCapacity = capacity;
	}
	
	public Map<Long,Page<byte[]>> GetAllPages()
	{
		return pages;
	}
	
	public int ramSize()
	{
		return pages.size();
	}
	
	public void addPage(Page<byte[]> addPage) 
	{
			pages.put(addPage.getPageId(), addPage);
	}
	
	public void addPages(Page<byte[]>[] addPages) 
	{
		for(Page<byte[]> addPage : addPages)
		{
			pages.put(addPage.getPageId(), addPage);
		}
		
	}
	
	public int getInitialCapacity() 
	{
		return initialCapacity;	
	}
	
	public Page<byte[]> getPage(Long pageId)
	{
			return	pages.get(pageId);		
	}
	
	public Page<byte[]>[] getPages(Long[] pageIds) 
	{
		Page<byte[]>[] copyPages = new Page[pageIds.length];
		int count=0;
		for(int i = 0; i < pageIds.length; i++)
		{ 
			copyPages[i] = pages.get(pageIds[i]);
		}
		
		return copyPages;
	}
	
	public void removePage(Page<byte[]> removePage) 
	{
		pages.remove(removePage);
	}
	
	public void removePages(Page<byte[]>[] removePages) 
	{
		for(Page<byte[]> page:removePages)
		{
			pages.remove(page);
		}
	}
	
	public void setInitialCapacity(int initialCapacity) 
	{
		this.initialCapacity = initialCapacity;
	}
}
