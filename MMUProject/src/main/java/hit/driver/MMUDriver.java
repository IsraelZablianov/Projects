package hit.driver;

import java.io.BufferedReader;
import java.io.FileNotFoundException;
import java.io.FileReader;
import java.io.IOException;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;
import java.util.Map;
import java.util.Map.Entry;
import java.util.concurrent.Executor;
import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;
import java.util.concurrent.TimeUnit;
import java.util.logging.Level;
import java.util.logging.Logger;

import com.google.gson.Gson;
import com.google.gson.JsonIOException;
import com.google.gson.JsonSyntaxException;
import com.google.gson.stream.JsonReader;

import hit.algorithm.LRUAlgoCacheImpl;
import hit.controller.Controller;
import hit.controller.MMUController;
import hit.memoryunits.HardDisk;
import hit.memoryunits.MemoryManagementUnit;
import hit.memoryunits.Page;
import hit.model.MMUModel;
import hit.model.Model;
import hit.processes.*;
import hit.processes.Process;
import hit.util.MMULogger;
import hit.view.MMUView;
import hit.view.View;
public class MMUDriver 
{
	private static int appIds;  
	private static final String CONFIG_FILE_NAME = "src/main/resources/configuration/Configuration.json";   
	private final static boolean remote = true;
	
	public static void main(String[] args) throws JsonIOException, JsonSyntaxException, InterruptedException, IOException
	{
		if(remote)
		{
			MMUController controller = new MMUController();
			MMUView view = new MMUView();
			controller.SetView(view);
			controller.LogIn();
			controller.start();
		}
		else
		{
			MemoryManagementUnit mmu = new MemoryManagementUnit(5, new LRUAlgoCacheImpl<Long,Long>(5));
			RunConfiguration runconfig = ReadConfigurationFile();
			List<ProcessCycles> processCycles = runconfig.getProcessesCycles();
			List<Process> processes = createProcesses(processCycles, mmu);
			runProcesses(processes);
			mmu.ShutDown();
			
			MMUModel model = new MMUModel(MMULogger.DEFAULT_FILE_NAME);
			MMUView view = new MMUView();
			Controller controller = new MMUController(model, view);
			controller.start();
		}
	}
	
	/**
	 * creating and using Thread pool to manage all the processes
	 * @param applications
	 * @throws InterruptedException 
	 */
	private static void runProcesses(List<Process> applications) throws InterruptedException  
	{	
		ExecutorService ThreadPool = Executors.newCachedThreadPool();
		for(Process procs : applications)
		{
			ThreadPool.execute(procs);
		}
		
		((ExecutorService) ThreadPool).shutdown();
		ThreadPool.awaitTermination(Integer.MAX_VALUE, TimeUnit.SECONDS);
	}
	
	private static List<Process> createProcesses(List<ProcessCycles> appliocationsScenarios, MemoryManagementUnit mmu)
	{
		List<Process> processes = new ArrayList<>();
		for(int appIds = 0; appIds < appliocationsScenarios.size(); appIds++)
		{
			processes.add(new Process(appIds, mmu, appliocationsScenarios.get(appIds)));
		}
		
		MMULogger mLoger = MMULogger.getInstance();
		String newLine = System.getProperty("line.separator");
		mLoger.write("PN:" + processes.size() + newLine, Level.INFO);
		return processes;
	}
	
	/**
	 * Reading Json file
	 * and import it to our MMU driver 
	 * @return RunConfiguration object
	 * @throws JsonIOException
	 * @throws JsonSyntaxException
	 * @throws FileNotFoundException
	 */
	public static RunConfiguration ReadConfigurationFile() throws JsonIOException, JsonSyntaxException, FileNotFoundException
	{
		Gson gson = new Gson();
		RunConfiguration runConfiguration = null;
		runConfiguration = gson.fromJson(new JsonReader(new FileReader(CONFIG_FILE_NAME)), RunConfiguration.class);			
		return runConfiguration;
	}	
}
