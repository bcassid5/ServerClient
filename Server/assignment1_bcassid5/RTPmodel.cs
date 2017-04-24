using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace assignment1_bcassid5
{
    class RTPmodel
    {
        //create class variables for and endpoint and for a udp socket
        IPEndPoint ep;
        Socket UDP_sock;

        //constructor to create the udp socket (as seen in lecture slides - unit 3)
        public RTPmodel (int pn)
        {
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            IPEndPoint new_ep = new IPEndPoint(ip, pn);
            ep = new_ep;
            UDP_sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp); 
        }

        //function to send udp datagram
        internal void Send_Data(byte[] b)
        {
            UDP_sock.SendTo(b, ep);
        }
        //function to return the socket
        public Socket Get_Socket()
        {
            return UDP_sock;
        }
        //function to return the endpoint of the udp socket
        public IPEndPoint Get_EndPoint()
        {
            return ep;
        }
        
    }
}
