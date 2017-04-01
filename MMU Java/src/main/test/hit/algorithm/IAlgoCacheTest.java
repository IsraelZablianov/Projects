package hit.algorithm;

public class IAlgoCacheTest 
{
	public static void main(String[] args) 
	{
		IAlgoCache<Integer, Integer> cache = new LFUAlgoCacheImpl<>(3);
		Integer res = -1;
		putOrGet(cache,1, 1);
		putOrGet(cache,0, 0);
		putOrGet(cache,7, 7);
		putOrGet(cache,0, 0);
		res = putOrGet(cache,8, 8);
		System.out.println(res);
		/**
		 * 1 expected LFU
		 */
		
		Integer print;
		IAlgoCache<Integer, Integer> cache2 = new FIFOAlgoImpl<>(3);
		putOrGet(cache2, 8, 8);
		putOrGet(cache2, 9, 9);
		putOrGet(cache2, 2, 2);
		print=putOrGet(cache2, 1, 1);//8 expected{9,2,1}
		System.out.println(print);
		print=putOrGet(cache2, 3, 1);//9 expected {2,1,3}
		System.out.println(print);
		print =putOrGet(cache2, 9, 9);//2 expected {1,3,9}
		System.out.println(print);
		IAlgoCache<Integer, Integer> cache3 = new LRUAlgoCacheImpl<>(3);
		print=putOrGet(cache3, 9, 9);
		print=	putOrGet(cache3, 9, 9);
		print=putOrGet(cache3, 2, 2);
		print=putOrGet(cache3, 2, 2);
		print=putOrGet(cache3, 4, 4);
		print=putOrGet(cache3, 3, 3);
		print=putOrGet(cache3, 9, 9);
		print=putOrGet(cache3, 2, 2);
		putOrGet(cache3, 2, 2);
		System.out.println(print);			
	}

		private static Integer putOrGet(IAlgoCache<Integer, Integer> cache, Integer key, Integer value) 
		{
			if(cache.getElement(key) == null)
			{
				return cache.putElement(key, value);
			}
			return -1;
		}		
	}