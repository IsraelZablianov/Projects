package hit.memoryunits;

import java.io.FileInputStream;
import java.io.FileNotFoundException;
import java.io.IOException;
import java.io.ObjectInputStream;
import java.util.LinkedHashMap;
import java.util.Map;
import java.util.Map.Entry;
import java.util.logging.Level;

import hit.algorithm.IAlgoCache;
import hit.util.HardDiskInputStream;
import hit.util.MMULogger;

public class MemoryManagementUnit 
{

	private IAlgoCache<Long, Long> algo;
	private RAM ram;
	private HardDisk HDisk;
	private MMULogger mLoger = MMULogger.getInstance();
	public MemoryManagementUnit(int ramCapacity, IAlgoCache<Long, Long> Ialgo) 
	{
		algo = Ialgo;
		ram = new RAM(ramCapacity);
		String newLine = System.getProperty("line.separator");
		mLoger.write("RC " + ramCapacity + newLine, Level.INFO);
	}
	
	public IAlgoCache<Long, Long> getAlgo()  
	{
		return algo;
	}
	
	public synchronized Page<byte[]>[] getPages(Long[] pageIds) throws IOException  
	{
		Page<byte[]>[] pagesReturned = new Page[pageIds.length];
		HDisk = HardDisk.getInstance();
		for(int i = 0; i < pageIds.length; i++)
		{
			/**
			 * if the page exist in the  ram
			 */
			if(algo.getElement(pageIds[i]) != null)
			{
				pagesReturned[i] = ram.getPage(pageIds[i]);
			}
			
			/**
			 * if there is a place to add the page but the page is'nt in the ram
			 */
			else if(ram.ramSize() < ram.getInitialCapacity())
			{
				if(HDisk.pageFault(pageIds[i]) != null)
				{
					algo.putElement(pageIds[i], pageIds[i]);
					ram.addPage(HDisk.pageFault(pageIds[i]));
					pagesReturned[i] = ram.getPage(pageIds[i]);
					String newLine = System.getProperty("line.separator");
					mLoger.write("PF " + pageIds[i] + newLine, Level.INFO);
				}
			}
			
			/**
			 * if the ram is full
			 * we will use page replacement algorithm
			 */
			else if(HDisk.pageFault(pageIds[i]) != null)
			{
				Long id = algo.putElement(pageIds[i], pageIds[i]);
				Page<byte[]> removedPage = new Page<byte[]>();
				removedPage = ram.getPage(id);
				ram.removePage(removedPage);
				ram.addPage(HDisk.pageReplacement(removedPage, pageIds[i]));
				pagesReturned[i] = ram.getPage(pageIds[i]);			
				String newLine = System.getProperty("line.separator");
				mLoger.write("PR MTH " + removedPage.getPageId() + " MTR " + pageIds[i] + newLine, Level.INFO);
			}
		}
		
		return pagesReturned;
	}
	
	public void ShutDown() throws FileNotFoundException, IOException
	{	
		Map<Long,Page<byte[]>> pages = ram.GetAllPages();
		for(Map.Entry<Long,Page<byte[]>> page : pages.entrySet())
		{
			HDisk.pageReplacement(page.getValue(), null);
		}
	}
	
	public RAM getRam()  
	{
		return ram;
	}
	
	public void setAlgo(IAlgoCache<Long, Long> Ialgo)  
	{
		algo = Ialgo;
	}
	
	public void setRam(RAM Iram)
	{
		ram = Iram;	
	}
}
