package hit.controller;

import hit.model.MMUClient;
import hit.model.MMUModel;
import hit.view.MMUView;

public class MMUController implements Controller
{
	MMUModel model = null;
	MMUView view = null;
	public MMUController(MMUModel i_Model, MMUView i_View)
	{		
		model = i_Model;
		view = i_View;
		model.readData();
	}
	
	public MMUController()
	{		
	}
	
	@Override
	public void start() 
	{
		view.setPageSize(model.GetPageSize());
		view.setProcessesNumber(model.GetProcessesNumber());
		view.setRamCapacity(model.GetRamCapacity());
		view.SetData(model.GetListOfData());
		view.open();
	}	

	public int GetRamCapacity()
	{
		return model.GetRamCapacity();
	}

	public int GetProcessesNumber() 
	{
		return model.GetProcessesNumber();
	}
	
	/**setting the model while using remote logs
	 * @param i_Model
	 */
	public void SetModel(MMUModel i_Model)
	{
		model = i_Model;
		model.readData();
	}
	
	public void SetView(MMUView i_View)
	{
		view = i_View;		
	}
	
	/**
	 * getting password user name and file
	 * to initialise the model and the entire system 
	 */
	public void LogIn() 
	{
		MMUClient client = new MMUClient();
		boolean UPFInfo = false;
		boolean logedIn = true;
		while(!UPFInfo && logedIn)
		{
			view.LogIn();
			logedIn = view.LogInIsLogedIn();
			if(logedIn)
			{
				UPFInfo = client.Request(view.LogInGetUsername(), view.LogInGetPassword(), view.LogInGetFileName());
			}
			
			if(!UPFInfo && logedIn)
			{
				view.ErrorMsg("Wrong!!!");
			}
		}
		if(!UPFInfo)
		{
			/**
			 * If the user chose not to log in 
			 * the application must stop.
			 */
			System.exit(0);
		}
		else
		{
			SetModel(new MMUModel(client.getLogFile()));
		}
	}
}
