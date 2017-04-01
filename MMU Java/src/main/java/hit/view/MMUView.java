package hit.view;

import java.awt.BorderLayout;
import java.awt.Color;
import java.awt.Dimension;
import java.awt.FlowLayout;
import java.awt.Font;
import java.awt.GridLayout;
import java.awt.Image;
import java.awt.Toolkit;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.awt.event.MouseEvent;
import java.net.MalformedURLException;
import java.net.URL;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.Timer;
import java.util.TimerTask;
import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;
import java.util.concurrent.TimeUnit;

import javax.swing.*;
import javax.swing.border.Border;
import javax.swing.event.DocumentEvent;
import javax.swing.event.DocumentListener;
import javax.swing.event.MouseInputListener;
import javax.swing.table.DefaultTableModel;
import javax.swing.table.TableColumn;
import javax.swing.table.TableColumnModel;
import javax.swing.text.BadLocationException;
import javax.swing.text.DefaultHighlighter;

import com.google.gson.JsonArray;
import com.google.gson.JsonObject;
import com.google.gson.internal.bind.JsonTreeReader;

public class MMUView implements View, ActionListener, MouseInputListener
{
    private Integer currentLineInData = 0;
	private Integer pageSize = 0;
	private Integer processesNumber = 0;
	private Integer ramCapacity = 0;
    private Integer pageFaultAmountCounter = 0;
    private Integer pageReplacmentAmountCounter = 0;
	private JFrame frame = new JFrame("MMU model by Israel Zablianov");
    private JTable table;
    private JButton play;
    private JButton background;
    private JButton playAll;
    private JButton reset;
    private DefaultTableModel tableModel;
    private JTextField pageFaultAmountTextField;
    private JTextField pageReplacmentAmountTextField;
    private JTextArea processesTextArea;
    private Map<String,String> processesSelected = new HashMap<>();
    private Map<String,List<String>> pagesInRam = new HashMap<>();
    private List<String> data = null;
    private JTextArea dataTextArea;
    private String path = "src/main/resources/Image/matrix4.jpg";
    private List<ImagePanel> ImagePanels = new ArrayList<>();
    private Integer indexOfPath = 4;
    private LoginDialog login;
    private Display slowDisplay;
    private JSpinner spinner;
	
    public MMUView() 
	{
	}
   
    public void SetData(List<String> i_DataToReturn)
    {
    	this.data = i_DataToReturn;
    }
    
	public int getPageSize() {
		return pageSize;
	}

	public void setPageSize(int pageSize) {
		this.pageSize = pageSize;
	}
	
	public void setProcessesNumber(int processesNumber) {
		this.processesNumber = processesNumber;
	}
	
	public int getProcessesNumber() {
		return processesNumber;
	}

	public int getRamCapacity() {
		return ramCapacity;
	}

	public void setRamCapacity(int pageAmount) {
		this.ramCapacity = pageAmount;
	}
	
	public JSpinner getSpinner() {
		return spinner;
	}
	
	private JPanel InitialTextArea()
	{
        processesTextArea = new JTextArea(5,7);
        ImagePanel westPanel = new ImagePanel(path);
        ImagePanels.add(westPanel);
        westPanel.setBackground( Color.LIGHT_GRAY );
        JLabel lb3 = new JLabel("Processes");
		String newLine = System.getProperty("line.separator");
		StringBuilder processStr = new StringBuilder();
		for(int i = 0 ;i < processesNumber; i++)
		{
			processStr.append("process" + i + newLine);
		}
		
        Border border = BorderFactory.createLineBorder(Color.BLACK);
        processesTextArea.setBorder(BorderFactory.createCompoundBorder(border, 
        BorderFactory.createEmptyBorder(8, 8, 8, 8)));
        processesTextArea.setLineWrap(true);
        processesTextArea.setText(processStr.toString());
        processesTextArea.addMouseListener(this);
        westPanel.setLayout(new BorderLayout());
        westPanel.add(lb3, BorderLayout.NORTH);
        westPanel.add(processesTextArea, BorderLayout.WEST);
        
        return westPanel;
	}

