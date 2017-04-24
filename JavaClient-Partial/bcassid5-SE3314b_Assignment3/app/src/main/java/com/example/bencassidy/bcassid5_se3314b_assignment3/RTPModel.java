package com.example.bencassidy.bcassid5_se3314b_assignment3;

import java.io.IOException;
import java.net.DatagramPacket;
import java.net.DatagramSocket;
import java.net.InetAddress;
import java.net.InetSocketAddress;
import java.net.SocketException;
import java.net.UnknownHostException;

/**
 * Created by bencassidy on 2017-03-20.
 */


public class RTPModel {

    InetSocketAddress ip_ep;
    DatagramSocket socket;
    byte[] data;
    int length;
    int pn;

    public RTPModel(String s, int p) {
        try
        {
            //parse the ip address
            InetAddress ip = InetAddress.getByName(s);
            pn = p;
            //IPEndPoint new_ep = new IPEndPoint(ip, pn); **not needed from server
            socket = new DatagramSocket(pn, ip);
            //UDP_client = new UdpClient(0);
            //ip_ep = new InetSocketAddress(InetSocketAddress.Any, 0);//= new_ep; ** not needed from server
            //UDP_sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp); ** not needed from server
            //pn = ((IPEndPoint)UDP_client.Client.LocalEndPoint).Port;
        }
        catch (UnknownHostException e) {
            e.printStackTrace();
        } catch (SocketException e) {
            e.printStackTrace();
        }
    }

    public byte[] getData() {
        return data;
    }

    public int getLength() {
        return length;
    }


    public byte[] Get_Data() throws IOException {
        //Byte[] ret = socket.receive(ip_ep);
        //socket.connect(new InetSocketAddress(5000));
        byte[] ret = new byte[512];
        DatagramPacket packet = new DatagramPacket(ret, ret.length);

        socket.receive(packet);

        length = ret.length;
        return ret;
    }

    public InetSocketAddress getEP()
    {
        return ip_ep;
    }

    public void close()
    {
        socket.close();
    }
    public int getPort()
    {
        return pn;
    }

}
