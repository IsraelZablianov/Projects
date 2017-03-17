package hit.login;

import java.io.IOException;
import java.nio.file.Files;
import java.nio.file.Paths;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

public class Authentication 
{
	private final static String DEFAULT_FILE_NAME = "src/main/resources/users/users.txt";
	private Map<String,String> listOfUsers = null;
	public Authentication() throws IOException
	{
		listOfUsers = new HashMap<>();
		List<String> listOfDataAboutUsers = new ArrayList<>(); 
		listOfDataAboutUsers = Files.readAllLines(Paths.get(DEFAULT_FILE_NAME));
		String[] partsOfUserInfo;
		for(String userInfo : listOfDataAboutUsers)
		{
			partsOfUserInfo = userInfo.split(" ");
			listOfUsers.put(partsOfUserInfo[0], partsOfUserInfo[1]);
		}
	}
	
	public boolean authenticate(String user, String password) throws IOException  
	{	
		return listOfUsers.containsKey(user) && listOfUsers.get(user).equals(password);
	}
}