	private JPanel InitialTextFields() 
	{
		ImagePanel eastPanel = new ImagePanel(path);
		ImagePanel insidePanel = new ImagePanel(path);
		ImagePanel insidePanel2 = new ImagePanel(path);
		ImagePanels.add(eastPanel);
		ImagePanels.add(insidePanel);
		ImagePanels.add(insidePanel2);
        eastPanel.setBackground( Color.LIGHT_GRAY );
        insidePanel.setBackground( Color.LIGHT_GRAY );
        insidePanel2.setBackground( Color.LIGHT_GRAY );
        JLabel lb1 = new JLabel("              Page Fault Amount");
        lb1.setForeground(Color.white);
        pageFaultAmountTextField = new JTextField();
        pageFaultAmountTextField.setForeground(Color.blue);
        pageFaultAmountTextField.setText(pageFaultAmountCounter.toString() + "   ");
        JLabel lb2 = new JLabel("Page Replacment Amount");
        lb2.setForeground(Color.white);
        pageReplacmentAmountTextField = new JTextField();
        pageReplacmentAmountTextField.setForeground(Color.blue);
        pageReplacmentAmountTextField.setText(pageReplacmentAmountCounter.toString() + "   ");        
        insidePanel.add(lb1);
        insidePanel.add(pageFaultAmountTextField);
        insidePanel2.add(lb2);
        insidePanel2.add(pageReplacmentAmountTextField);
        eastPanel.setLayout(new BorderLayout());
        eastPanel.add(insidePanel, BorderLayout.NORTH);
        eastPanel.add(insidePanel2, BorderLayout.EAST);
        return eastPanel;
	}
	
	private JPanel InitialTable()
	{
		ImagePanel northPanel = new ImagePanel(path);
		ImagePanels.add(northPanel);
        JLabel tableLabel = new JLabel("       RAM - DISPLAY  ");
        ImagePanel tabelPanel = new ImagePanel(path);
        ImagePanels.add(tabelPanel);
		SpinnerModel sm = new SpinnerNumberModel(5, 1, 10, 1); //default value,lower bound,upper bound,increment by
		spinner = new JSpinner(sm);
		spinner.addMouseListener(this);
		spinner.setToolTipText("Display speed of the table");
        tabelPanel.setBackground( Color.LIGHT_GRAY );
        tableLabel.setForeground(Color.white);
        tableLabel.setFont(new Font(null, 350, 35));
        tabelPanel.setLayout(new BorderLayout());
        tabelPanel.add(tableLabel, BorderLayout.NORTH);
        northPanel.setBackground( Color.LIGHT_GRAY );
        tableModel = new DefaultTableModel(pageSize + 1,ramCapacity);
        table = new JTable(tableModel);
		for(int i = 0; i < ramCapacity; i++)
		{
	        TableColumnModel colModel = table.getColumnModel();
	        TableColumn col = colModel.getColumn(i);
	        col.setPreferredWidth(80);
		}
		
		for(int j = 0; j < ramCapacity; j++)
		{
			table.setValueAt("PAGE ID: ", 0, j);
		}
		
		for(int i = 1; i < pageSize + 1; i++)
		{
			for(int j = 0; j < ramCapacity; j++)
			{
				table.setValueAt(0, i, j);
			}
		}
		
        Border border = BorderFactory.createLineBorder(Color.BLACK);
        table.setBorder(BorderFactory.createCompoundBorder(border, 
        BorderFactory.createEmptyBorder(8, 8, 8, 8)));
        tabelPanel.add(table, BorderLayout.CENTER);        
        northPanel.add(spinner);
        northPanel.add(tabelPanel);
        return northPanel;
	}
	
	private JPanel InitialButtons()
	{
		ImagePanel southPanel = new ImagePanel(path);
		ImagePanels.add(southPanel);
        southPanel.setBackground( Color.LIGHT_GRAY );
        background = new JButton("background");
        reset = new JButton("Reset");
        play = new JButton("Play");
        playAll = new JButton("Play All");
        background.addActionListener(this);
        reset.addActionListener(this);
        play.addActionListener(this);
        playAll.addActionListener(this);
        play.setToolTipText( "Click here to perform the action once." );
        playAll.setToolTipText( "Click here to perform all the actions." );
        reset.setToolTipText( "Click here to initialize the system." );
        background.setToolTipText("Click here to change the background.");
        play.addMouseListener(this);
        playAll.addMouseListener(this);
        reset.addMouseListener(this);
        southPanel.add(play);
        southPanel.add(playAll);
        southPanel.add(reset);
        southPanel.add(background);
        return southPanel;
	}

