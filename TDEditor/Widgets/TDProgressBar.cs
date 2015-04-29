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
    class TDProgressBar : RenderBase
    {
        private Image _imgBar = DynamicObj.DefaultProgressImg;
        private String _imgBarPath = Constant.PathProgressImg;
        [ImageAttribute]
        public String ImageBarPath
        {
            get
            {
                return _imgBarPath;
            }
            set
            {
                if (_imgBar != null)
                {
                    ImageHelper.releaseImage(_imgBar);
                }
                try
                {
                    _imgBar = ImageHelper.FromFileInc(value);;
                    _imgBarPath = UIProject.Instance().GetRelativePath(value);
                }
                catch (System.Exception)
                {
                    _imgBar = DynamicObj.DefaultProgressImg;
                    _imgBarPath = Constant.PathProgressImg;
                }
                raisePropChange();
            }
        }

        public float _progress = 1;
        public float progress
        {
            set
            {
                _progress = Math.Min(Math.Max(value, 0), 1);
                raisePropChange();
            }
            get
            {
                return _progress;
            }
        }
        public TDProgressBar()
        {
            this.Name = Constant.TypeProgressBar;
            this.size = _imgBar.Size;
        }

        ~TDProgressBar()
        {
            if (_imgBar != null)
            {
                ImageHelper.releaseImage(_imgBar);
            }
        }

        protected override void paintSelft(object sender, PaintEventArgs e)
        {
            int drawWidth = (int)(_imgBar.Width * _progress);
            e.Graphics.DrawImage(_imgBar, 0, 0, new Rectangle(0, 0, drawWidth, _imgBar.Height), GraphicsUnit.Pixel);
            base.paintSelft(sender, e);
        }

        public override void setAttrToXml(ref XElement xml)
        {
            base.setAttrToXml(ref xml);
            if (!UtilHelper.isEmpty(this.ImageBarPath) && this.ImageBarPath != Constant.PathProgressImg)
                xml.SetAttributeValue("ImageBar", this.ImageBarPath);
            if (!UtilHelper.isEqual(this.progress, 1))
                xml.SetAttributeValue("Progress", this.progress.ToString(Constant.DefaultSingleFormat));
        }


        public override void getAttrByXml(XElement xml)
        {
            base.getAttrByXml(xml);
            if (xml.Attribute("ImageBar") != null)
                this.ImageBarPath = XmlHelper.GetString(xml, "ImageBar");
            this.progress = XmlHelper.GetFloat(xml, "Progress", 1.0f);
        }
    }
}
