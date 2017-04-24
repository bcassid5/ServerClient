using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace assignment1_bcassid5
{
    class ClientModel
    {
        //declare model variables
        private RTPmodel _RTPmodel;
        //declare other class variables
        Video _video;
        private RTSPtimer _timer;
        //declare int variables for pack
        private int id;
        int interval = 80;
        private int seq;
        
        //constructor to set all of these variables from a pn pass
        public ClientModel(int rand, Video vid, int p)
        {
            id = rand;
            seq = 1;
            _video = vid;
            _timer = new RTSPtimer(rand);
            _timer.Interval = interval;
            _RTPmodel = new RTPmodel(p);
        }
        //constructor to set all of these variables from an RTP model pass
        public ClientModel(int rand, Video vid, RTPmodel rtp)
        {
            id = rand;
            seq = 1;
            _video = vid;
            _RTPmodel = rtp;
            _timer = new RTSPtimer(rand);
            _timer.Interval = interval;
        } 

        //return the rtp model
        public RTPmodel Get_RTP()
        {
            return _RTPmodel;
        }
        //return video
        public Video Get_Video()
        {
            return _video;
        }
        //set video
        public void Set_Video(Video vid)
        {
            _video = vid;
        }
        //return the timer
        public RTSPtimer Get_Timer()
        {
            return _timer;
        }
        public void startTimer()
        {
            _timer.startTime();
        }
        public void stopTimer()
        {
            _timer.stopTime();
        }

        //id functions
        public void Set_ID(int r)
        {
            this.id = r;
        }
        public int Get_ID()
        {
            return id;
        }

        //sequence functions
        public int Get_Seq()
        {
            return seq;
        }
        public void Inc_Seq()
        {
            seq++;
        }
        public void Reset_Seq()
        {
            seq = 1;
        }

    }
}