	private JPanel InitialScrolPane()
	{
		ImagePanel centerPanel = new ImagePanel(path); 
		ImagePanels.add(centerPanel);
		centerPanel.setLayout(new BorderLayout());
		centerPanel.setBackground( Color.LIGHT_GRAY );
		dataTextArea = new JTextArea(15,15);
		String listString = new String();
		String newLine = System.getProperty("line.separator");
		for (String s : data)
		{
		    listString += s + newLine;
		}

		dataTextArea.setText(listString);
		dataTextArea.addMouseListener(this);
		dataTextArea.setToolTipText("List of processes actions");
		JScrollPane scrolPane = new JScrollPane(dataTextArea);	
		centerPanel.add(scrolPane);
		return centerPanel;
	}
	
	private void InitialFrameContent() 
	{
		frame.setLayout(new BorderLayout());
		frame.setSize((int)(Toolkit.getDefaultToolkit().getScreenSize().width * 0.5), (int)(Toolkit.getDefaultToolkit().getScreenSize().height * 0.5));
		frame.setLocationRelativeTo(null);
		frame.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
		VideoIcon videoIcon = new VideoIcon(frame);
		videoIcon.execute();
		ImagePanel panel = new ImagePanel(path);
		ImagePanels.add(panel);
		
		JPanel northPanel = InitialTable();
        JPanel westPanel = InitialTextArea();
        JPanel eastPanel = InitialTextFields();		       
        JPanel southPanel = InitialButtons();      
        JPanel centerPanel = InitialScrolPane();
        westPanel.add(centerPanel);
        
        frame.add(northPanel, BorderLayout.NORTH);
        frame.add(southPanel, BorderLayout.SOUTH);
        frame.add(westPanel, BorderLayout.WEST);
        frame.add(eastPanel, BorderLayout.EAST);
        frame.add(panel);
        frame.setVisible(true);
	}
	
	public void LogIn()
	{
        login = new LoginDialog(frame);
        login.setVisible(true);
	}
	
    public String LogInGetUsername() {
        return login.getUsername();
    }
 
    public String LogInGetPassword() {
        return login.getPassword();
    }
 
    public String LogInGetFileName() {
        return login.getFileName();
    }
    
    public boolean LogInIsLogedIn() {
        return login.isSucceeded();
    }
	
	@Override
	public void open() 
	{
		InitialFrameContent();
	}
	
	private boolean CheckSelctionProcesses()
	{
		boolean goodSelection = true;
		String newLine = System.getProperty("line.separator");
		String[] parts = processesTextArea.getSelectedText().split(newLine);
		if(!processesTextArea.getSelectedText().startsWith("process"))
		{
			goodSelection = false;
		}
		else
		{
			for(String part : parts)
			{
				if(part.length() <= 7)
				{
					goodSelection = false;
				}
			}
		}
		
		return goodSelection;
	}
	
	private void WrongSelectionProcessMsg()
	{
		processesSelected.clear();
		processesTextArea.getHighlighter().removeAllHighlights();
		JOptionPane.showMessageDialog(frame,"Wrong selection of processes", "MMU-PROCESS-ERROR",JOptionPane.ERROR_MESSAGE);	
	}
	
	public void ErrorMsg(String msg)
	{
		JOptionPane.showMessageDialog(frame,msg, "ERROR",JOptionPane.ERROR_MESSAGE);
	}
	
