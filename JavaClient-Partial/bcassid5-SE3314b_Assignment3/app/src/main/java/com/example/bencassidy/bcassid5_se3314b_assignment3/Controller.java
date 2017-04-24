package com.example.bencassidy.bcassid5_se3314b_assignment3;

/**
 * Created by bencassidy on 2017-03-20.
 */

import android.content.Context;

import java.io.ByteArrayInputStream;
import java.io.IOException;
import java.lang.Thread;

import java.net.DatagramPacket;
import java.net.DatagramSocket;
import java.net.Socket;
import java.util.Timer;
import java.util.TimerTask;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;

public class Controller {

    PrimeThread t_server;
    MainView _view;
    public RTSPModel _RTSPModel = null;
    public RTPModel _RTPModel = null;
    DatagramSocket rtsp_sock = null;

    int cseq;
    int sess;
    int port;

    boolean sess_set = true;

    Timer timer;
    MyTask elapsedEvent;

    public Controller(){
        elapsedEvent = new MyTask(_view, _RTPModel);
        timer.schedule(elapsedEvent, 80);
    }

    //implement action listener for the connect button
    public void connect(int p) throws IOException {

        //Run connections **very similar to my server
        _RTSPModel = new RTSPModel(p, "10.0.2.2");
        //connect socket
        rtsp_sock = _RTSPModel.acceptServer();

        /*CHANGE THIS TO JAVA THREAD*/
        //t_server = new Thread(new ParameterizedThreadStart(Main_Handler));
        //set thread to background
        //t_server.IsBackground = true;
        //start the thread from the rtsp socket
        //t_server.Start(rtsp_sock);
        PrimeThread mainThread = new PrimeThread(rtsp_sock);
        mainThread.start();
    }

    public void setup() throws IOException {
        //set initial sequence
        cseq = 1;
        //set new state

        //Set up new rtp (to get port)
        _RTPModel = new RTPModel("10.0.2.2", 4000);
        port = _RTPModel.getPort();

        //run output lines, set vars first
        String r = "SETUP rtsp://10.0.2.2" + ":" + _view.getPort() + "/video3.mjpeg RTSP/1.0\r\n";
        String c = "CSeq: " + cseq++ + "\r\n";
        String s = "Transport: RTP/UDP; client_port= " + port + "\r\n";

        //return final message
        String ret = r + c + s;
        byte[] ret_byte = ret.getBytes();
        DatagramPacket packet = new DatagramPacket(ret_byte, ret_byte.length);
        try {
            rtsp_sock.send(packet);
        } catch (IOException e) {
            e.printStackTrace();
        }

    }
    public void play(){
        //run output lines, set vars first
        String r = "SETUP rtsp://10.0.2.2" + ":" + _view.getPort() + "/video3.mjpeg RTSP/1.0\r\n";
        String c = "CSeq: " + cseq++ + "\r\n";
        String s = "Session: " + sess + "\r\n";

        //return final message
        String ret = r + c + s;
        byte[] ret_byte = ret.getBytes();
        DatagramPacket packet = new DatagramPacket(ret_byte, ret_byte.length);
        try {
            rtsp_sock.send(packet);
        } catch (IOException e) {
            e.printStackTrace();
        }

    }
    public void pause(){
        //run output lines, set vars first
        String r = "SETUP rtsp://10.0.2.2" + ":" + _view.getPort() + "/video3.mjpeg RTSP/1.0\r\n";
        String c = "CSeq: " + cseq++ + "\r\n";
        String s = "Session: " + sess + "\r\n";

        //return final message
        String ret = r + c + s;
        byte[] ret_byte = ret.getBytes();
        DatagramPacket packet = new DatagramPacket(ret_byte, ret_byte.length);
        try {
            rtsp_sock.send(packet);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
    public void teardown(){
        //run output lines, set vars first
        String r = "SETUP rtsp://10.0.2.2" + ":" + _view.getPort() + "/video3.mjpeg RTSP/1.0\r\n";
        String c = "CSeq: " + cseq++ + "\r\n";
        String s = "Session: " + sess + "\r\n";

        //return final message
        String ret = r + c + s;
        byte[] ret_byte = ret.getBytes();
        DatagramPacket packet = new DatagramPacket(ret_byte, ret_byte.length);
        try {
            rtsp_sock.send(packet);
        } catch (IOException e) {
            e.printStackTrace();
        }

        //close the socket
        _RTPModel.close();

        //clear the picture box
        _view.clearImage();
    }
}

class PrimeThread extends Thread {

    DatagramSocket sock;
    PrimeThread(DatagramSocket s){
        sock = s;
    }
    public void run() {
        //networking variables
        byte[] rcv = new byte[256];

        //run similar loop as from server, less info, store all other data in button functions for actions
        try
        {
            //run forever in this loop
            while (true)
            {
                //get the length of the rcv packet into the socket
                int length = sock.getReceiveBufferSize();
                if (length == 0) break; //break if empty
            }
        }
        //catch any socket error
        catch (IOException e)
        {
            if (sock != null) {
                sock.close();
            }
        }
        //close when done
        finally
        {
            sock.close();
        }
    }
}

class MyTask extends TimerTask {

    MainView _view;
    RTPModel _RTPModel = null;


    public MyTask(MainView v, RTPModel r){
        _view = v;
        _RTPModel = r;

    }

    public void run(){

        byte[] rcv_packet = _RTPModel.getData();
        int len = _RTPModel.getLength();

        Bitmap bm;

        ByteArrayInputStream stream = new ByteArrayInputStream(rcv_packet, 12, len - 12);

        bm = BitmapFactory.decodeStream(stream);

        //set this new image to the imageBox
        _view.setImage(bm);
    }
}


