using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NAudio;
using NAudio.Wave;
using System.Net;
using System.IO;
using System.IO.Compression;
using System.Media;

using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;


namespace SpeechToText
{
   
    public partial class Form1 : Form
    {
 
        Timer timer01 = new Timer();
        //wmp player       
        public WMPLib.WindowsMediaPlayer WMP = new WMPLib.WindowsMediaPlayer();
        public WMPLib.WindowsMediaPlayer WMPL = new WMPLib.WindowsMediaPlayer();       
        //SoundPlayer sp = new SoundPlayer("1_converted.wav");
        bool b = false;
        WaveIn waveIn;
        WaveFileWriter writer;
        string outputFilename = "demo.wav";
        bool ON = false;
        bool stop = false;
         int Hutc;
        string musicUrl;
        private static int h;

        // string[] patters;
        List<string> patters = new List<string>();
        Random rnd = new Random();
        int num_putter;
        public Form1()
        {
            InitializeComponent();
            h = DateTime.Now.Hour;//установка времени
            Data.EventHandler = new Data.MyEvent(funcWMP);
            //Скрываем кнопки и textbox
            btnRecord.Visible = false;
            showPatterTB.Visible = false;
            textBox2.Visible = false;
            //Установка стандарной аудио
            WMP.URL = "standart.mp3";
            WMP.controls.stop();
            WMPL.URL = "standart.mp3";
            WMPL.controls.stop();
            // Объект запроса
            try
            {
                HttpWebRequest rew = (HttpWebRequest)WebRequest.Create("http://studypay.ru/alarmclock.php");
                HttpWebResponse resp = (HttpWebResponse)rew.GetResponse();
                WebHeaderCollection myWebHeaderCollection = resp.Headers;
                rew.Accept = "*/*";
                rew.Referer = "";
                rew.Headers.Add("Accept-Language", "ru");
                rew.Headers.Add("Accept-Encoding", "gzip, deflate");
                StreamReader str = new StreamReader(rew.GetResponse().GetResponseStream(), Encoding.UTF8);
                //Encoding.UTF8, Encoding.Unicode, Encoding.ASCII .... 
                string massage = str.ReadToEnd();
                string a = massage.Trim(new Char[] { '[', ']' });
                char[] delimiterChars = { ',' };
                string[] words = a.Split(delimiterChars);
                for (int i = 0; i < words.Length; i++)
                {
                    int n = words[i].Length;
                    string l = words[i].Remove(n - 1, 1);
                    patters.Add(l.Remove(0, 1));
                }
                str.Close();
            }
            catch (WebException)
            {
                if (MessageBox.Show(
                        "Отсутствует подключение к интернету, воспользуйтесь напоминанием",
                        "Будильник",
                        MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    this.tabControl1.SelectedTab = tabPage2;
                    tabPage1.Enabled = false;
                }
                else Environment.Exit(0);
            }
        }

        private void SpeechToTex()
        {
            WMPL.URL = openFileDialog1.FileName;
            WMP.controls.play();
            waveIn.StopRecording();
            writer.Close();
            label2.Text = "";
            ON = false;
            btnRecord.Text = "Запись";
            //ОТПРАВКА НА GSPEECH
            WebRequest request =
                WebRequest.Create(
                    "https://www.google.com/speech-api/v2/recognize?output=json&lang=ru-RU&key=AIzaSyBOti4mM-6x9WDnZIjIeyEU21OpBXqWBgw");
            request.Method = "POST";
            byte[] byteArray = File.ReadAllBytes(outputFilename);
            request.ContentType = "audio/l16; rate=16000"; //"16000";
            request.ContentLength = byteArray.Length;
            request.GetRequestStream().Write(byteArray, 0, byteArray.Length);
            // Get the response.
            HttpWebResponse response = (HttpWebResponse) request.GetResponse();
            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string a = String.Copy(reader.ReadToEnd());
            
            //*************ПРОВЕРКА НА ПРАВИЛЬНОСТЬ ПРОИЗНОШЕНИЯ*************
            if (a.Contains(patters[num_putter]) == true)
            {
                //label6.Text = "Правильно!"; // Yes
                // label6.TextAlign = ContentAlignment.TopCenter;
                textBox2.Visible = true;
                textBox2.Text = "Правильно!";
                textBox2.TextAlign = HorizontalAlignment.Center;
                timer1.Stop();
                WMP.controls.stop();
                maskedshowPatterTB.Visible = true;
                maskedshowPatterTB.Text = "00:00";
                btnSetAlarm.Text = "Завести будильник";
                //скрываем кнопки
                btnRecord.Visible = false;
                showPatterTB.Visible = false;
            }
            else
            {
                textBox2.Visible = true;
                textBox2.Text = "Неправильно, запишите снова."; //  No, ofcourse
                textBox2.TextAlign = HorizontalAlignment.Center;
            }

            reader.Close();
            response.Close();
        }
        void waveIn_DataAvailable(object sender, WaveInEventArgs e)
        {
            writer.WriteData(e.Buffer, 0, e.BytesRecorded);
        }


        void waveIn_RecordingStopped(object sender, EventArgs e)
        {
            waveIn.Dispose();
            waveIn = null;
            writer.Close();
            writer = null;
        }
        private void Form1_Load_1(object sender, EventArgs e)
        {

            // button4.Enabled = false;
            timer01.Interval = 1000;
            timer01.Tick += new EventHandler(timer1_Tick_1);
            timer01.Start();
        }
        private void timer1_Tick_1(object sender, EventArgs e)
        {

            label3.Text = DateTime.Now.Hour.ToString("00") + ":" + DateTime.Now.Minute.ToString("00") + ":" +
                          DateTime.Now.Second.ToString("00");        
             label5.Text = h.ToString("00") + ":" + DateTime.UtcNow.Minute.ToString("00") + ":" +
                 DateTime.UtcNow.Second.ToString("00");
        }
        public void StopMusic()
        {
            WMP.controls.stop();
        }
        private void btnRecord_Click_1(object sender, EventArgs e)
        {

            if (ON == false)
            {
                WMP.controls.stop();
                waveIn = new WaveIn();
                waveIn.DeviceNumber = 0;
                waveIn.DataAvailable += waveIn_DataAvailable;
                waveIn.RecordingStopped += new EventHandler<NAudio.Wave.StoppedEventArgs>(waveIn_RecordingStopped);
                waveIn.WaveFormat = new WaveFormat(16000, 1);
                writer = new WaveFileWriter(outputFilename, waveIn.WaveFormat);
                label2.Text = "Идет запись...";
                btnRecord.Text = "Стоп";
                waveIn.StartRecording();
                ON = true;
            }
            else
            {
                SpeechToTex();
            }
        }

        private void btnSetNote_Click_1(object sender, EventArgs e)
        {
            if (b == false)
            {
                string getTime = maskedshowPatterTB.Text.ToString();
                int topHours = 23;
                int topMinutes = 59;
                string[] hoursMinutes = getTime.Split(':');
                int hours = Int16.Parse(hoursMinutes[0]);
                int minutes = Int16.Parse(hoursMinutes[1]);
                if (topHours >= hours && topMinutes >= minutes)
                {
                    errorProvider1.Clear();

                    label4.Text = maskedshowPatterTB.Text;
                    timer2.Start();
                    maskedshowPatterTB.Visible = false;
                    btnSetAlarm.Text = "Убрать будильник";
                    b = true;
                }
                else
                {
                    errorProvider1.SetError(maskedshowPatterTB, "Вы ввели некорректное время, повторите ввод");
                }

            }
            else if (b == true)
            {
                label4.Text = "00:00";
                timer2.Stop();
                maskedshowPatterTB.Visible = true;
                btnSetAlarm.Text = "Завести будильник";
                b = false;
            }
        }
        private void timer2_Tick(object sender, EventArgs e)
        {
            if (label3.Text == label4.Text + ":00")
            {          
                WMP.controls.play();
                //Показываем кнопки
                showPatterTB.Visible = true;
                btnRecord.Visible = true;
                //вывод скороговорки
                num_putter = rnd.Next(0, 9);
                showPatterTB.TextAlign = HorizontalAlignment.Center;
                showPatterTB.Text = patters[num_putter];
            }          
        }
        private void btnSetNote_Click(object sender, EventArgs e)
        {         
            if (b == false)
            {
                string getTime1 = maskedTextBox2.Text.ToString();
                int topHours1 = 23;
                int topMinutes1 = 59;
                string[] hoursMinutes = getTime1.Split(':');
                int hours = Int16.Parse(hoursMinutes[0]);
                int minutes = Int16.Parse(hoursMinutes[1]);
                if (topHours1 >= hours && topMinutes1 >= minutes)
                {
                    errorProvider1.Clear();
                    label7.Text = maskedTextBox2.Text;
                    timer3.Start();
                    maskedTextBox2.Visible = false;
                    textBox4.Visible = false;
                    AddNoteLabel.Visible = false;
                    btnSetNote.Text = "Убрать будильник";
                    b = true;
                }
                else
                {
                    errorProvider1.SetError(maskedTextBox2, "Вы ввели некорректное время, повторите ввод");
                }                
            }
            else if (b == true)
            {
                label7.Text = "00:00";
                timer3.Stop();
                maskedTextBox2.Visible = true;
                textBox4.Visible = true;
                AddNoteLabel.Visible = true;
                btnSetNote.Text = "Завести будильник";
                b = false;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
                WMPL.URL = openFileDialog1.FileName;
            WMPL.settings.volume = 100;
            WMPL.controls.stop();
        }
        private void button5_Click(object sender, EventArgs e)
        {
            WMPL.controls.stop();
            maskedTextBox2.Visible = true;
            maskedTextBox2.Text = "00:00";
            btnSetNote.Text = "Завести будильник";
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            string reminder = textBox4.Text;   
             if (label5.Text == label7.Text + ":00"&& !stop)
             {

                 stop = true;
                WMPL.controls.play();
                 if (MessageBox.Show(
                         reminder,
                         "Будильник",
                         MessageBoxButtons.OK) == DialogResult.OK)
                 {
                     WMPL.controls.stop();
                     maskedTextBox2.Visible = true;
                     maskedTextBox2.Text = "00:00";
                     textBox4.Visible = true;
                     textBox4.Text = "";
                     btnSetNote.Text = "Завести будильник";
                     b = false;
                     stop = false;
                 }
            }
            
        }
        public void funcWMP(WMPLib.WindowsMediaPlayer WMP, int hT, int num)
        {
            if (num==0)
            {
                this.WMP = WMP;
            }
            if (num == 1)
            {
                this.WMPL = WMP;
                if ((hT + DateTime.UtcNow.Hour) > 23)
                {
                    hT = (hT + DateTime.UtcNow.Hour) - 24;

                }
                else if ((DateTime.UtcNow.Hour + hT) < 0) hT = (hT + DateTime.UtcNow.Hour) + 24;
                else hT = (hT + DateTime.UtcNow.Hour);
            }
            AddNoteLabel.Text = hT.ToString();
            
            h = hT;
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            Form2 settingForm = new Form2(0);
           
            settingForm.ShowDialog();
        }

        private void btnSettings2_Click(object sender, EventArgs e)
        {
            Form2 settingForm = new Form2(1);

            settingForm.ShowDialog();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
