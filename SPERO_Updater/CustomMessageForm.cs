using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SPERO_Updater
{
    public partial class CustomMessageForm : Form
    {
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        bool bAlert;
        string title;
        string description;
        Color separatorColor;
        FontFamily font;
        
        public CustomMessageForm()
        {
            InitializeComponent();
        }

        public CustomMessageForm(bool bAlert, string title, string description, Color separatorColor, FontFamily font)
        {
            InitializeComponent();

            this.bAlert = bAlert;
            this.title = title;
            this.description = description;
            this.separatorColor = separatorColor;
            this.font = font;

            
        }
        
        private void btn_Yes_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Yes;
            this.Close();

        }

        private void btn_Ok_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btn_No_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
            this.Close();
        }

        private void CustomMessageForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void label_Title_MouseDown(object sender, MouseEventArgs e)
        {
            CustomMessageForm_MouseDown(sender, e);
        }

        private void label_Description_MouseDown(object sender, MouseEventArgs e)
        {
            CustomMessageForm_MouseDown(sender, e);
        }

        private void pictureBox_Separator_MouseDown(object sender, MouseEventArgs e)
        {
            CustomMessageForm_MouseDown(sender, e);
        }

        private void CustomMessageForm_Load(object sender, EventArgs e)
        {

            this.label_Title.Font = new Font(font, this.label_Title.Font.Size);
            this.label_Title.Text = title;

            this.label_Description.Font = new Font(font, this.label_Description.Font.Size);
            this.label_Description.Text = description;

            this.pictureBox_Separator.BackColor = separatorColor;

            this.btn_Ok.Font = new Font(font, this.btn_Ok.Font.Size);
            this.btn_Yes.Font = new Font(font, this.btn_Yes.Font.Size);
            this.btn_No.Font = new Font(font, this.btn_No.Font.Size);

            System.ComponentModel.ComponentResourceManager res = new ComponentResourceManager(typeof(MainForm));
            this.btn_Ok.Text = res.GetString("Str_Btn_Ok");
            this.btn_Yes.Text = res.GetString("Str_Btn_Yes");
            this.btn_No.Text = res.GetString("Str_Btn_No");


            while ((label_Description.Width * label_Description.Height) < (System.Windows.Forms.TextRenderer.MeasureText(label_Description.Text,
                new Font(label_Description.Font.FontFamily, label_Description.Font.Size, label_Description.Font.Style)).Width * System.Windows.Forms.TextRenderer.MeasureText(label_Description.Text,
                new Font(label_Description.Font.FontFamily, label_Description.Font.Size, label_Description.Font.Style)).Height * 2))
            {
                label_Description.Font = new Font(label_Description.Font.FontFamily, label_Description.Font.Size - 1.0f, label_Description.Font.Style);
            }

            if (bAlert)
            {
                btn_Ok.Visible = true;
                btn_Yes.Visible = false;
                btn_No.Visible = false;
            }
            else
            {
                btn_Ok.Visible = false;
                btn_Yes.Visible = true;
                btn_No.Visible = true;
            }

            Invalidate();
        }
    }

    public static class CustomMessageBox
    {
        public static void ShowAlert(MainForm parent, string title, string description, Color separatorColor, FontFamily font)
        {
            using (var form = new CustomMessageForm(true, title, description, separatorColor, font))
            {
                form.StartPosition = FormStartPosition.CenterParent;
                parent.Invoke(new Action(() => {
                    form.ShowDialog(parent);
                }));
                
            }
        }

        public static DialogResult ShowConfirmBox(MainForm parent, string title, string description, Color separatorColor, FontFamily font)
        {
            using (var form = new CustomMessageForm(false, title, description, separatorColor, font))
            {
                form.StartPosition = FormStartPosition.CenterParent;
                return form.ShowDialog(parent);
            }
        }
    }
}
