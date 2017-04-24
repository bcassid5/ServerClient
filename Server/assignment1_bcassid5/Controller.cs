using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Timers;
using System.Windows.Forms;

namespace assignment1_bcassid5
{
    class Controller
    {
        //create threads for the listening thread and for the client thread
        Thread t_listen;
        Thread t_client;
        //create a variable for the view and model classes
        private static View _view;
        public RTSPmodel _RTSPmodel = null;
        //however the clients will be a listed structure, due to multiple potential client threads
        List<ClientModel> _ClientModels = new List<ClientModel>();
        //finally, some housekeeping variables
        List<int> used = new List<int>();
        int count = 0;

        //call delegates to update view elements
        public void Add_Server(String mess)
        {
            _view.Add_Server_del(mess);
        }
        public void Set_IP(String mess)
        {
            _view.Set_IP_del(mess);
        }

        //event handler for the button click (passed over from the view on initialization)
        public void listen_Click(object sender, EventArgs e)
        {
            _view = (View)((Button)sender).FindForm();
            //start listening thread
            this.t_listen = new Thread(Listener);
            //this will be a thread running in the background*
            this.t_listen.IsBackground = true;
            //start er' up
            this.t_listen.Start();
        }
        //the function that will run in a separate thread as a listener for client connections
        public void Listener()
        {
            //create the rtsp model with the port number specified in the view
            _RTSPmodel = new RTSPmodel(int.Parse(_view.GetPortNumber()));
            //set the ip address in the view
            Set_IP(_RTSPmodel.ip.ToString());

            //run the loop to keep searching for clients
            while (true)
            {
                Add_Server("The server is now waiting for new connections ... \r\n");
                //create the rtsp socket
                Socket RTSP_sock = _RTSPmodel.Accept_Client();
                Add_Server("A client has joined: " + RTSP_sock.RemoteEndPoint.ToString() + "\r\n");

                //now start another thread (the client threads)
                this.t_client = new Thread(new ParameterizedThreadStart(Main_Handler));
                this.t_client.IsBackground = true;
                this.t_client.Start(RTSP_sock);

            }
        }

        
        
        public void Main_Handler(object o)
        {
            //networking variables
            Socket s = (Socket)o;
            byte[] rcv = new byte[264];
            //control variables
            int cnt = 0; //extension of the class count variable
            int rnd = 0; //extension of the class rand variable
            int seq = 1; //for the sequence number
            bool set = true; //setup is always initially true
            //class variables
            RTPmodel _RTPmodel = null;
            ClientModel _newClient = null;
            Video _video = null;

            //try and catch all potential socket reactions
            try
            {
                while (set)
                {
                    bool exit = false;
                    int size = s.Receive(rcv);
                    if (size == 0) { break; }
                    string size_mess = System.Text.Encoding.UTF8.GetString(rcv, 0, size);
                    _view.Add_Client_del(size_mess);
                    string check_set = System.Text.Encoding.UTF8.GetString(rcv, 0, 4);

                    if (check_set == "SETU" && set)
                    {
                        //rtp
                        string[] new_str = size_mess.Split(new char[] { '=' });
                        int p = int.Parse(new_str[1]);
                        _RTPmodel = new RTPmodel(p);

                        //video
                        string vid = System.Text.Encoding.UTF8.GetString(rcv, 28, 12);
                        _video = new Video(vid);
                        rnd = Get_Random();

                        //client
                        _newClient = new ClientModel(rnd, _video, p);
                        _ClientModels.Add(_newClient);
                        cnt = count++;

                        //update serv
                        Add_Server("The client " + _ClientModels.ElementAt(cnt).Get_RTP().Get_EndPoint().ToString() + " is setting up\r\n");
                        _ClientModels.ElementAt(cnt).Get_Timer().Elapsed += Time_Process;

                        set = false;

                        //response
                        string reqt = "RTSP/1.0 200 OK\r\n";
                        string cseq = "CSeq: " + seq + "\r\n";
                        ++seq;
                        string sess = "Session: " + _ClientModels.ElementAt(cnt).Get_ID() + "\r\n";
                        string mess = reqt + cseq + sess + "\r\n";
                        byte[] mb = System.Text.Encoding.ASCII.GetBytes(mess);
                        s.Send(mb);
                    }

                    //otherwise the setup is not required
                    while (!set)
                    {
                        int size_ns = s.Receive(rcv);
                        if (size == 0) { break; }
                        string size_mess_ns = System.Text.Encoding.UTF8.GetString(rcv, 0, size);
                        _view.Add_Client_del(size_mess_ns);
                        string check_set_ns = System.Text.Encoding.UTF8.GetString(rcv, 0, 4);

                        //determine which action is being performed...
                        if (check_set_ns == "PLAY")
                        {
                            string reqt_play = "RTSP/1.0 200 OK\r\n";
                            string cseq_play = "CSeq: " + seq + "\r\n";
                            ++seq;
                            string sess_play = "Session: " + _ClientModels.ElementAt(cnt).Get_ID() + "\r\n";
                            string mess_play = reqt_play + cseq_play + sess_play + "\r\n";
                            byte[] mb_play = System.Text.Encoding.ASCII.GetBytes(mess_play);
                            s.Send(mb_play);

                            Add_Server("The client " + _ClientModels.ElementAt(cnt).Get_RTP().Get_EndPoint().ToString() + " is playing " + _video.Get_Name() + "\r\n");
                            _ClientModels.ElementAt(cnt).startTimer();
                        }
                        else if (check_set_ns == "PAUS")
                        {
                            string reqt_paus = "RTSP/1.0 200 OK\r\n";
                            string cseq_paus = "CSeq: " + seq + "\r\n";
                            ++seq;
                            string sess_paus = "Session: " + rnd + "\r\n";
                            string mess_paus = reqt_paus + cseq_paus + sess_paus + "\r\n";
                            byte[] mb_paus = System.Text.Encoding.ASCII.GetBytes(mess_paus);
                            s.Send(mb_paus);

                            Add_Server("The client " + _ClientModels.ElementAt(cnt).Get_RTP().Get_EndPoint().ToString() + " paused " + _video.Get_Name() + "\r\n");
                            _ClientModels.ElementAt(cnt).stopTimer();
                        }
                        else if(check_set_ns == "TEAR")
                        {
                            string reqt_stop = "RTSP/1.0 200 OK\r\n";
                            string cseq_stop = "CSeq: " + seq + "\r\n";
                            ++seq;
                            string sess_stop = "Session: " + rnd + "\r\n";
                            string mess_stop = reqt_stop + cseq_stop + sess_stop + "\r\n";
                            byte[] mb_stop = System.Text.Encoding.ASCII.GetBytes(mess_stop);
                            s.Send(mb_stop);

                            Add_Server("The client " + _ClientModels.ElementAt(cnt).Get_RTP().Get_EndPoint().ToString() + " is tearing down " + _video.Get_Name() + "\r\n");
                            _ClientModels.ElementAt(cnt).stopTimer();

                            _ClientModels.ElementAt(cnt).Get_Video().Trash_Video();
                            this.used.Remove(_ClientModels.ElementAt(cnt).Get_ID());
                            set = true;
                        }
                        else
                        {
                            //dead state...
                        }
                    }
                    if (exit)
                        break;
                }
            }
            catch (SocketException e)
            {
                if(s!=null) { s.Close(); }
            }
            finally
            {
                _ClientModels.ElementAt(cnt).stopTimer();
                s.Close();
            }

        }



