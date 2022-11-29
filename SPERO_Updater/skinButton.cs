// Last version at http://www.dotnetskin.net
// Created by PanWen 2005
//================================= 
//DotNetSkin
//=================================
// You may include the source code, modified source code, assembly
// within your own projects for either personal or commercial use 
// with the only one restriction:
// don't change the name library "CSpk_BootGUI".

using System;
using System.Drawing;
using System.Text;
using System.Reflection;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;


namespace SPERO_Updater
{
	public enum State
	{
		Normal = 1,
		MouseOver  = 2,
		MouseDown = 3,
		Disable = 4,
		Default = 5
	}

	/// <summary>
	/// skinButton
	/// </summary>
	public class SkinButton:Button
	{
        public SkinImage skinImage;
		private State state=State.Normal;

		public SkinButton()
		{
			try
			{
				this.SetStyle(ControlStyles.DoubleBuffer, true);
				this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
				this.SetStyle(ControlStyles.UserPaint, true);
				this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
				this.SetStyle(ControlStyles.StandardDoubleClick, false);
				this.SetStyle(ControlStyles.Selectable, true);
				this.ResizeRedraw = true;
                this.skinImage = new SPERO_Updater.SkinImage();
            }
			catch{}
		}

		protected override void OnMouseEnter(EventArgs e)
		{
			state = State.MouseOver;
			this.Invalidate();
			base.OnMouseEnter(e);
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			state = State.Normal;
			this.Invalidate();
			base.OnMouseLeave(e);
		}

		protected override void OnMouseDown(System.Windows.Forms.MouseEventArgs e)
		{
			if ((e.Button & MouseButtons.Left) != MouseButtons.Left) return;

			state = State.MouseDown;
			this.Invalidate();
			base.OnMouseDown(e);
		}

		protected override void OnMouseUp(System.Windows.Forms.MouseEventArgs e)
		{
			if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
				state = State.Normal;
			this.Invalidate();
			base.OnMouseUp(e);
		}

		protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
		{
            if (skinImage.button.img == null) 
			{
				base.OnPaint(e);
				return;
			}

			int i = (int)state;
			//if (this.Focused && state != State.MouseDown) 	i = 5;
			if (!this.Enabled) i = 4;
			Rectangle rc = this.ClientRectangle;
			Graphics g = e.Graphics;

			base.InvokePaintBackground(this, new PaintEventArgs(e.Graphics, base.ClientRectangle));

            SkinDraw.DrawRect2(g, skinImage.button, rc, i);

			Image img = null;
			Size txts,imgs;

			txts = Size.Empty;
			imgs = Size.Empty;

			if ( this.Image != null ) 
			{
				img = this.Image;
			}
			else if ( this.ImageList != null && this.ImageIndex != -1)
			{
				img = this.ImageList.Images[this.ImageIndex];
			}

			if (img != null)
			{
				imgs.Width = img.Width;
				imgs.Height = img.Height;
			} 

			StringFormat format1;
			using (format1 = new StringFormat())
			{
				format1.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.Show;
				SizeF ef1 = g.MeasureString(this.Text, this.Font, new SizeF((float) rc.Width, (float) rc.Height), format1);
				txts = Size.Ceiling(ef1);
			}

			rc.Inflate(-4,-4);
			if (imgs.Width*imgs.Height != 0)
			{
				Rectangle imgr = rc;
				imgr = SkinDraw.HAlignWithin(imgs,imgr,this.ImageAlign);
				imgr = SkinDraw.VAlignWithin(imgs,imgr,this.ImageAlign);
				if (!this.Enabled)
				{
					ControlPaint.DrawImageDisabled(g,img, imgr.Left, imgr.Top, this.BackColor);
				}
				else
				{
					g.DrawImage(img,imgr.Left, imgr.Top, img.Width, img.Height);
				}
			}

			Rectangle txtr = rc;
			txtr = SkinDraw.HAlignWithin(txts,txtr,this.TextAlign);
			txtr = SkinDraw.VAlignWithin(txts,txtr,this.TextAlign);
            txtr.Y += txtr.Height / 8;

			Brush brush2;
			format1 = new StringFormat();
			format1.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.Show;

			if (this.RightToLeft == RightToLeft.Yes)
			{
				format1.FormatFlags |= StringFormatFlags.DirectionRightToLeft;
			}
			brush2 = new SolidBrush(this.ForeColor);
			g.DrawString(this.Text, this.Font, brush2, (RectangleF) txtr, format1);
			brush2.Dispose();

		}
	}
	
	public class SkinCheckBox:CheckBox
	{
        public SkinImage skinImage;
        private State state = State.Normal;

