using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SPERO_Updater
{
    public partial class InstructionForm : Form
    {
        ImageObject imgMain;
        
        public InstructionForm()
        {
            InitializeComponent();

            imgMain = new ImageObject(SPERO_Updater.Properties.Resources.Instructions_en, 1, new Rectangle(0, 0, 0, 0));
        }

        private void btn_Exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
                  
            
        }

        private void btn_Ok_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void InstructionForm_Shown(object sender, EventArgs e)
        {
            if (Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName == "zh")
            {
                imgMain = new ImageObject(SPERO_Updater.Properties.Resources.Instructions_zh, 1, new Rectangle(0, 0, 0, 0));
            }
            else if (Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName == "it")
            {
                imgMain = new ImageObject(SPERO_Updater.Properties.Resources.Instructions_it, 1, new Rectangle(0, 0, 0, 0));
            }
            else if (Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName == "fr")
            {
                imgMain = new ImageObject(SPERO_Updater.Properties.Resources.Instructions_fr, 1, new Rectangle(0, 0, 0, 0));
            }
            else
            {
                imgMain = new ImageObject(SPERO_Updater.Properties.Resources.Instructions_en, 1, new Rectangle(0, 0, 0, 0));
            }

            panel_Main.Left = 0;
            pictureBox_Main.Left = panel_Main.Left;
            pictureBox_Main.Width = pictureBox_Main.Width - 1;

            int imageHeight = imgMain.Height * pictureBox_Main.Width / imgMain.Width;
            pictureBox_Main.Height = imageHeight;
            pictureBox_Main.Image = imgMain.img;
        }
    }
}
