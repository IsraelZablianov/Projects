package hit.controller;

import java.io.IOException;
import java.net.ServerSocket;
import java.net.Socket;
import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;

public class MMULogFileController 
{
	public void start() throws IOException, ClassNotFoundException
	{
		ServerSocket server = new ServerSocket(12345);
		ExecutorService Users = Executors.newFixedThreadPool(25);
		while(true)
		{
			Socket someClient = server.accept();
			User user = new User(someClient);
			Users.execute(user);
		}
	}
}
