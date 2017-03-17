package hit.view;

import java.awt.*;
import java.util.List;
import java.awt.event.*;
import java.util.ArrayList;

import javax.swing.*;
import javax.swing.border.*;
 

/**
 * A dialog of login display
 * @author Israel
 */

public class LoginDialog extends JDialog 
{
 
    private JTextField tfUsername;
    private JTextField tfFileName;
    private JPasswordField pfPassword;
    private JLabel lbUsername;
    private JLabel lbPassword;
    private JLabel lbFileName;
    private JButton btnLogin;
    private JButton btnCancel;
    private boolean succeeded = false;
    private List<String> UPFInformation = new ArrayList<>();
 
    public LoginDialog(Frame parent) 
    {
        super(parent, "Login", true);
        JPanel panel = new JPanel(new GridBagLayout());
        GridBagConstraints cs = new GridBagConstraints();
        setSize((int)(Toolkit.getDefaultToolkit().getScreenSize().width * 0.25), (int)(Toolkit.getDefaultToolkit().getScreenSize().height * 0.25));
        cs.fill = GridBagConstraints.HORIZONTAL; 
        
        lbUsername = new JLabel("Username: ");
        cs.gridx = 0;
        cs.gridy = 0;
        cs.gridwidth = 1;
        panel.add(lbUsername, cs);
 
        tfUsername = new JTextField(20);
        cs.gridx = 1;
        cs.gridy = 0;
        cs.gridwidth = 2;
        panel.add(tfUsername, cs);
 
        lbPassword = new JLabel("Password: ");
        cs.gridx = 0;
        cs.gridy = 1;
        cs.gridwidth = 1;
        panel.add(lbPassword, cs);
 
        pfPassword = new JPasswordField(20);
        cs.gridx = 1;
        cs.gridy = 1;
        cs.gridwidth = 2;
        panel.add(pfPassword, cs);
        panel.setBorder(new LineBorder(Color.GRAY));
        
        lbFileName = new JLabel("File  name: ");
        cs.gridx = 0;
        cs.gridy = 2;
        cs.gridwidth = 1;
        panel.add(lbFileName, cs);
 
        tfFileName = new JTextField(20);
        cs.gridx = 1;
        cs.gridy = 2;
        cs.gridwidth = 2;
        panel.add(tfFileName, cs);
        btnLogin = new JButton("Login");
        btnLogin.addActionListener(new ActionListener() 
        {
 
            public void actionPerformed(ActionEvent e) 
            {
            	succeeded = true;            
                dispose();
            }
        });
        btnCancel = new JButton("Cancel");
        btnCancel.addActionListener(new ActionListener() 
        {
 
            public void actionPerformed(ActionEvent e) 
            {
                System.exit(0);
            }
        });
        JPanel bp = new JPanel();
        bp.add(btnLogin);
        bp.add(btnCancel);
 
        getContentPane().add(panel, BorderLayout.CENTER);
        getContentPane().add(bp, BorderLayout.PAGE_END);
 
        setResizable(false);
        setLocationRelativeTo(parent);
    }
    
    public String getUsername() {
        return tfUsername.getText();
    }
 
    public String getPassword() {
        return new String(pfPassword.getPassword());
    }
 
    public String getFileName() {
        return tfFileName.getText();
    }
    
    public boolean isSucceeded() {
        return succeeded;
    }
}
