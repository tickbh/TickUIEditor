using TDEditor.Utils;
using TDEditor.project;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using TDEditor.Prop;

namespace TDEditor.Widgets
{
    class TDScale9 : RenderBase
    {
        private Image _curImage = DynamicObj.DefaultScale9Image;
        private String _imagePath = Constant.PathScale9Img;
        [ImageAttribute]
        public String ImagePath
        {
            get
            {
                return _imagePath;
            }
            set
            {
                if (_curImage != null)
                {
                    ImageHelper.releaseImage(_curImage);
                }
                try
                {
                    _curImage = ImageHelper.FromFileInc(value);;
                    _imagePath = UIProject.Instance().GetRelativePath(value);
                }
                catch (System.Exception)
                {
                    _curImage = DynamicObj.DefaultScale9Image;
                    _imagePath = Constant.PathScale9Img;
                }
                raisePropChange();
            }
        }

        public static TDScale9 CreateScale9(String path)
        {
            TDScale9 scale9 = new TDScale9();
            scale9.ImagePath = path;
            return scale9;
        }

        public TDScale9()
        {
            this.Name = Constant.TypeScale9;
            this.size = new SizeF(50, 50);
        }

        ~TDScale9()
        {
            if (_curImage != null)
            {
                ImageHelper.releaseImage(_curImage);
            }
        }

        public bool isMissImage()
        {
            return _imagePath == Constant.PathMissImg;
        }

        public void DrawScale9(Graphics painter, SizeF drawSize, float offsetX = 0, float offsetY = 0)
        {
            SizeF size = _curImage.Size;
            RectangleF rect = new RectangleF(size.Width / 3.0f, size.Height / 3.0f, size.Width / 3.0f, size.Height / 3.0f);
            float left_w = rect.X;
            float right_w = rect.Width;
            float center_w = size.Width - (left_w + right_w);

            float top_h = rect.Y;
            float bottom_h = rect.Height;
            float center_h = size.Height - (top_h + bottom_h);

            float draw_center_w = (drawSize.Width - left_w - right_w);
            float draw_center_h = (drawSize.Height - top_h - bottom_h);
            float rightStart = drawSize.Width - right_w;
            float bottomStart = drawSize.Height - bottom_h;

            RectangleF lefttopbounds = new RectangleF(0, 0, left_w, top_h);
            RectangleF centertopbounds = new RectangleF(left_w, 0, center_w, top_h);
            RectangleF righttopbounds = new RectangleF(left_w + center_w, 0, right_w, top_h);
            RectangleF leftcenterbounds = new RectangleF(0, top_h, left_w, center_h);
            RectangleF centerbounds = new RectangleF(left_w, top_h, center_w, center_h);
            RectangleF rightcenterbounds = new RectangleF(left_w + center_w, top_h, right_w, center_h);
            RectangleF leftbottombounds = new RectangleF(0, top_h + center_h, left_w, bottom_h);
            RectangleF centerbottombounds = new RectangleF(left_w, top_h + center_h, center_w, bottom_h);
            RectangleF rightbottombounds = new RectangleF(left_w + center_w, top_h + center_h, right_w, bottom_h);


            GraphicsState transState = painter.Save();
            painter.TranslateTransform(offsetX, offsetY);
            //top
            painter.DrawImage(_curImage, new RectangleF(0, 0, left_w, top_h), lefttopbounds, GraphicsUnit.Pixel);
            if (draw_center_w > 0)
                painter.DrawImage(_curImage, new RectangleF(left_w, 0, draw_center_w, top_h), centertopbounds, GraphicsUnit.Pixel);
            painter.DrawImage(_curImage, new RectangleF(rightStart, 0, right_w, top_h), righttopbounds, GraphicsUnit.Pixel);

            //center
            if (draw_center_h > 0)
            {
                painter.DrawImage(_curImage, new RectangleF(0, top_h, left_w, draw_center_h), leftcenterbounds, GraphicsUnit.Pixel);
                if (draw_center_w > 0)
                    painter.DrawImage(_curImage, new RectangleF(left_w, top_h, draw_center_w, draw_center_h), centerbounds, GraphicsUnit.Pixel);
                painter.DrawImage(_curImage, new RectangleF(rightStart, top_h, right_w, draw_center_h), rightcenterbounds, GraphicsUnit.Pixel);
            }

            //bottom
            painter.DrawImage(_curImage, new RectangleF(0, bottomStart, left_w, bottom_h), leftbottombounds, GraphicsUnit.Pixel);
            if (draw_center_w > 0)
                painter.DrawImage(_curImage, new RectangleF(left_w, bottomStart, draw_center_w, bottom_h), centerbottombounds, GraphicsUnit.Pixel);
            painter.DrawImage(_curImage, new RectangleF(rightStart, bottomStart, right_w, bottom_h), rightbottombounds, GraphicsUnit.Pixel);
            painter.Restore(transState);
        }

        protected override void paintSelft(object sender, PaintEventArgs e)
        {
            DrawScale9(e.Graphics, this.size, 0, 0);
            base.paintSelft(sender, e);
        }

        public override void setAttrToXml(ref XElement xml)
        {
            base.setAttrToXml(ref xml);
            if (!UtilHelper.isEmpty(this._imagePath) && this._imagePath != Constant.PathScale9Img)
                xml.SetAttributeValue("Image", this._imagePath);
        }


        public override void getAttrByXml(XElement xml)
        {
            base.getAttrByXml(xml);
            if(xml.Attribute("Image") != null)
                this.ImagePath = XmlHelper.GetString(xml, "Image");
        }
    }
}
