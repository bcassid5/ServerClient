using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace assignment1_bcassid5
{
    class RTPpacket
    {
        byte[] head;
        byte[] pack;
        byte[] data;
        private byte[] arra;

        private int interval = 100;
        private int seq;
        private int length;

        //constructor 1
        public RTPpacket(byte[] d, int s)
        {
            int ts = s * interval;
            head = Create_Header(s, ts);
            data = new byte[d.Length];
            data = d;
            pack = new byte[d.Length + 12];
            for (int i = 0; i < 12; i++)
            {
                pack[i] = head[i];
            }
            for (int i = 12; i < d.Length + 12; i++)
            {
                pack[i] = data[i - 12];
            }
        }
        //constructor 2
        public RTPpacket(byte[] d, int s, int l)
        {
            arra = d;
            seq = s;
            length = l;

            int ts = s * interval;
            head = Create_Header(s, ts);
            data = new byte[d.Length];
            data = d;
            pack = new byte[d.Length + 12];
            for (int i = 0; i < 12; i++)
            {
                pack[i] = head[i];
            }
            for (int i = 12; i < d.Length + 12; i++)
            {
                pack[i] = data[i - 12];
            }
        }
        //create a header - pay attention to the byte positions
        public byte[] Create_Header(int s, int ts)
        {
            int V = 2;
            int P = 0;
            int X = 0;
            int CC = 0;
            int M = 0;
            int PT = 26;
            int sequence = s;
            long time = ts;
            long ssrc = 0;

            byte[] ret = new byte[12];

            //set all bytes to proper spots... (found help)
            ret[0] = (byte)((V & 0x3) << 6 | (P & 0x1) << 5 | (X & 0x0) << 4 | (CC & 0x0));
            ret[1] = (byte)((M & 0x1) << 7 | PT & 0x7f);
            ret[2] = (byte)((sequence & 0xff00) >> 8);
            ret[3] = (byte)(sequence & 0x00ff);
            ret[4] = (byte)((time & 0xff000000) >> 24);
            ret[5] = (byte)((time & 0x00ff0000) >> 16);
            ret[6] = (byte)((time & 0x0000ff00) >> 8);
            ret[7] = (byte)(time & 0x000000ff);
            ret[8] = (byte)((ssrc & 0xff000000) >> 24);
            ret[9] = (byte)((ssrc & 0x00ff0000) >> 16);
            ret[10] = (byte)((ssrc & 0x0000ff00) >> 8);
            ret[11] = (byte)(ssrc & 0x000000ff);

            return ret;
        }
        //return bytes
        public byte[] Get_Bytes()
        {
            return pack;
        }
        //return header
        public string Get_Header()
        {
            var ret = string.Concat(head.Select(b => Convert.ToString(b, 2).PadLeft(8, '0').PadRight(9, ' ')));
            return ret;
        }
    }
}
