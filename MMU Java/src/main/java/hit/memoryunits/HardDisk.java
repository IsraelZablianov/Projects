package hit.memoryunits;

import java.io.FileInputStream;
import java.io.FileNotFoundException;
import java.io.FileOutputStream;
import java.io.IOException;
import java.io.ObjectInputStream;
import java.io.ObjectOutputStream;
import java.util.LinkedHashMap;
import java.util.Map;
import java.util.logging.Level;

import hit.algorithm.LRUAlgoCacheImpl;
import hit.util.HardDiskInputStream;
import hit.util.HardDiskOutputStream;
import hit.util.MMULogger;

public class HardDisk 
{
	private static final int SIZE = 1000;
	private static final String DEFAULT_FILE_NAME = "src/main/resources/hardDisk/HardDisk.txt";
	private static final HardDisk instance = new HardDisk();
	private Map<Long,Page<byte[]>> pages = new LinkedHashMap<Long,Page<byte[]>>(SIZE);
	private boolean allreadyRead = false; 
	private MMULogger mLoger = MMULogger.getInstance();
	
	/**
	 * Initialising the hard disk 
	 */
	private HardDisk()
	{
		byte[] page1 = "isra1".getBytes();
		byte[] page2 = "isra2".getBytes();
		byte[] page3 = "isra3".getBytes();
		byte[] page4 = "isra4".getBytes();
		byte[] page5 = "isra5".getBytes();
		byte[] page6 = "isra6".getBytes();		
		Page<byte[]> p1 = new Page<byte[]>((long) 1, page1);
		Page<byte[]> p2 = new Page<byte[]>((long) 2, page2);
		Page<byte[]> p3 = new Page<byte[]>((long) 3, page3);
		Page<byte[]> p4 = new Page<byte[]>((long) 4, page4);
		Page<byte[]> p5 = new Page<byte[]>((long) 5, page5);
		Page<byte[]> p6 = new Page<byte[]>((long) 6, page6);
		pages.put(p1.getPageId(), p1);
		pages.put(p2.getPageId(), p2);
		pages.put(p3.getPageId(), p3);
		pages.put(p4.getPageId(), p4);
		pages.put(p5.getPageId(), p5);
		pages.put(p6.getPageId(), p6);
		
		byte[] page = {1,1,1,1,1};
		for(int i = 7; i < HardDisk.SIZE; i++)
		{
			pages.put((long) i, new Page<byte[]>((long) i,page.clone()));
		}
		try 
		{
			writeHd();
		} 
		catch (IOException e) 
		{
			System.out.println(e.getMessage());
		}
	}
	
	public static HardDisk getInstance()
	{
		return instance;
	}
	
	public String getFileName()
	{
		return DEFAULT_FILE_NAME;
	}
	
	public Map<Long,Page<byte[]>> getPages() throws FileNotFoundException, IOException
	{
		readHD();
		return pages;
	}
	
	public Page<byte[]> pageFault(Long pageId) throws FileNotFoundException, IOException
	{
		if(pages.containsKey(pageId))
		{
			writeHd();
			return pages.get(pageId);
		}
		
		return null;
	}
	public Page<byte[]> pageReplacement(Page<byte[]> moveToHdPage, Long moveToRamId) throws FileNotFoundException, IOException
	{
		pages.put(moveToHdPage.getPageId(), moveToHdPage);
		if(pageFault(moveToRamId) != null)
		{
			return pageFault(moveToRamId);
		}
		
		writeHd();	
		return null;
	}
	
	private void writeHd() throws FileNotFoundException, IOException 
	{
		HardDiskOutputStream WriteToHardDisk = null;
		WriteToHardDisk = new HardDiskOutputStream(new ObjectOutputStream(new FileOutputStream(DEFAULT_FILE_NAME)));
		WriteToHardDisk.writeAllPages(pages);
		WriteToHardDisk.close();
	}
	
	/**
	 * only one read is needed at the first use
	 * @throws FileNotFoundException
	 * @throws IOException
	 */
	private void readHD() throws FileNotFoundException, IOException
	{
		if(!allreadyRead)
		{
			HardDiskInputStream readFromHDisk = null;
			readFromHDisk = new HardDiskInputStream(new ObjectInputStream(new FileInputStream(DEFAULT_FILE_NAME)));
			pages = readFromHDisk.readAllPages();
			readFromHDisk.close();
			allreadyRead = true;
		}
	}
	
	/**
	 * making the HD with singleton pattern is not enough,
	 * because it can be cloned.
	 * that is why the method clone need to be override.
	 */
	@Override
	protected Object clone() throws CloneNotSupportedException 
	{
		throw new CloneNotSupportedException();
	}
}
