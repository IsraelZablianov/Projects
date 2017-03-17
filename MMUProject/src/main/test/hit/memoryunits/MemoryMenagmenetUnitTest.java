package hit.memoryunits;

import java.io.IOException;
import org.junit.Assert;
import org.junit.Test;
import hit.algorithm.LRUAlgoCacheImpl;

public class MemoryMenagmenetUnitTest 
{	
	@Test
	public void test() throws IOException
	{
		MemoryManagementUnit mmu = new MemoryManagementUnit(3, new LRUAlgoCacheImpl<>(3));
		Page<byte[]>[] pageReturned;
		Long[] pageIds = new Long[6];
		pageIds[0] = (long)1;
		pageIds[1] = (long)2;
		pageIds[2] = (long)3;
		pageIds[3] = (long)5;
		pageIds[4] = (long)2;
		pageIds[5] = (long)4;
		pageReturned = mmu.getPages(pageIds);
		
		byte[] page1 = "isra1".getBytes();
		byte[] page2 = "isra2".getBytes();
		byte[] page3 = "isra3".getBytes();
		byte[] page4 = "isra4".getBytes();
		byte[] page5 = "isra5".getBytes();
		
		Page<byte[]> p1 = new Page<byte[]>((long) 1, page1);
		Page<byte[]> p2 = new Page<byte[]>((long) 2, page2);
		Page<byte[]> p3 = new Page<byte[]>((long) 3, page3);
		Page<byte[]> p4 = new Page<byte[]>((long) 4, page4);
		Page<byte[]> p5 = new Page<byte[]>((long) 5, page5);
		
		Assert.assertEquals(p1, pageReturned[0]);
		Assert.assertEquals(p2, pageReturned[1]);
		Assert.assertEquals(p3, pageReturned[2]);
		Assert.assertEquals(p5, pageReturned[3]);
		Assert.assertEquals(p2, pageReturned[4]);
		Assert.assertEquals(p4, pageReturned[5]);
		
		for(Page<byte[]> page : pageReturned)
		{
			String str = new String(page.getContent(), "UTF-8");
			System.out.println(str);
		}	
	}
}
