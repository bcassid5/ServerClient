using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Net;
using System.Net.Sockets;

using System.Threading;

namespace assignment1_bcassid5
{
    public partial class View : Form
    {
        /*RTSPmodel rtsp_control = new RTSPmodel();
        IPAddress ip;
        int pn;*/

        //instantiate controller object
        Controller _controller;
        
        //instantiate text setting delegate **
        delegate void SetTextCallback(string text);
        
        /*constructor for the view
        - this constructor will initialize the component as well as set the controller function that 
        will act as the listener for when the button 'listen' is pressed
             */
        public View()
        {
            InitializeComponent();

            //set event handler to controller
            _controller = new Controller();
            this.listen.Click += new System.EventHandler(_controller.listen_Click);
        }

        /*fucntion to get the port number...*/
        public String Get_PN()
        {
            return this.portNum.Text.ToString();
        }

        /*Create member functions to handle UI elements... */
        //ip text box setting
        public void Set_IP(String mess)
        {
            this.ipNum.Text = mess;
        }
        //ip text box setting - delegate version
        public void Set_IP_del(String mess)
        {
            //use delegate to set view text
            SetTextCallback new_t = new SetTextCallback(Set_IP);
            this.Invoke(new_t, new Object[] { mess });
        }

        //text functions for updating the server status and the client requests boxes

        //regular client updates 
        public void Add_Client(String mess)
        {
            this.client.Text += mess;
        }
        //regular server updates 
        public void Add_Server(String mess)
        {
            this.server.Text += mess;
        }

        //delegate client updates
        public void Add_Client_del (String mess)
        {
            SetTextCallback new_t = new SetTextCallback(Add_Client);
            this.Invoke(new_t, new Object[] { mess });
        }
        //delegate client updates
        public void Add_Server_del(String mess)
        {
            SetTextCallback new_t = new SetTextCallback(Add_Server);
            this.Invoke(new_t, new Object[] { mess });
        }

        //almost forgot the checkbox
        public bool Get_Check()
        {
            return printRTP.Checked;
        }
















        public String GetPortNumber() {             
            return this.portNum.Text;
        }

        public IPAddress GetIpAddress()
        {
            return IPAddress.Parse(ipNum.Text);
        }


        public void DisplayError_Empty() {
            MessageBox.Show("Please enter a port number first.", "Invalid Port", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }


        public void DisplayError_Invalid()
        {
            MessageBox.Show("The port number entered is invalid (>1024).", "Invalid Port", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }


        public void AddServerText(String mess) {
            server.Text += mess + "\r\n";
        }

        private void listen_Click(object sender, EventArgs e)
        {

        }
        /*
Listen Click Function
- this function is called when listen is clicked.
- it first checks the port number field to make sure something was entered and that it is a valid port number

    */
        /*private void listen_Click(object sender, EventArgs e)
        {
            
            if (portNum.Text == "")
            {
                MessageBox.Show("Please enter a port number first.", "Invalid Port", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else {
                pn = int.Parse(portNum.Text);
                if (pn <= 1024)
                {
                    MessageBox.Show("The port number entered is invalid (>1024).", "Invalid Port", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
                else
                {
                    //Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    server.Text += "Server is waiting for a new connection...\r\n";
                    //parse the IP address
                    ip = IPAddress.Parse(ipNum.Text);
                    //create the end point
                    IPEndPoint listenEndPoint = new IPEndPoint(ip, pn);

                    //bind the socket
                    Socket tcpServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    tcpServer.Bind(listenEndPoint);
                    tcpServer.Listen(int.MaxValue);
                    //wait for client connections
                    Socket tcpClient;
                    byte[] receiveBuffer = new byte[4096];
                    while (true)
                    {
                        try
                        {
                            tcpClient = tcpServer.Accept(); //the process holds until
                                                            // a client connects
                            MessageBox.Show("Accepted connection from: " + tcpClient.RemoteEndPoint.ToString(), "Connection Established");
                            server.Text += "Accepted connection from: " + tcpClient.RemoteEndPoint.ToString() + "\r\n";
                            try
                            {
                                while (true)
                                {
                                    int rc = tcpClient.Receive(receiveBuffer);
                                    if (rc == 0)
                                        break;
                                }
                            }
                            catch (SocketException err)
                            {
                                MessageBox.Show("Error occurred on accepted socket: {0}",
                                err.Message);
                            }
                            finally
                            {
                                tcpClient.Close();
                            }
                        }
                        catch (SocketException err)
                        {
                            MessageBox.Show("Accept failed: {0}", err.Message);
                        }
                    }
                    tcpServer.Close();

                    /*ThreadStart work = ListenThread;
                    Thread thread = new Thread(work);
                    thread.Start();
                }
            }
            
        }*/

    }
}
