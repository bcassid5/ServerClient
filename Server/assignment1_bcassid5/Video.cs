using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace assignment1_bcassid5
{
    class Video
    {
        private string file;
        FileStream fileStream;

        //constructor for video
        public Video(string n)
        {
            this.file = n;
            //set path (make sure to include in a read me)
            string t = @"c:\3314vids";
            //set the enviro path to this string for getting the video files
            Environment.CurrentDirectory = (t);
            //set the stream to be able to open and read
            fileStream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read);

        }

        public void Make_Video()
        {
            this.fileStream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read);
        }
        public void Trash_Video()
        {
            this.fileStream.Dispose();
            this.fileStream.Close();
        }
        public string Get_Name()
        {
            return file;
        }
        public byte[] Get_NextFrame()
        {
            int length = 0;
            byte[] frame_length = new byte[5];

            //read current frame length
            fileStream.Read(frame_length, 0, 5);

            //transform frame_length to integer
            try
            {
                length = int.Parse(System.Text.Encoding.UTF8.GetString(frame_length));
            }
            catch (FormatException exc)
            {
                //string doesn't exist because the file's over
                length = 0;
            }

            byte[] ret = null;
            if (length > 0)
            {
                ret = new byte[length];
                fileStream.Read(ret, 0, length);
            }
            else
            {
                fileStream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read);
                ret = Get_NextFrame();
            }

            return ret;
        }
    }
}
