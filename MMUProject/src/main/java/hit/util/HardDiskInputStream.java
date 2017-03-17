package hit.util;

import java.io.EOFException;
import java.io.IOException;
import java.io.ObjectInputStream;
import java.util.LinkedHashMap;
import java.util.Map;

import hit.memoryunits.Page;

public class HardDiskInputStream extends ObjectInputStream
{

	private ObjectInputStream input;
	
	public HardDiskInputStream(ObjectInputStream in) throws IOException, SecurityException 
	{
		input = in;
	}
	
	public Map<Long, Page<byte[]>> readAllPages() 
	{
		Map<Long, Page<byte[]>> pages = new LinkedHashMap<Long, Page<byte[]>>();
		Page<byte[]> page = new Page<byte[]>();
		try 
		{
			while(true)
			{
				page = (Page<byte[]>)input.readObject();
				pages.put(page.getPageId(), page);
			}
		} 
		
		catch (ClassNotFoundException e) 
		{
			e.printStackTrace();
		}
		
		catch (EOFException e) 
		{
			/**
			 * end of file do nothing
			 */
		}
		
		catch (IOException e) 
		{
			e.printStackTrace();
		}
		
		finally 
		{
			try 
			{
				input.close();
			} 
			
			catch (IOException e) 
			{
				e.printStackTrace();
			}
		}
		return pages;
	}
	
	@Override
	public void close() throws IOException 
	{
		input.close();
	}
}
