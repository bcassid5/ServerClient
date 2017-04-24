using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.Net.Sockets;

namespace assignment2_bcassid5
{
    class RTPModel
    {
        //create class variables for and endpoint and for a udp socket
        IPEndPoint ip_ep;
        //Socket UDP_sock;
        //adding more variables that were not in server
        //udp client side vairable
        UdpClient UDP_client;
        //some integer variables to control the port/rcving values
        int pn;
        int rcv_length;

        //constructor to create the udp socket (as seen in lecture slides - unit 3)
        public RTPModel (string new_ip)
        {
            try
            {
                //parse the ip address
                IPAddress ip = IPAddress.Parse(new_ip);
                //IPEndPoint new_ep = new IPEndPoint(ip, pn); **not needed from server
                UDP_client = new UdpClient(0);
                ip_ep = new IPEndPoint(IPAddress.Any, 0);//= new_ep; ** not needed from server
                //UDP_sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp); ** not needed from server
                pn = ((IPEndPoint)UDP_client.Client.LocalEndPoint).Port;
            }
            catch { }

        }

        //function to send udp datagram
        /*
        internal void Send_Data(byte[] b)
        {
            UDP_sock.SendTo(b, ip_ep);
        } NOT NEEDED FROM SERVER **/
        public byte[] Get_Data()
        {
            Byte[] ret = UDP_client.Receive(ref ip_ep);
            rcv_length = ret.Length;
            return ret;
            //Byte[] ret;
            //Got exception here ONE time during testing, never again? 
            /*try
            {
                Byte[] ret = UDP_client.Receive(ref ip_ep);
                rcv_length = ret.Length;
                return ret;
            }   //{ ret = UDP_client.Receive(ref ip_ep); }
            catch (SocketException e) { //some kind of socket exception pops up here }//exception unhandled here once... not sure on error
             */   
        }
        //function to return the socket
        /*
        public Socket Get_Socket()
        {
            return UDP_sock;
        } NOT NEEDED FROM SERVER*/
        //function to return the endpoint of the udp socket
        public IPEndPoint Get_IPEndPoint()
        {
            return ip_ep;
        }
        
        public void Close()
        {
            UDP_client.Close();
        }
        public int Get_Port()
        {
            return pn;
        }
        public int Get_Length()
        {
            return rcv_length;
        }
    }
}
