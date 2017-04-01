package hit.util;

import java.io.FileInputStream;
import java.io.FileOutputStream;
import java.io.IOException;
import java.io.ObjectInputStream;
import java.io.ObjectOutputStream;
import java.util.LinkedHashMap;
import java.util.Map;
import java.util.Map.Entry;

import javax.swing.text.html.HTMLDocument.Iterator;

import hit.memoryunits.Page;

public class HardDiskOutputStream extends ObjectOutputStream 
{
	private ObjectOutputStream output;

	public HardDiskOutputStream(ObjectOutputStream out) throws IOException, SecurityException 
	{
		output = out;
	}
	
	public void writeAllPages(Map<Long,Page<byte[]>> hd)  
	{
		try 
		{
			for(Page<byte[]> page : hd.values())
			{
				output.writeObject(page);
			}
		} 
		
		catch (IOException e) 
		{
			e.printStackTrace();
		} 

		finally 
		{
			try 
			{
				output.close();
			}
			
			catch (IOException e) 
			{
				e.printStackTrace();
			}
		}
	}
	
	@Override
	public void close() throws IOException 
	{
		output.close();
	}
}