	private void InitialSelectedProcesses() 
	{
		if(processesTextArea.getSelectedText() != null && CheckSelctionProcesses())
		{
			String newLine = System.getProperty("line.separator");
			String[] parts = processesTextArea.getSelectedText().split(newLine);
			try
			{
				int start =  processesTextArea.getText().indexOf(processesTextArea.getSelectedText());
				int end = start + processesTextArea.getSelectedText().length();
		        DefaultHighlighter.DefaultHighlightPainter painter = new DefaultHighlighter.DefaultHighlightPainter( Color.green );
		        processesTextArea.getHighlighter().removeAllHighlights();
		        processesTextArea.getHighlighter().addHighlight(start, end, painter);
				processesSelected.clear();
				for(String part : parts)
				{
					processesSelected.put(part.substring(7).replace("\r",""), part.substring(7));
				}
			}
			catch(Exception ee)
			{		
				WrongSelectionProcessMsg();
			}
		}
		else if(processesTextArea.getSelectedText() != null)
		{
			WrongSelectionProcessMsg();
		}
	} 
	
	synchronized void SynchrnizeViewWithProcessesSelected(String ProcessID, String PageID, List<String> contantOfPage)
	{
		for(int i = 0; i < ramCapacity; i++)
		{
			if(!pagesInRam.containsKey(((String) table.getValueAt(0, i)).substring(9)))
			{
				table.setValueAt("PAGE ID: ", 0, i);
				for(int j = 1; j < pageSize + 1; j++)
				{
					table.setValueAt(0, j, i);
				}
			}	
		}
		
		if(processesSelected.containsKey(ProcessID))
		{
			boolean done = false;
			
			for(int i = 0; i < ramCapacity && !done; i++)
			{
				if(((String) table.getValueAt(0, i)).substring(9).equals(PageID))
				{
					int j = 1;
					done = true;
					for(String value : contantOfPage)
					{
						table.setValueAt(value, j, i);
						j++;
					}
				}
			}
			
			for(int i = 0; i < ramCapacity && !done; i++)
			{
				if(!pagesInRam.containsKey(((String) table.getValueAt(0, i)).substring(9)))
				{
					table.setValueAt("PAGE ID: " + PageID, 0, i);
					int j = 1;
					done = true;
					for(String value : contantOfPage)
					{
						table.setValueAt(value, j, i);
						j++;
					}
				}
			}
		}
	}
	
	private Boolean CheckComand(String IsComand)
	{
		return data.get(currentLineInData).length() >= 2 && data.get(currentLineInData).substring(0,2).equals(IsComand);
	}
	
	private Boolean CheckIfPageFaultAndExcecuteAction()
	{
		Boolean isPageFault = false;
		if(CheckComand("PF"))
		{
			isPageFault = true;
			pageFaultAmountCounter++;
			pageFaultAmountTextField.setText(" " + pageFaultAmountCounter.toString());
			pagesInRam.put(data.get(currentLineInData).substring(3), null);			
		}
		
		return isPageFault;
	}
	
    private Boolean CheckIfPageReplacmentAndExcecuteAction()
	{
		Boolean isPageReplacment = false;
		if(CheckComand("PR"))
		{
			isPageReplacment = true;
			pageReplacmentAmountCounter++;
			pageReplacmentAmountTextField.setText(pageReplacmentAmountCounter.toString());
			String[] parts = data.get(currentLineInData).split(" ");
			pagesInRam.remove(parts[2]);
			pagesInRam.put(parts[4], null);
		}
		
		return isPageReplacment;
	}

	private Boolean CheckIfGetPagesAndExcecuteAction()
	{
		Boolean isGetPages = false;
		if(CheckComand("GP"))
		{
			isGetPages = true;
			String ProcessID, PageID;
			List<String> contantOfPage = new ArrayList(pageSize);			
			String[] parts = data.get(currentLineInData).split(" ");
			ProcessID = parts[0].substring(4);
			PageID = parts[1];
			
			int start =  data.get(currentLineInData).indexOf("[") + 1;
			int end = data.get(currentLineInData).indexOf("]");
			String arrayOfData = data.get(currentLineInData).substring(start, end);
			String[] items = arrayOfData.split(", ");
			
			for (int i = 0; i < items.length; i++) 
			{
				contantOfPage.add(items[i]);
			}
			
			SynchrnizeViewWithProcessesSelected(ProcessID, PageID, contantOfPage);
		}
		
		return isGetPages;
	}
	
