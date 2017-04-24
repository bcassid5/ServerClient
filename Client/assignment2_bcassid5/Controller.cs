using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using System.Drawing;

namespace assignment2_bcassid5
{
    class Controller
    {
        //create threads for the listening thread and for the client thread
        Thread t_server;
        //Thread t_client; **not needed from server
        //create a variable for the view and model classes
        private static View _view;
        public RTSPModel _RTSPModel = null;
        public RTPModel _RTPModel = null;
        //however the clients will be a listed structure, due to multiple potential client threads
        //List<ClientModel> _ClientModels = new List<ClientModel>(); **not needed from server
        //finally, some housekeeping variables
        //List<int> used = new List<int>(); **not needed from server

        //create socket for rtsp model
        Socket rtsp_sock = null;
        //integer values
        int cseq;
        int sess;
        int port;

        //adding bool to control sessions
        bool sess_set = true;
        //adding a timer variable (timer from server not used - using system timer)
        System.Timers.Timer timer;
        //string for the video name
        //string vid = "video1.mjpeg";

        public Controller()
        {
            //establish the view form **moved
            //_view = FindForm();
            //establish timer settings
            timer = new System.Timers.Timer();
            timer.Interval = 80;
            timer.Elapsed += OnElapsedEvent;
        }

        private void OnElapsedEvent(object sender, System.Timers.ElapsedEventArgs e)
        {
            //determine info of the sent packet
            //get the physical data
            byte[] rcv_packet = _RTPModel.Get_Data();
            //determine the length
            int len = _RTPModel.Get_Length();
            //check if we want to print the header
            if (_view.Get_PrintHeadStatus())
            {
                //pull the header off the rcv packet, first 12 bytes**
                byte[] head = new byte[12];
                for (int i = 0; i < 12; i++)
                {
                    head[i] = rcv_packet[i];
                }
                //get the data *found similar from server
                var ret = string.Concat(head.Select(b => Convert.ToString(b, 2).PadLeft(8, '0').PadRight(9, ' ')));
                _view.Add_Client_del(ret + "\r\n");
            }
            /*
             ERROR HERE??
             -solved... nested if wrong -.-, assignment complete
             
             */
            
            //displaying the image, use bitmap
            Bitmap bm;
            //use a memory stream? not sure how these work.. testing 'using' c# function
            using (var stream = new MemoryStream(rcv_packet, 12, len - 12))
            {
                bm = new Bitmap(stream);
            }
            //set this new image to the imageBox
            _view.Set_Image(bm);

            //must also check if we want to print the packet report
            if (_view.Get_PackStatus())
            {
                //get all variables for packet report output
                int t = rcv_packet[1] & 0x7f;
                int i = 2;
                int s = (rcv_packet[i++] << 8 | rcv_packet[i++]);
                int ts = (rcv_packet[i++] << 24) | (rcv_packet[i++] << 16) | (rcv_packet[i++] << 8) | rcv_packet[i++];
                //display in the text box
                _view.Add_Client_del("Got RTP packet with SeqNum #" + s + " TimeStamp " + ts + "ms of Type " + t + "\r\n");
            }
            
        }

        //main thread function
        public void Main_Handler(object o)
        {
            //networking variables
            Socket s = (Socket)o;
            byte[] rcv = new byte[256];

            //run similar loop as from server, less info, store all other data in button functions for actions
            try
            {
                //run forever in this loop
                while (true)
                {
                    //get the length of the rcv packet into the socket
                    int length = s.Receive(rcv);
                    if (length == 0) break; //break if empty
                    //recieve the incoming message and display to the test box
                    string ret = System.Text.Encoding.UTF8.GetString(rcv, 0, length);

                    _view.Add_Server_del(ret + "\r\n");

                    //set sess
                    if (sess_set)
                    {
                        string[] split = ret.Split(new char[] { ' ' });
                        sess = Convert.ToInt32(split[split.Length - 1]);
                        sess_set = false;
                    }
                }
            }
            //catch any socket error
            catch (SocketException e)
            {
                if (s != null) s.Close();
            }
            //close when done
            finally
            {
                s.Close();
            }
        }

