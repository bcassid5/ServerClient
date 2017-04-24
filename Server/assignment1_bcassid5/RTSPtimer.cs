using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace assignment1_bcassid5
{
    class RTSPtimer : System.Timers.Timer
    {
        private int id;
        private int inter = 80;

        //constructor that takes an id
        public RTSPtimer(int id)
        {
            this.id = id;
            this.Interval = inter;
        }

        //function to enable the time
        public void startTime()
        {
            this.Enabled = true;
        }
        public void stopTime()
        {
            this.Enabled = false;
        }

        //functions to get and set id
        public void Set_ID(int id)
        {
            this.id = id;
        }
        public int Get_ID()
        {
            return id;
        }
    }
}
