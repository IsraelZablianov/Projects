package hit.view;

import java.awt.Color;
import java.awt.Graphics;
import java.awt.image.BufferedImage;
import java.io.File;
import java.io.IOException;

import javax.imageio.ImageIO;
import javax.swing.JPanel;

/**
 * A panel with Image background
 * @author Israel
 */
class ImagePanel extends JPanel
{
	private boolean showImage = true;
    private BufferedImage image;
    private String path;

    public ImagePanel(String path) 
    {
       this.path = path;
    }

    public void SetPath(String iPath)
    {
    	this.path = iPath;
    }
    
    @Override
    protected void paintComponent(Graphics g) 
    {
		try 
		{
			setBackground( Color.LIGHT_GRAY ); 
			image = ImageIO.read(new File(path));
		} 
		catch (IOException e) 
		{showImage = false;}
		
		if(showImage)
    	{
	        super.paintComponent(g);
	        g.drawImage(image, 0, 0, getWidth(), getHeight(), null); 
    	}
    }
}