        public void Connect_Click(object sender, EventArgs e)
        {
            //had to enclose in try/catch forgot - error
            try
            {
                //establish the view form from the button click
                _view = (View)((Button)sender).FindForm();
                _view.Disable();
                //Run connections **very similar to my server
                _RTSPModel = new RTSPModel(Int32.Parse(_view.Get_Port()), _view.Get_IP());
                //connect socket
                rtsp_sock = _RTSPModel.Accept_Server();
                //start the thread (Main_Handler)
                t_server = new Thread(new ParameterizedThreadStart(Main_Handler));
                //set thread to background
                t_server.IsBackground = true;
                //start the thread from the rtsp socket
                t_server.Start(rtsp_sock);
            }
            catch
            {
                //connection not established
            }
       
            
        }
        /*public void Set_VideoName(object sender, EventArgs e)
        {
            vid = _view.Get_Video();
        }*/
        
        //handling all button clicks from the view (click action passed to controller)
        public void Setup_Click(object sender, EventArgs e)
        {
            //set initial sequence
            cseq = 1;
            //set new state
            _view.Add_Client_del("New RTSP State: READY\r\n");
            //Set up new rtp (to get port)
            _RTPModel = new RTPModel(_view.Get_IP());
            port = _RTPModel.Get_Port();

            //run output lines, set vars first
            string r = "SETUP rtsp://" + _view.Get_IP() + ":" + _view.Get_Port() + "/" + _view.Get_Video() + " RTSP/1.0\r\n"; ;
            string c = "CSeq: " + cseq++ + "\r\n";
            string s = "Transport: RTP/UDP; client_port= " + port + "\r\n";
                
            //return final message
            string ret = r + c + s;
            byte[] ret_byte = System.Text.Encoding.ASCII.GetBytes(ret);
            rtsp_sock.Send(ret_byte);
        }
        public void Play_Click(object sender, EventArgs e)
        {
            //set new state
            _view.Add_Client_del("New RTSP State: PLAYING\r\n");

            //run output lines, set vars first
            string r = "PLAY rtsp://" + _view.Get_IP() + ":" + _view.Get_Port() + "/" + _view.Get_Video() + " RTSP/1.0\r\n"; ;
            string c = "CSeq: " + cseq++ + "\r\n";
            string s = "Session: " + sess + "\r\n";

            //return final message
            string ret = r + c + s;
            byte[] ret_byte = System.Text.Encoding.ASCII.GetBytes(ret);
            rtsp_sock.Send(ret_byte);

            //start the timer
            timer.Start();
        }
        public void Pause_Click(object sender, EventArgs e)
        {
            //set new state
            _view.Add_Client_del("New RTSP State: READY\r\n");

            //run output lines, set vars first
            string r = "PAUSE rtsp://" + _view.Get_IP() + ":" + _view.Get_Port() + "/" + _view.Get_Video() + " RTSP/1.0\r\n"; ;
            string c = "CSeq: " + cseq++ + "\r\n";
            string s = "Session: " + sess + "\r\n";

            //return final message
            string ret = r + c + s;
            byte[] ret_byte = System.Text.Encoding.ASCII.GetBytes(ret);
            rtsp_sock.Send(ret_byte);

            //start the timer
            timer.Stop();
        }
        public void Teardown_Click(object sender, EventArgs e)
        {
            //run output lines, set vars first
            string r = "TEARDOWN rtsp://" + _view.Get_IP() + ":" + _view.Get_Port() + "/" + _view.Get_Video() + " RTSP/1.0\r\n"; ;
            string c = "CSeq: " + cseq++ + "\r\n";
            string s = "Session: " + sess + "\r\n";

            //return final message
            string ret = r + c + s;
            byte[] ret_byte = System.Text.Encoding.ASCII.GetBytes(ret);
            rtsp_sock.Send(ret_byte);

            //close the socket
            _RTPModel.Close();

            //clear the picture box
            _view.Clear_Image();

            //start the timer
            timer.Stop();
            _view.Enable();
        }

        
    }
}
