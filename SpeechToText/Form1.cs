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



namespace SpeechToText
{
    public partial class Form1 : Form
    {//новый коммент
        //коменьт2
        Timer timer01 = new Timer();
        //wmp player
        public WMPLib.WindowsMediaPlayer WMP = new WMPLib.WindowsMediaPlayer();
        //SoundPlayer sp = new SoundPlayer("1_converted.wav");
        bool b = false;
        WaveIn waveIn;
        WaveFileWriter writer;
        string outputFilename = "demo.wav";
        bool ON = false;
        
        public String MusicFile { get; set; }
        string[] patters = {"Шла Саша по шоссе и сосала сушку",
            "Течет речка печет печка",
            "скороговорки как караси на сковородке",
            "пакет под попкорн",
            "Осип охрип Архип Осип",
            "сачок зацепился за сучок",
            "Цапля чахла цапля сохла цапля сдохла",
            "Ехал Грека через реку видит Грека",
            "прецедент с претендентом",
            "жутко жуку жить на суку"};
        Random rnd = new Random();
         int num_putter;

        public Form1()
        {
            InitializeComponent();
            this.BackColor = Color.FromArgb(25, 30, 49);
            //кнопка записи
            button2.BackColor = Color.FromArgb(44, 51, 80);
            System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
            Rectangle pathRect = new Rectangle(4, 4, 142, 42);
            path.AddRectangle(pathRect);
            Region rgn = new Region(path);
            button2.Region = rgn;
        
            //Скрываем кнопки и textbox
            button2.Visible = false;
            textBox1.Visible = false;
            textBox2.Visible = false;
            textBox3.Visible = false;


            label6.ForeColor = Color.White;
            //кнопка завести будильник
            button3.BackColor = Color.FromArgb(44, 51, 80);
            button3.Region = rgn;
            //установка времени
            maskedTextBox1.ForeColor = Color.FromArgb(25, 30, 49);
            textBox1.ForeColor = Color.FromArgb(25, 30, 49);
            textBox2.ForeColor = Color.White;
            textBox2.BackColor = Color.FromArgb(25, 30, 49);
            //цвет индикатора
            textBox3.ForeColor = Color.FromArgb(255, 123, 91);
        }
        
        

        public void visibleNone()
        {
            this.Visible = false;
        }



        /* private void button1_Click(object sender, EventArgs e)
         {
             if (ON == false)
             {
                 sp.Stop();
                 waveIn = new WaveIn();
                 waveIn.DeviceNumber = 0;
                 waveIn.DataAvailable += waveIn_DataAvailable;
                 waveIn.RecordingStopped += new EventHandler<NAudio.Wave.StoppedEventArgs>(waveIn_RecordingStopped);
                 waveIn.WaveFormat = new WaveFormat(16000, 1);
                 writer = new WaveFileWriter(outputFilename, waveIn.WaveFormat);
                 label2.Text = "Идет запись...";
                 button1.Text = "Стоп";
                 waveIn.StartRecording();
                 ON = true;

             }
             else
             {

                 sp.Play();    
                 waveIn.StopRecording();
                 writer.Close();
                 label2.Text = "";
                 ON = false;
                 button1.Text = "Запись";
                 //button2_Click(this, EventArgs.Empty);
                 button1.Text = "Распознавание";

             }


         }*/




        private void button2_Click(object sender, EventArgs e)
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
                button2.Text = "Стоп";
                waveIn.StartRecording();
                ON = true;
               

            }
            else
            {
                WMP.controls.play();
                waveIn.StopRecording();
                writer.Close(); 
                label2.Text = "";
                ON = false;
                button2.Text = "Запись";
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
                // Read the content.
                // label1.Text = reader.ReadToEnd();
                // Clean up the streams.


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
                    maskedTextBox1.Visible = true;
                    maskedTextBox1.Text = "00:00";
                    button3.Text = "Завести будильник";
                    //скрываем кнопки
                    button2.Visible = false;
                    textBox1.Visible = false;
                   
                    //for (int x = 0; x < 20; x++)
                    //{
                    //    button3.Location = new Point(button3.Location.X, button3.Location.Y+x);
                    //}
                    //textBox2.Visible = false;

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

        private void maskedTextBox1_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }
        

       

        


      

        private void button3_Click(object sender, EventArgs e)
        {
            if (b == false)
            {
                string getTime = maskedTextBox1.Text.ToString();
                int topHours = 23;
                int topMinutes = 59;

                string[] hoursMinutes = getTime.Split(':');
                int hours = Int16.Parse(hoursMinutes[0]);
                int minutes = Int16.Parse(hoursMinutes[1]);
                //  DateTime d1 = new DateTime(0, 0, 0, 23, 59, 0, 0);
                if (topHours >= hours && topMinutes >= minutes)
                {
                    label4.Text = maskedTextBox1.Text;
                    timer2.Start();
                    maskedTextBox1.Visible = false;
                    button3.Text = "Убрать будильник";
                    b = true;
                    textBox3.Visible = false;
                }
                else
                {
                    //если ввели неправильное время
                     MessageBox.Show("Вы ввели некорректное время, повторите ввод", "Error",
                       MessageBoxButtons.OK);

                          
                    //textBox3.Visible = true;
                }

            }
            else if (b == true)
            {
                label4.Text = "00:00";
                timer2.Stop();
                maskedTextBox1.Visible = true;
                button3.Text = "Завести будильник";
                b = false;
            }           
            
        }
        

//        private void button4_Click(object sender, EventArgs e)
//        {
//            sp.Stop();
//            button4.Enabled = false;
//            maskedTextBox1.Visible = true;
//            maskedTextBox1.Text = "00:00";
//            button3.Text = "Завести будильник";
//            
//            b = false;
//        }

        private void Form1_Load_1(object sender, EventArgs e)
        {

           // button4.Enabled = false;
            timer01.Interval = 1000;
            timer01.Tick += new EventHandler(timer1_Tick_1);
            timer01.Start();
        }

        private void timer2_Tick_1(object sender, EventArgs e)
        {
            if (label3.Text == label4.Text + ":00")
            {
                WMP.controls.play();
                //Показываем кнопки
                textBox1.Visible = true;
                button2.Visible = true;
                //вывод скороговорки
                num_putter = rnd.Next(0, 9);
                textBox1.TextAlign = HorizontalAlignment.Center;
                textBox1.Text = patters[num_putter];
             

            }
        }

        private void timer1_Tick_1(object sender, EventArgs e)
        {
            label3.Text = DateTime.Now.Hour.ToString("00") + ":" + DateTime.Now.Minute.ToString("00") + ":" + DateTime.Now.Second.ToString("00");
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void maskedTextBox1_MaskInputRejected_1(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }
        //выбор звукового файла
        private void btnMusic_Click(object sender, EventArgs e)
        {
            
        

           if (openFileDialog1.ShowDialog() == DialogResult.OK)
                WMP.URL = openFileDialog1.FileName;
            WMP.settings.volume = 100;
              WMP.controls.stop();

        }
        public void StopMusic()
        {
            WMP.controls.stop();
        }

           
    }
}
