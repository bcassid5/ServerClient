namespace assignment1_bcassid5
{
    partial class View
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.portNo = new System.Windows.Forms.Label();
            this.IP = new System.Windows.Forms.Label();
            this.s_status = new System.Windows.Forms.Label();
            this.c_requests = new System.Windows.Forms.Label();
            this.listen = new System.Windows.Forms.Button();
            this.portNum = new System.Windows.Forms.TextBox();
            this.ipNum = new System.Windows.Forms.TextBox();
            this.printRTP = new System.Windows.Forms.CheckBox();
            this.server = new System.Windows.Forms.TextBox();
            this.client = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // portNo
            // 
            this.portNo.AutoSize = true;
            this.portNo.Location = new System.Drawing.Point(13, 28);
            this.portNo.Name = "portNo";
            this.portNo.Size = new System.Drawing.Size(107, 20);
            this.portNo.TabIndex = 0;
            this.portNo.Text = "Listen on Port";
            // 
            // IP
            // 
            this.IP.AutoSize = true;
            this.IP.Location = new System.Drawing.Point(13, 75);
            this.IP.Name = "IP";
            this.IP.Size = new System.Drawing.Size(137, 20);
            this.IP.TabIndex = 1;
            this.IP.Text = "Server IP Address";
            // 
            // s_status
            // 
            this.s_status.AutoSize = true;
            this.s_status.Location = new System.Drawing.Point(13, 122);
            this.s_status.Name = "s_status";
            this.s_status.Size = new System.Drawing.Size(106, 20);
            this.s_status.TabIndex = 2;
            this.s_status.Text = "Server Status";
            // 
            // c_requests
            // 
            this.c_requests.AutoSize = true;
            this.c_requests.Location = new System.Drawing.Point(17, 381);
            this.c_requests.Name = "c_requests";
            this.c_requests.Size = new System.Drawing.Size(122, 20);
            this.c_requests.TabIndex = 3;
            this.c_requests.Text = "Client Requests";
            // 
            // listen
            // 
            this.listen.BackColor = System.Drawing.SystemColors.Highlight;
            this.listen.Location = new System.Drawing.Point(263, 18);
            this.listen.Name = "listen";
            this.listen.Size = new System.Drawing.Size(105, 41);
            this.listen.TabIndex = 4;
            this.listen.Text = "Listen";
            this.listen.UseVisualStyleBackColor = false;
            // 
            // portNum
            // 
            this.portNum.Location = new System.Drawing.Point(126, 28);
            this.portNum.Name = "portNum";
            this.portNum.Size = new System.Drawing.Size(114, 26);
            this.portNum.TabIndex = 5;
            // 
            // ipNum
            // 
            this.ipNum.Enabled = false;
            this.ipNum.Location = new System.Drawing.Point(172, 68);
            this.ipNum.Name = "ipNum";
            this.ipNum.Size = new System.Drawing.Size(196, 26);
            this.ipNum.TabIndex = 6;
            // 
            // printRTP
            // 
            this.printRTP.AutoSize = true;
            this.printRTP.Location = new System.Drawing.Point(442, 35);
            this.printRTP.Name = "printRTP";
            this.printRTP.Size = new System.Drawing.Size(159, 24);
            this.printRTP.TabIndex = 7;
            this.printRTP.Text = "Print RTP Header";
            this.printRTP.UseVisualStyleBackColor = true;
            // 
            // server
            // 
            this.server.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.server.Location = new System.Drawing.Point(13, 146);
            this.server.Multiline = true;
            this.server.Name = "server";
            this.server.ReadOnly = true;
            this.server.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.server.Size = new System.Drawing.Size(606, 226);
            this.server.TabIndex = 8;
            // 
            // client
            // 
            this.client.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.client.Location = new System.Drawing.Point(13, 405);
            this.client.Multiline = true;
            this.client.Name = "client";
            this.client.ReadOnly = true;
            this.client.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.client.Size = new System.Drawing.Size(606, 230);
            this.client.TabIndex = 9;
            // 
            // View
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(631, 658);
            this.Controls.Add(this.client);
            this.Controls.Add(this.server);
            this.Controls.Add(this.printRTP);
            this.Controls.Add(this.ipNum);
            this.Controls.Add(this.portNum);
            this.Controls.Add(this.listen);
            this.Controls.Add(this.c_requests);
            this.Controls.Add(this.s_status);
            this.Controls.Add(this.IP);
            this.Controls.Add(this.portNo);
            this.Name = "View";
            this.Text = "3314b-Video Streaming (bcassid5)";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label portNo;
        private System.Windows.Forms.Label IP;
        private System.Windows.Forms.Label s_status;
        private System.Windows.Forms.Label c_requests;
        private System.Windows.Forms.Button listen;
        private System.Windows.Forms.TextBox portNum;
        private System.Windows.Forms.TextBox ipNum;
        private System.Windows.Forms.CheckBox printRTP;
        private System.Windows.Forms.TextBox server;
        private System.Windows.Forms.TextBox client;
    }
}