	private void MarkCurrentLine()
	{
		int start = 0;
		int end = 0;
		try 
		{
			start = dataTextArea.getLineStartOffset(currentLineInData);
			end = dataTextArea.getLineEndOffset(currentLineInData);
			
		} 
		catch (BadLocationException e1) 
		{
			ErrorMsg(e1.getMessage());
		}
        DefaultHighlighter.DefaultHighlightPainter painter = new DefaultHighlighter.DefaultHighlightPainter( Color.green );
		try 
		{
			dataTextArea.getHighlighter().removeAllHighlights();
			dataTextArea.getHighlighter().addHighlight(start, end, painter);
		} 
		catch (BadLocationException e) 
		{
			ErrorMsg(e.getMessage());
		}
	}
	
	private void InitialSystem()
	{
	    currentLineInData = 0;
	    pageFaultAmountCounter = 0;
	    spinner.getModel().setValue(5);
	    pageFaultAmountTextField.setText("  " + pageFaultAmountCounter.toString());
	    pageReplacmentAmountCounter = 0;
	    pageReplacmentAmountTextField.setText("  " +pageReplacmentAmountCounter.toString());
	    pagesInRam.clear();
	    dataTextArea.getHighlighter().removeAllHighlights();
	    processesSelected.clear();
        processesTextArea.getHighlighter().removeAllHighlights();
		StringBuilder processStr = new StringBuilder();
		String newLine = System.getProperty("line.separator");
		for(int i = 0 ;i < processesNumber; i++)
		{
			processStr.append("process" + i + newLine);
		}
        processesTextArea.setText(processStr.toString());
		
		for(int j = 0; j < ramCapacity; j++)
		{
			table.setValueAt("PAGE ID: ", 0, j);
		}
		
		for(int i = 1; i < pageSize + 1; i++)
		{
			for(int j = 0; j < ramCapacity; j++)
			{
				table.setValueAt(0, i, j);
			}
		}
		
		slowDisplay.cancel(true);
		play.setEnabled(true);
		playAll.setEnabled(true);
	    ReadNextLine();
	}
	
	public boolean ReadNextLine()
	{
		boolean readNextLine = true;
		if(currentLineInData != data.size())
		{
			readNextLine = true;
			if(CheckIfPageFaultAndExcecuteAction()){}
			else if(CheckIfPageReplacmentAndExcecuteAction()){}
			else if(CheckIfGetPagesAndExcecuteAction()){}
			MarkCurrentLine();
			currentLineInData++;
		}		
		else if(currentLineInData == data.size())
		{
			readNextLine = false;
			int dialogButton = JOptionPane.YES_NO_OPTION; 
			int dialogResult = JOptionPane.showConfirmDialog(null, "Would you like to try again?", "The simulation is over!!!", dialogButton);
			if(dialogResult == 0) 
			{
				InitialSystem();
			}
			else
			{
				play.setEnabled(false);
				playAll.setEnabled(false);
			}
		}
		
		return readNextLine;
	}
	
	@Override
	public void actionPerformed(ActionEvent arg0) 
	{	
		if(arg0.getSource() == play)
		{
			ReadNextLine();
		}
		else if(arg0.getSource() == playAll)
		{	
			slowDisplay = new Display(this);
			slowDisplay.execute();
		}
		else if(arg0.getSource() == reset)
		{
			InitialSystem();
		}
		else if(arg0.getSource() == background)
		{
			indexOfPath++;
			for(ImagePanel panel : ImagePanels)
			{
				panel.SetPath("src/main/resources/Image/matrix" + indexOfPath % 7 + ".jpg");
				panel.paint(panel.getGraphics());
			}
		}
	}

	@Override
	public void mouseClicked(MouseEvent e) {
		// TODO Auto-generated method stub
		
	}

	@Override
	public void mouseEntered(MouseEvent e)
	{	
	}

	@Override
	public void mouseExited(MouseEvent e) {		
	}

	@Override
	public void mousePressed(MouseEvent e) {	
	}

	@Override
	public void mouseReleased(MouseEvent e) {
		InitialSelectedProcesses();
	}

	@Override
	public void mouseDragged(MouseEvent arg0) {
		// TODO Auto-generated method stub	
	}

	@Override
	public void mouseMoved(MouseEvent arg0) {
	}

	
}
