package hit.view;

import java.util.List;

import javax.swing.SwingWorker;

public class Display extends SwingWorker<Integer, Integer> 
{
	private MMUView mmuView;
	public Display(MMUView view)
	{
		mmuView = view;
	}
	  
	@Override	 
	protected Integer doInBackground() throws Exception 
	{
		Long value = Long.parseLong((mmuView.getSpinner().getModel().getValue()).toString());
		while(mmuView.ReadNextLine())
		{
			Thread.sleep(1000 - value * 100);			
		}
		
		return 1;	  
	}
	  
}