        private Bitmap offScreenBmp;
        private Graphics offScreenDC;

		public SkinCheckBox()
		{
			this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
			this.BackColor = System.Drawing.Color.Transparent;
            this.CheckAlign = ContentAlignment.MiddleCenter;
            this.skinImage = new SPERO_Updater.SkinImage();
            this.Cursor = Cursors.Hand;
        }

		protected override void OnMouseEnter(EventArgs e)
		{
			state = State.MouseOver;
			this.Invalidate();
			base.OnMouseEnter(e);
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			state = State.Normal;
			this.Invalidate();
			base.OnMouseLeave(e);
		}

		protected override void OnMouseDown(System.Windows.Forms.MouseEventArgs e)
		{
			if ((e.Button & MouseButtons.Left) != MouseButtons.Left) return;

			state = State.MouseDown;
			this.Invalidate();
			base.OnMouseDown(e);
		}

		protected override void OnMouseUp(System.Windows.Forms.MouseEventArgs e)
		{
			if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
				state = State.Normal;
			this.Invalidate();
			base.OnMouseUp(e);
		}
		
		protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
		{
            if (skinImage.checkbox.img == null) 
			{
				base.OnPaint(e);
				return;
			}
			Graphics g = e.Graphics;

            Rectangle rc = this.ClientRectangle;
            offScreenBmp = new Bitmap(rc.Width, rc.Height);
            offScreenDC = Graphics.FromImage(offScreenBmp);

			int i = (int)state;
			if (!this.Enabled) i = 4;
			if (this.CheckState == CheckState.Checked) i+=4;
			if (this.CheckState == CheckState.Indeterminate) i+=8;

// 			Rectangle r1 = rc;;
            Rectangle r1;
            if (this.skinImage.Scheme == Schemes.Plex)
            {
                r1 = new Rectangle(0, 0, 32, 32);
                this.Size = new Size(32, 32);
            }
            else
            {
                r1 = new Rectangle(0, 0, 60, 23);
                this.Size = new Size(60, 23);
            }
			base.OnPaint(e);
            
            SolidBrush br = new SolidBrush(Color.Transparent);
            offScreenDC.FillRectangle(br, r1);
			
			int cw = SystemInformation.MenuCheckSize.Width ;
// 			if (this.CheckAlign == ContentAlignment.MiddleLeft)
// 			{
//                 r1 = Rectangle.FromLTRB(0, (r1.Height - cw) / 2, 60, 23);
// 			} 
// 			else
// 			{
// 				r1=Rectangle.FromLTRB(r1.Right-cw-1,(r1.Height-cw)/2,r1.Right,(r1.Height+cw)/2);
// 			}

            SkinDraw.DrawRect1(offScreenDC, skinImage.checkbox, r1, i);

            g.DrawImage(offScreenBmp, rc.Left, rc.Top);
        }
	}

	public class SkinRadioButton:RadioButton
	{
        public SkinImage skinImage;
        private State state = State.Normal;

		public SkinRadioButton()
		{
			this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
			this.BackColor = System.Drawing.Color.Transparent;
            this.skinImage = new SPERO_Updater.SkinImage();
        }

		protected override void OnMouseEnter(EventArgs e)
		{
			state = State.MouseOver;
			this.Invalidate();
			base.OnMouseEnter(e);
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			state = State.Normal;
			this.Invalidate();
			base.OnMouseLeave(e);
		}

		protected override void OnMouseDown(System.Windows.Forms.MouseEventArgs e)
		{
			if ((e.Button & MouseButtons.Left) != MouseButtons.Left) return;

			state = State.MouseDown;
			this.Invalidate();
			base.OnMouseDown(e);
		}

		protected override void OnMouseUp(System.Windows.Forms.MouseEventArgs e)
		{
			if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
				state = State.Normal;
			this.Invalidate();
			base.OnMouseUp(e);
		}
		
		protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
		{	
			if (skinImage.radiobutton.img==null) 
			{
				base.OnPaint(e);
				return;
			}

			int i = (int)state;
			if (!this.Enabled) i = 4;
			if (this.Checked) i+=4;

			Rectangle rc = this.ClientRectangle;
			Rectangle r1 = rc;;
			Graphics g = e.Graphics;
			base.OnPaint(e);
			
			int cw = SystemInformation.MenuCheckSize.Width ;

			if (this.CheckAlign == ContentAlignment.MiddleLeft)
			{
				r1=Rectangle.FromLTRB(0,(r1.Height-cw)/2,0+cw,(r1.Height+cw)/2);
			} 
			else
			{
				r1=Rectangle.FromLTRB(r1.Right-cw-1,(r1.Height-cw)/2,r1.Right,(r1.Height+cw)/2);
			}
			SkinDraw.DrawRect1(g,skinImage.radiobutton,r1,i);
		}
	}

