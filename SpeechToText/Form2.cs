using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
namespace SpeechToText
{
    public  class Form2:Form

    {
        private ComboBox comboBoxGTM;
        private Button btnMusic;
        public WMPLib.WindowsMediaPlayer WMP = new WMPLib.WindowsMediaPlayer();
        private OpenFileDialog openFileDialog1;
        private Button btnSave;
        public int num;
        string musicUrl;
        List<Gtms> gtms = new List<Gtms>
             {
                 new Gtms {Gt = 6, City = "Омск"},
                 new Gtms {Gt = 12, City = "Камчатка, Чукотка"},
                 new Gtms {Gt = 10, City = "Магадан, Верхоянск, Сахалин, Владивосток"},
                 new Gtms {Gt = 9, City = "Якутск"},
                 new Gtms {Gt = 8, City = "Иркутск"},
                 new Gtms {Gt = 7, City = "Красноярск"},
                 new Gtms {Gt = 5, City = "Екатеринбург"},
                 new Gtms {Gt = 4, City = "Самара, Ижевск"},
                 new Gtms {Gt = 3, City = "Москва  Московское время (MSK)"},
                 new Gtms
                 {
                     Gt = 1,
                     City = "Париж Среднеевропейское (Центральноевропейское) время (CET - Central Europe Time Zone)"
                 },
                 new Gtms {Gt = 0, City = "Лондон Гринвичское время Западноевропейское время"},
                 new Gtms {Gt = -2, City = "Среднеатлантическое время"},
                 new Gtms {Gt = -3, City = "Аргентина, Буэнос-Айрес"},
                 new Gtms {Gt = -4, City = "Канада,  Атлантическое время"},
                 new Gtms {Gt = -5, City = "С.Ш.А, Нью-Йорк.  Восточное время (EST - US Eastern Savings Time Zone)"},
                 new Gtms {Gt = -6, City = "Чикаго (Chicago). Центральное время (CST - US Central Time)"},
                 new Gtms {Gt = -7, City = "Денвер (Denver), Горное время (MST - US Mountain Time)"},
                 new Gtms
                 {
                     Gt = -8,
                     City = "США, Лос-Анджелес, Сан-Франциско. Тихоокеанское время (PST - Pacific Savings Time)"
                 }
             };


        public Form2(int num)
         {
             InitializeComponent();

            WMP.URL = "standart.mp3";
            WMP.controls.stop();
            

            this.num = num;
             comboBoxGTM.SelectedIndexChanged += comboBoxGTM_SelectedIndexChanged;
             comboBoxGTM.DataSource = gtms;
             comboBoxGTM.DisplayMember = "City";
             comboBoxGTM.ValueMember = "Gt";





         }
        class Gtms
        {
            public int Gt { get; set; }
            public string City { get; set; }
            
        }

         private void InitializeComponent()
         {
            this.btnMusic = new System.Windows.Forms.Button();
            this.comboBoxGTM = new System.Windows.Forms.ComboBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.btnSave = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnMusic
            // 
            this.btnMusic.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(45)))), ((int)(((byte)(71)))));
            this.btnMusic.FlatAppearance.BorderSize = 0;
            this.btnMusic.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMusic.Font = new System.Drawing.Font("Roboto", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnMusic.ForeColor = System.Drawing.Color.White;
            this.btnMusic.Location = new System.Drawing.Point(80, 82);
            this.btnMusic.Name = "btnMusic";
            this.btnMusic.Size = new System.Drawing.Size(121, 34);
            this.btnMusic.TabIndex = 0;
            this.btnMusic.Text = "Мелодия";
            this.btnMusic.UseVisualStyleBackColor = false;
            this.btnMusic.Click += new System.EventHandler(this.btnMusic_Click);
            // 
            // comboBoxGTM
            // 
            this.comboBoxGTM.BackColor = System.Drawing.SystemColors.Window;
            this.comboBoxGTM.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboBoxGTM.Font = new System.Drawing.Font("Roboto", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.comboBoxGTM.FormattingEnabled = true;
            this.comboBoxGTM.Location = new System.Drawing.Point(80, 44);
            this.comboBoxGTM.Name = "comboBoxGTM";
            this.comboBoxGTM.Size = new System.Drawing.Size(121, 23);
            this.comboBoxGTM.TabIndex = 1;
            this.comboBoxGTM.SelectedIndexChanged += new System.EventHandler(this.comboBoxGTM_SelectedIndexChanged);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(45)))), ((int)(((byte)(71)))));
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Roboto", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnSave.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnSave.Location = new System.Drawing.Point(80, 156);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(121, 34);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "Сохранить";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // Form2
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(30)))), ((int)(((byte)(49)))));
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.comboBoxGTM);
            this.Controls.Add(this.btnMusic);
            this.Name = "Form2";
            this.ResumeLayout(false);

         }

         private void btnMusic_Click(object sender, EventArgs e)
         {
            
             if (openFileDialog1.ShowDialog() == DialogResult.OK)
                 WMP.URL = openFileDialog1.FileName;
           
             WMP.settings.volume = 100;
             WMP.controls.stop();
            
         }

         public void comboBoxGTM_SelectedIndexChanged(object sender, EventArgs e)
         {
             

         }

         private void btnSave_Click(object sender, EventArgs e)
         {
             Data.EventHandler(this.WMP, gtms.ElementAt(comboBoxGTM.SelectedIndex).Gt, num);
            
             Close(); 
         }
    
    }
}
