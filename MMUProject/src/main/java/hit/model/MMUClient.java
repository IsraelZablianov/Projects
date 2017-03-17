package hit.model;

import java.io.File;
import java.io.FileWriter;
import java.io.IOException;
import java.io.ObjectInputStream;
import java.io.ObjectOutputStream;
import java.net.InetAddress;
import java.net.Socket;
import java.util.ArrayList;
import java.util.List;

public class MMUClient 
{
	private String logFile = null;
	private boolean succeed = false;
	private List<String> copyFile = new ArrayList<>();

	/**
	 * sending a request to the server for downloading the file requested
	 * @param userName
	 * @param Password
	 * @param fileName
	 * @return true if the user exist in the server
	 */
	public boolean Request(String userName, String Password, String fileName)
	{
		try
		{ 
				
			InetAddress address = InetAddress.getLocalHost();
			Socket myServer = new Socket(address, 12345);
			ObjectOutputStream output = new ObjectOutputStream(myServer.getOutputStream());
			output.writeObject(userName);
			output.writeObject(Password);
			output.writeObject(fileName);
			output.flush();
			ObjectInputStream input = new ObjectInputStream(myServer.getInputStream());
			logFile = (String)input.readObject(); 
			copyFile = (List<String>)input.readObject(); 
			output.close(); 
			input.close(); 
			myServer.close();
		}
		catch (Exception e) 
		{
			System.out.println(e.getMessage());
		}
		
		if(logFile != null && !logFile.equals("ERROR"))
		{			
			succeed = true;
			try 
			{
				logFile = "remotelogs/" + fileName;
				FileWriter writer = new FileWriter(logFile); 
				for(String str : copyFile)
				{			
					writer.write(str + "\r");
				}
				writer.close();
			}
			catch (IOException e) 
			{
				System.out.println(e.getMessage());
			}
		}
		
		return succeed;
	}
	
	public String getLogFile() 
	{
		return logFile;
	}
}
