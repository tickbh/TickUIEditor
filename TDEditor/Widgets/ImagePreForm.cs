using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TDEditor.Widgets
{
    class ImagePreForm : UserControl
    {
        private String _drawIcon;
        public String drawIcon
        {
            set
            {
                _drawIcon = value;
                try
                {
                    drawImage = Image.FromFile(_drawIcon);
                    this.Size = drawImage.Size;
                }
                catch
                {
                    drawImage = null;
                }
            }
            get
            {
                return _drawIcon;
            }
        }
        private Image drawImage;

        public ImagePreForm(String icon)
            :base()
        {
            this.Paint += new PaintEventHandler(this.Item_Paint);
            this.drawIcon = icon;
            this.Enabled = false;
            this.DoubleBuffered = true;
            //this.ShowInTaskbar = false;
            //this.FormBorderStyle = FormBorderStyle.None;
        }

        private void Item_Paint(object sender, PaintEventArgs e)
        {
            if (drawImage != null)
            {
                e.Graphics.DrawImage(drawImage, new RectangleF(0, 0, drawImage.Width, drawImage.Height));
            }
        }

    }
}
