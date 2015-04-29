using TDEditor.Utils;
using TDEditor.project;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using TDEditor.Prop;

namespace TDEditor.Widgets
{
    class TDSprite : RenderBase
    {
        private Image _curImage = DynamicObj.DefaultSpriteImage;
        private String _imagePath = Constant.PathSpriteImg;
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
                    _curImage = ImageHelper.FromFileInc(value);
                    _imagePath = UIProject.Instance().GetRelativePath(value);
                }
                catch (System.Exception )
                {
                    _curImage = DynamicObj.DefaultSpriteImage;
                    _imagePath = Constant.PathSpriteImg;
                }
                this.size = _curImage.Size;
                raisePropChange();
            }
        }

        public TDSprite()
        {
            this.size = _curImage.Size;
            this.Name = Constant.TypeSprite;
        }

        ~TDSprite()
        {
            if (_curImage != null)
            {
                ImageHelper.releaseImage(_curImage);
            }
        }

        protected override void paintSelft(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(_curImage, new Rectangle(0, 0, _curImage.Width, _curImage.Height));
            base.paintSelft(sender, e);
        }


        public override void setAttrToXml(ref XElement xml)
        {
            base.setAttrToXml(ref xml);
            if (!UtilHelper.isEmpty(this._imagePath) && this._imagePath != Constant.PathSpriteImg)
                xml.SetAttributeValue("Image", this._imagePath);
        }


        public override void getAttrByXml(XElement xml)
        {
            base.getAttrByXml(xml);
            if (xml.Attribute("Image") != null)
                this.ImagePath = XmlHelper.GetString(xml, "Image");
        }
    }
}
