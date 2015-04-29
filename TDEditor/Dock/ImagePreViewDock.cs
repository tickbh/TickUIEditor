using TDEditor.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TDEditor.project;

namespace TDEditor.Dock
{
    public partial class ImagePreViewDock : ToolWindow
    {
        private String _drawIcon;
        public String drawIcon
        {
            set
            {
                if (drawImage != null)
                {
                    ImageHelper.releaseImage(drawImage);
                }
                _drawIcon = value;
                try
                {
                    drawImage = ImageHelper.FromFileInc(_drawIcon);
                }
                catch
                {
                    drawImage = null;
                }
                Refresh();
            }
            get
            {
                return _drawIcon;
            }
        }
        private Image drawImage;

        public ImagePreViewDock()
        {
            InitializeComponent();
            this.Paint += new PaintEventHandler(this.Item_Paint);
            this.DoubleBuffered = true;

            EventManager.RegisterAudience(Constant.PreViewImageChange, new EventHandler<object>(this.itemChange));
        }

        private Rectangle calcDrawRect(Size size, Image img)
        {
            Rectangle rect = Rectangle.Empty;
            float xRatio = img.Size.Width * 1.0f / size.Width;
            float yRatio = img.Size.Height * 1.0f / size.Height;
            float ratio = Math.Max(Math.Max(xRatio, yRatio), 1);
            Size resize = new Size((int)(img.Size.Width / ratio), (int)(img.Size.Height / ratio));
            return new Rectangle((size.Width - resize.Width) / 2, (size.Height - resize.Height) / 2,
                                resize.Width,
                                resize.Height);
        }

        private void Item_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.Gray);
            if (drawImage == null)
            {

            }
            else
            {
                Size curSize = this.Size;
                curSize.Height -= 40;
                e.Graphics.DrawImage(drawImage, calcDrawRect(curSize, drawImage));
                Font font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
                String content = String.Format("大小:{1}*{2}\r\n路径:{0}", UIProject.Instance().GetRelativePath(this.drawIcon), drawImage.Width, drawImage.Height);
                e.Graphics.DrawString(content, font, Brushes.Black, new Rectangle(0, curSize.Height, curSize.Width, 40));
            }
        }

        protected override void OnResize(EventArgs e)
        {
            Invalidate();
        }

        protected void itemChange(object sender, object e)
        {
            this.drawIcon = e as String;
        }

        //protected override void OnPaint(PaintEventArgs e)
        //{
        //    e.Graphics.Clear(Color.Gray);
        //    e.Graphics.DrawImage(drawImage, 0, 0);

        //}
    }
}
