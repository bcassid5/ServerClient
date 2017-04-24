using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.Net.Sockets;

using System.Threading;

namespace assignment1_bcassid5
{
    class RTSPmodel
    {
        //variables for the ip and port
        private int pn;
        public IPAddress ip;
        //variables for the two sockets
        Socket RTSP_sock = null;
        Socket tcp_sock = null;
        //variable for end point to be set up
        IPEndPoint ep = null;
        //variable to recieve message
        byte[] rcv = new byte[1024]; 


        public RTSPmodel(int p)
        {
            //create and bind the sockets (as seen in Lecture slides - Unit 3)

            this.pn = p;
            try
            {
                this.ip = IPAddress.Parse("127.0.0.1");
            }
            catch (FormatException e)
            {
                //invalid IP
            }
            
            ep = new IPEndPoint(ip, pn);
            try
            {
                RTSP_sock = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);
                RTSP_sock.Bind(ep);
            }
            catch (SocketException e)
            {
                if (RTSP_sock != null) { RTSP_sock.Close(); }
            }
            

            RTSP_sock.Listen(int.MaxValue);
        }

        public Socket Accept_Client()
        {
            //hold the process until connection made (.accept())
            tcp_sock = RTSP_sock.Accept();
            return tcp_sock;
        }

        
        /*
        public void ListenLoop(IPAddress ip, int pn) {

        
        }*/
    }
}