    public class SkinProgressBar : Panel
    {
        public Color clrValue;
        public Color clrBorder;

        private int maximum;
        private int minimum;
        private int value;

        private Bitmap offScreenBmp;
        private Graphics offScreenDC;

        public enum ColorMode
        {
            Red,
            Green,
            Blue,
        }

        public int Value
        {
            get { return value; }
            set
            {
                this.value = Math.Min(maximum, Math.Max(minimum, value));
                Invalidate();
            }
        }

        public int Maximum
        {
            get { return maximum; }
            set { maximum = value; }
        }

        public int Minimum
        {
            get { return minimum; }
            set { minimum = value; }
        }

        public SkinProgressBar()
        {
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.BackColor = Color.Transparent;
            this.maximum = 100;
            this.minimum = 0;
            this.value = 0;
            SetColor(ColorMode.Blue);
        }

        public void SetColor(ColorMode clrMode)
        {
            if (clrMode == ColorMode.Red)
            {
                clrValue = Color.FromArgb(192, 0, 0);
                clrBorder = Color.FromArgb(255, 0, 0);
            }
            else if (clrMode == ColorMode.Green)
            {
                clrValue = Color.FromArgb(0, 187, 23);
                clrBorder = Color.FromArgb(13, 255, 59);
            }
            else if (clrMode == ColorMode.Blue)
            {
                clrValue = Color.FromArgb(0, 112, 192);
                clrBorder = Color.FromArgb(0, 176, 240);
            }
        }

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            int nVal = this.Value;
            int nMax = this.Maximum;
            Graphics clientDC = e.Graphics;

            Rectangle rc = this.ClientRectangle;
            offScreenBmp = new Bitmap(rc.Width, rc.Height);
            offScreenDC = Graphics.FromImage(offScreenBmp);

            Color clrBgnd = (this.Enabled == true) ? Color.FromArgb(209, 210, 211) : Color.FromArgb(64, 64, 64);
            SolidBrush brBgnd = new SolidBrush(clrBgnd);

            offScreenDC.FillPie(brBgnd, new Rectangle(rc.Left, rc.Top, rc.Height, rc.Height-1), 90, 180);
            offScreenDC.FillPie(brBgnd, new Rectangle(rc.Right - rc.Height, rc.Top, rc.Height, rc.Height-1), 270, 180);
            offScreenDC.FillRectangle(brBgnd, new Rectangle(rc.Left + rc.Height / 2 , rc.Top, rc.Width - rc.Height, rc.Height));
            
            if (this.value > 0)
            {
                SolidBrush brValue = new SolidBrush(clrValue);
                Pen peBorder = new Pen(clrBorder, 1.0f);
                int nValueWid = (rc.Width - rc.Height) * this.value / this.maximum;

                offScreenDC.FillPie(brValue, new Rectangle(rc.Left, rc.Top, rc.Height, rc.Height-1), 90, 180);
                offScreenDC.DrawPie(peBorder, new Rectangle(rc.Left, rc.Top, rc.Height, rc.Height - 1), 90, 180);
                offScreenDC.FillPie(brValue, new Rectangle(rc.Left + nValueWid, rc.Top, rc.Height, rc.Height - 1), 270, 180);
                offScreenDC.DrawPie(peBorder, new Rectangle(rc.Left + nValueWid, rc.Top, rc.Height-1, rc.Height - 1), 270, 180);
                offScreenDC.FillRectangle(brValue, new Rectangle(rc.Left + rc.Height / 2 - 1, rc.Top+1, nValueWid+2, rc.Height-2));
                offScreenDC.DrawLine(peBorder, rc.Left + rc.Height / 2, rc.Top, rc.Left + rc.Height / 2 + nValueWid, rc.Top);
                offScreenDC.DrawLine(peBorder, rc.Left + rc.Height / 2, rc.Bottom-1, rc.Left + rc.Height / 2 + nValueWid, rc.Bottom-1);

                Font drawFont = new Font("Arial", rc.Height/3);
                Rectangle txtRect = new Rectangle(rc.Left, rc.Top, nValueWid + rc.Height, rc.Height);

                StringFormat stringFormat = new StringFormat();
                stringFormat.Alignment = StringAlignment.Center;
                stringFormat.LineAlignment = StringAlignment.Center;

                offScreenDC.DrawString(this.value.ToString() + "%", drawFont, Brushes.White,  txtRect, stringFormat);
            }
            

            clientDC.DrawImage(offScreenBmp, rc.Left, rc.Top);
        }
    }
}
