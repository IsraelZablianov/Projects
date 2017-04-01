package hit.processes;

import java.util.*;

public class ProcessCycle 
{
	private List<byte[]> data;  
	private List<Long> pages;  
	private int sleepMs;
	
	public ProcessCycle(List<Long> pages, int sleepMs, List<byte[]> data) 
	{
		this.data = data;
		this.pages = pages;
		this.sleepMs = sleepMs;
	}
	
	public List<byte[]> getData()  
	{
		return data;
	}
	
	public List<Long> getPages() 
	{
		return pages;
	}
	
	public int getSleepMs() 
	{
		return sleepMs;
	}
	
	void setData(List<byte[]> data) 
	{
		this.data = data;		
	}
	
	void setPages(List<Long> pages) 
	{
		this.pages = pages;
	}
	
	void setSleepMs(int sleepMs)
	{
		this.sleepMs = sleepMs;
	}
	
	public String toString()
	{
		String msg = new String();
		for(int i = 0; i < data.size(); i++)
		{
			msg += Arrays.toString(data.get(i));
		}
		
		return msg;
	}
}
