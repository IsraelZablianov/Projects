package hit.util;

import java.io.IOException;
import java.text.Format;
import java.util.logging.FileHandler;
import java.util.logging.Formatter;
import java.util.logging.Level;
import java.util.logging.LogRecord;
import java.util.logging.Logger;

public class MMULogger 
{
	public static final String DEFAULT_FILE_NAME = "logs/Log.txt";  
	private static final MMULogger MLoger = new MMULogger();
	private FileHandler handler;
	
	private MMULogger()
	{
		try 
		{
			handler = new FileHandler(DEFAULT_FILE_NAME);
		} 
		catch (IOException e) 
		{
			e.printStackTrace();
		}
	}
	
	public static MMULogger getInstance()  
	{
		return MLoger;
	}
	
	public synchronized void write(String command, Level level)
	{
		LogRecord record = new LogRecord(level, command);
		handler.setFormatter(new OnlyMessageFormatter());
		handler.publish(record);
	}
		
	class OnlyMessageFormatter extends Formatter
	{
		public OnlyMessageFormatter()
		{
			super();
		}
		
		@Override
		public String format(LogRecord record) 
		{
			return record.getMessage();
		}		
	}
}
