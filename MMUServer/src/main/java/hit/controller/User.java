package hit.controller;

import java.io.IOException;
import java.io.ObjectInputStream;
import java.io.ObjectOutputStream;
import java.net.Socket;

import hit.applicationservice.MMULogService;
import hit.login.Authentication;

/**
 * user requests implements runnable so that the server
 * could manage several requests at the same time using thread pool 
 * @author Israel
 */
public class User implements Runnable
{
	private String userName;
	private String password;
	private String fileName;
	private Socket someClient;
	
	public User(Socket isomeClient)
	{
		this.someClient = isomeClient;
	}
	
	@Override
	public void run()
	{
		try
		{
			ObjectInputStream input = new ObjectInputStream(someClient.getInputStream()); 
			userName = (String)input.readObject(); 
			password = (String)input.readObject(); 
			fileName = (String)input.readObject(); 
			Authentication authentication = new Authentication();
			
			if(authentication.authenticate(userName, password))
			{
				MMULogService service = new MMULogService(someClient, fileName);
				service.ReturnFileOrErrorMsg();
			}
			else
			{
				ObjectOutputStream output = new ObjectOutputStream(someClient.getOutputStream());
				output.writeObject("ERROR");
				output.close();
			}
			input.close();  
			someClient.close(); 
		}
		catch(IOException | ClassNotFoundException e)
		{
			System.exit(0);
		}
	}
}
