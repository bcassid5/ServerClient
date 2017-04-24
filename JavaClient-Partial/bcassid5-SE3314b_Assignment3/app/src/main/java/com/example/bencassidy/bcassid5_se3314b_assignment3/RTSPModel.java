package com.example.bencassidy.bcassid5_se3314b_assignment3;

import java.io.IOException;
import java.net.DatagramSocket;
import java.net.InetAddress;
import java.net.InetSocketAddress;
import java.net.Socket;
import java.net.SocketAddress;
import java.net.UnknownHostException;

/**
 * Created by bencassidy on 2017-03-20.
 */

public class RTSPModel {

    //variables for the ip and port
    private int pn;
    public InetAddress ip;
    //variable for the socket
    DatagramSocket RTSP_sock = null;
    //variable for end point to be set up
    InetSocketAddress ep = null;
    //variable to recieve message
    byte[] rcv = new byte[1024];

    public RTSPModel(int p, String ip) throws IOException {
        this.pn = p;
        //try {
        this.ip = InetAddress.getByName(ip);
        //}
        //catch (FormatException e) { /*invalid IP*/ }
        ep = new InetSocketAddress(this.ip, this.pn);

        //try
        //{
            //ep = new SocketAddress(this.pn, this.ip);
        RTSP_sock = new DatagramSocket(this.pn, this.ip);
        RTSP_sock.connect(ep);
            //RTSP_sock.Bind(ep); copied file from server** minor changes here required
        //}
        /*catch (SocketException e)
        {
            if (RTSP_sock != null) { RTSP_sock.Close(); }
        }*/

        //try { RTSP_sock.Connect(ep); }
        //RTSP_sock.Connect(ep);
        //catch { }
    }

    public DatagramSocket acceptServer()
    {
        //hold the process until connection made (.accept())
        //tcp_sock = RTSP_sock.Accept();
        return RTSP_sock;
    }

}
