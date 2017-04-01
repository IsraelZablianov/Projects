package hit.view;

import java.awt.Image;
import java.net.URL;
import java.text.DecimalFormat;
import java.util.ArrayList;
import java.util.List;

import javax.imageio.ImageIO;
import javax.swing.ImageIcon;
import javax.swing.JFrame;
import javax.swing.SwingWorker;

public class VideoIcon extends SwingWorker<Integer, Integer> 
{
	private JFrame frame;
	private List<Image> images = new ArrayList<>();
	public VideoIcon(JFrame frame)
	{
		this.frame = frame;
		Image image = null;
		DecimalFormat formatter = new DecimalFormat("000");
		String format;
		for(int i = 0; i < 200; i++)
		{
			format = formatter.format(1 + i % 200);
			image = new ImageIcon("src/main/resources/Image/Matrix/matrix " + (format) + ".jpg").getImage();
			images.add(image);	
		}
	}
	  
	@Override	 
	protected Integer doInBackground() throws Exception 
	{	
		while(true)
		{
			for(Image images : images)
			{
				frame.setIconImage(images); 
				Thread.sleep(150);
			}
		}
	}	  
}