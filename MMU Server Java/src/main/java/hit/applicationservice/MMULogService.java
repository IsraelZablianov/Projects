package hit.applicationservice;

import java.io.IOException;
import java.io.ObjectOutputStream;
import java.net.Socket;
import java.nio.file.Files;
import java.nio.file.Paths;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

public class MMULogService 
{
	private Socket socket;
	private String fileName;
	private final static String DEFAULT_FILE_NAME = "src/main/resources/listOfLogCaches/ListOfFileNames.txt";
	private Map<String,String> listOfFileNames = null;
	public MMULogService(Socket isocket, String ifileName) throws IOException
	{
		this.socket = isocket;
		this.fileName = ifileName;
		listOfFileNames = new HashMap<>();
		List<String> fileNames = new ArrayList<>(); 
		fileNames = Files.readAllLines(Paths.get(DEFAULT_FILE_NAME));
		for(String fName : fileNames)
		{
			listOfFileNames.put(fName, "src/main/resources/logcache/" + fName);
		}
	}
	
	/**
	 * checks if the file exist 
	 * returning the file or error message
	 * @throws IOException
	 * @throws ClassNotFoundException
	 */
	public void ReturnFileOrErrorMsg() throws IOException, ClassNotFoundException
	{
		ObjectOutputStream output = new ObjectOutputStream(socket.getOutputStream());
		if(listOfFileNames.containsKey(fileName))
		{			
			output.writeObject(fileName);
			output.writeObject(Files.readAllLines(Paths.get(listOfFileNames.get(fileName))));
		}
		else
		{
			output.writeObject("ERROR");
		}
		output.flush();
		output.close(); 
	}
}
