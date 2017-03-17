package hit.processes;

import java.io.IOException;
import java.util.Arrays;
import java.util.List;
import java.util.Map;
import java.util.logging.Level;

import hit.memoryunits.HardDisk;
import hit.memoryunits.MemoryManagementUnit;
import hit.memoryunits.Page;
import hit.util.MMULogger;

public class Process implements Runnable
{
	private int id;
	private MemoryManagementUnit mmu;
	private ProcessCycles processCycles;
	private MMULogger mLoger = MMULogger.getInstance();
	
	public Process(int id, MemoryManagementUnit mmu, ProcessCycles processCycles)
	{
		this.id = id;
		this.mmu = mmu;
		this.processCycles = processCycles;
	}
	
	public int getId()
	{
		return id;
	}
	
	public void setId(int id)  
	{
		this.id = id;
	}

	@Override
	public void run() 
	{
		List<ProcessCycle> psCycles = processCycles.getProcessCycles();
		
		for(ProcessCycle psCycle : psCycles)
		{	
			/**
			*moving over every ProcessCycle
			*/
			Long[] pageIds = new Long[psCycle.getPages().size()];
			pageIds = psCycle.getPages().toArray(pageIds);			
			try 
			{
				Page<byte[]>[] pagesReturned = mmu.getPages(pageIds);
				for(int i = 0; i < pagesReturned.length; i++)
				{
					/**
					*moving over all pages requested 
					*and making changes in them
					*/
					synchronized (pagesReturned[i]) 
					{
						/**
						*I don't want that any thread will interrupt
						*while changing the page content
						*this why I made it synchronised
						*/
						for(int j = 0; j < psCycle.getData().get(i).length; j++)
						{
							pagesReturned[i].getContent()[j] = psCycle.getData().get(i)[j];
						}
						
						/**
						 * This will retrieve line separator dependent on OS.
						 */
						String newLine = System.getProperty("line.separator");
						mLoger.write("GP:P" + id + " " + pagesReturned[i].getPageId() + " " + Arrays.toString(pagesReturned[i].getContent()) + newLine, Level.INFO);
					}
				}
				
				Thread.sleep(psCycle.getSleepMs());
			} 
			
			catch(InterruptedException e) 			
			{
				System.out.println(e.getMessage());
			}
			
			catch(IOException e1) 			
			{
				System.out.println(e1.getMessage());
			}
		}
	}
}
