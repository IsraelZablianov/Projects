package hit.model;

import java.io.FileNotFoundException;
import java.io.IOException;
import java.nio.file.Files;
import java.nio.file.Paths;
import java.util.ArrayList;
import java.util.List;
import java.util.logging.Level;

import hit.util.MMULogger;

public class MMUModel implements Model
{
	private final int pageSize = 5;
	private Integer ramCapacity;
	private Integer processesNumber;
	private final String DEFAULT_FILE_NAME;
	List<String> dataToReturn = null;
	
	public MMUModel(String i_FileName)
	{
		DEFAULT_FILE_NAME = i_FileName;
	}
	
	public List<String> GetListOfData()
	{	
		return dataToReturn;
	}
	
	@Override
	public void readData() 
	{
		try 
		{
			dataToReturn = new ArrayList<>();
			dataToReturn.addAll(Files.readAllLines(Paths.get(DEFAULT_FILE_NAME)));
			for (int i = 0; i < dataToReturn.size() && (ramCapacity == null || processesNumber == null); i++) 
			{
				if(dataToReturn.get(i).length() >=2 && dataToReturn.get(i).substring(0,2).equals("RC"))
				{
					ramCapacity = Integer.parseInt(dataToReturn.get(i).substring(3));
				}
				
				else if(dataToReturn.get(i).length() >=2 && dataToReturn.get(i).substring(0,2).equals("PN"))
				{
					processesNumber = Integer.parseInt(dataToReturn.get(i).substring(3));
				}
			}
		} 
		catch (IOException e) 
		{
			System.out.println(e.getMessage());
		}
	}

	public int GetRamCapacity()
	{
		return ramCapacity;
	}

	public int GetProcessesNumber() 
	{
		return processesNumber;
	}
	
	public int GetPageSize() 
	{
		return pageSize;
	}
}
