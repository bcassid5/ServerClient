using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace assignment2_bcassid5
{
    public partial class View : Form
    {

        //instantiate controller object
        Controller _controller;

        //instantiate text setting delegate **
        delegate void SetTextCallback(string text);

        string vid = "video1.mjpeg";

        public View()
        {
            InitializeComponent();
            _controller = new Controller();
            
            this.connectBtn.Click += new System.EventHandler(_controller.Connect_Click);
            /*
             ATTEMPTED OUDA/SERVER METHOD
             -unclear on errors? see bottom of file for new implementation, still used by controller, not view
             -error: all button functions in controller called at on connect? fixed by changing this implementation
             */
            //this.connectBtn.Click += new System.EventHandler(_controller.Setup_Click);
            //this.connectBtn.Click += new System.EventHandler(_controller.Play_Click);
            //this.connectBtn.Click += new System.EventHandler(_controller.Pause_Click);
            //this.connectBtn.Click += new System.EventHandler(_controller.Teardown_Click);
            //this.vidName.SelectedIndexChanged += new System.EventHandler(_controller.Set_VideoName);
            this.vidName.SelectedIndex = 0;

            //disable buttons
            this.setupBtn.Enabled = false;
            this.playBtn.Enabled = false;
            this.pauseBtn.Enabled = false;
            this.teardownBtn.Enabled = false;
        }

        public void Disable()
        {
            this.connectBtn.Enabled = false;
            //disable buttons
            this.setupBtn.Enabled = true;
            this.playBtn.Enabled = false;
            this.pauseBtn.Enabled = false;
            this.teardownBtn.Enabled = false;
        }
        public void Enable()
        {
            this.connectBtn.Enabled = true;

            //disable buttons
            this.setupBtn.Enabled = false;
            this.playBtn.Enabled = false;
            this.pauseBtn.Enabled = false;
            this.teardownBtn.Enabled = false;
        }

        private void exitBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //return port and IP
        public string Get_Port()
        {
            return this.portNum.Text;
        }
        public string Get_IP()
        {
            return this.ipNum.Text;
        }

        public bool Get_PrintHeadStatus()
        {
            return this.headerChk.Checked;
        }
        public bool Get_PackStatus()
        {
            return this.reportChk.Checked;
        }
        /*FROM SERVER - SIMILAR DELEGATE FUNCTIONS*/
        //regular client updates 
        public void Add_Client(String mess)
        {
            this.cText.Text += mess;
        }
        //regular server updates 
        public void Add_Server(String mess)
        {
            this.sText.Text += mess;
        }

        //delegate client updates
        public void Add_Client_del(String mess)
        {
            SetTextCallback new_t = new SetTextCallback(Add_Client);
            this.Invoke(new_t, new Object[] { mess });
        }
        //delegate client updates
        public void Add_Server_del(String mess)
        {
            SetTextCallback new_t = new SetTextCallback(Add_Server);
            this.Invoke(new_t, new Object[] { mess });
        }


        //controls for the image box
        public void Set_Image(Bitmap bm)
        {
            this.imageBox.Image = (Image)new Bitmap(bm, imageBox.Size);
        }
        public void Clear_Image()
        {
            this.imageBox.Image = null;
        }

        private void vidName_SelectedIndexChanged(object sender, EventArgs e)
        {
            vid = vidName.SelectedItem.ToString();
        }
        public string Get_Video()
        {
            return vid;
        }

        /*
         HAVING MAJOR BUTTON ISSUES - WORKING ON FIX HERE
             setting buttons disabled because of probably errors -.-
             */

        private void setupBtn_Click(object sender, EventArgs e)
        {
            _controller.Setup_Click(sender, e);
            //disable buttons
            this.setupBtn.Enabled = false;
            this.playBtn.Enabled = true;
            this.pauseBtn.Enabled = false;
            this.teardownBtn.Enabled = true;
        }

        private void playBtn_Click(object sender, EventArgs e)
        {
            _controller.Play_Click(sender, e);

            //disable buttons
            this.setupBtn.Enabled = false;
            this.playBtn.Enabled = false;
            this.pauseBtn.Enabled = true;
            this.teardownBtn.Enabled = true;
        }

        private void pauseBtn_Click(object sender, EventArgs e)
        {
            _controller.Pause_Click(sender, e);

            //disable buttons
            this.setupBtn.Enabled = false;
            this.playBtn.Enabled = true;
            this.pauseBtn.Enabled = false;
            this.teardownBtn.Enabled = true;
        }

        private void teardownBtn_Click(object sender, EventArgs e)
        {
            _controller.Teardown_Click(sender, e);

            //disable buttons
            this.setupBtn.Enabled = false;
            this.playBtn.Enabled = false;
            this.pauseBtn.Enabled = false;
            this.teardownBtn.Enabled = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.cText.Text = "";
            this.sText.Text = "";
        }
    }
}