        private void Time_Process(Object obj, ElapsedEventArgs args)
        {
            RTSPtimer t = (RTSPtimer)obj;
            for (int i=0; i < _ClientModels.Count; i++)
            {
                if (_ClientModels.ElementAt(i).Get_ID() == t.Get_ID())
                {
                    _ClientModels.ElementAt(i).Inc_Seq();
                    RTPmodel rtp = _ClientModels.ElementAt(i).Get_RTP();
                    IPEndPoint ep = rtp.Get_EndPoint();
                    Video vid = _ClientModels.ElementAt(i).Get_Video();
                    byte[] rcv = vid.Get_NextFrame();
                    RTPpacket rtp_pack = new RTPpacket(rcv, _ClientModels.ElementAt(i).Get_Seq(), rcv.Length);

                    if (_view.Get_Check())
                    {
                        Add_Server(rtp_pack.Get_Header() + "\r\n");
                    }
                    rtp.Send_Data(rtp_pack.Get_Bytes());
                }
            }
        }



        //creation of random variables
        private Random rand = new Random();
        private int Get_Random()
        {
            //want to get a new random id number, so it cannot already exist in used list
            bool found = false;
            int ret = 0;

            do
            {
                ret = rand.Next();
                if(!used.Contains(ret)) { found = true; }
            } while (!found);

            used.Add(ret);
            return ret;
        }

        
        
        

        
        
        
        
        /*
        public void listen_Click(object sender, EventArgs e)
        {
            _view = new View();
            
            if (_view.GetPortNumber() == "")
            {
                _view.DisplayError_Empty();
            }
            else
            {
                int pn = int.Parse(_view.GetPortNumber());
                if (pn <= 1024)
                {
                    _view.DisplayError_Invalid();
                }
                else
                {
                    //Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    _view.AddServerText("Server is waiting for a new connection...\r\n");
                    //parse the IP address
                    IPAddress ip = _view.GetIpAddress();
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
                                                            //MessageBox.Show("Accepted connection from: " + tcpClient.RemoteEndPoint.ToString(), "Connection Established");
                            _view.AddServerText("Accepted connection from: " + tcpClient.RemoteEndPoint.ToString());
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
                                //MessageBox.Show("Error occurred on accepted socket: {0}",err.Message);
                            }
                            finally
                            {
                                tcpClient.Close();
                            }
                        }
                        catch (SocketException err)
                        {
                            //MessageBox.Show("Accept failed: {0}", err.Message);
                        }
                    }
                    tcpServer.Close();
                }
            }
        }*/
    }
}
