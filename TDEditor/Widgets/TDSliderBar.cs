using TDEditor.project;
using TDEditor.Prop;
using TDEditor.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace TDEditor.Widgets
{
    class TDSliderBar : RenderBase
    {

        private Image _imgSliderBack = DynamicObj.DefaultSliderBackImg;
        private String _pathSliderBack = Constant.PathSliderBackImg;
        [ImageAttribute]
        public String PathSliderBack
        {
            get
            {
                return _pathSliderBack;
            }
            set
            {
                if (_imgSliderBack != null)
                {
                    ImageHelper.releaseImage(_imgSliderBack);
                }
                try
                {
                    _imgSliderBack = ImageHelper.FromFileInc(value);;
                    _pathSliderBack = UIProject.Instance().GetRelativePath(value);
                }
                catch (System.Exception)
                {
                    _imgSliderBack = DynamicObj.DefaultSliderBackImg;
                    _pathSliderBack = Constant.PathSliderBackImg;
                }
                raisePropChange();
            }
        }

        private Image _imgSliderBar = DynamicObj.DefaultSliderBarImg;
        private String _pathSliderBar = Constant.PathSliderBarImg;
        [ImageAttribute]
        public String PathSliderBar
        {
            get
            {
                return _pathSliderBar;
            }
            set
            {
                if (_imgSliderBar != null)
                {
                    ImageHelper.releaseImage(_imgSliderBar);
                }
                try
                {
                    _imgSliderBar = ImageHelper.FromFileInc(value);;
                    _pathSliderBar = UIProject.Instance().GetRelativePath(value);
                }
                catch (System.Exception)
                {
                    _imgSliderBar = DynamicObj.DefaultSliderBarImg;
                    _pathSliderBar = Constant.PathSliderBarImg;
                }
                raisePropChange();
            }
        }

        private Image _imgSliderDisable = DynamicObj.DefaultSliderDisableImg;
        private String _pathSliderDisable = Constant.PathSliderDisableImg;
        [ImageAttribute]
        public String PathSliderDisable
        {
            get
            {
                return _pathSliderDisable;
            }
            set
            {
                if (_imgSliderDisable != null)
                {
                    ImageHelper.releaseImage(_imgSliderDisable);
                }
                try
                {
                    _imgSliderDisable = ImageHelper.FromFileInc(value);;
                    _pathSliderDisable = UIProject.Instance().GetRelativePath(value);
                }
                catch (System.Exception)
                {
                    _imgSliderDisable = DynamicObj.DefaultSliderDisableImg;
                    _pathSliderDisable = Constant.PathSliderBarImg;
                }
                raisePropChange();
            }
        }

        private Image _imgSliderNormal = DynamicObj.DefaultSliderNormalImg;
        private String _pathSliderNormal = Constant.PathSliderNormalImg;
        [ImageAttribute]
        public String PathSliderNormal
        {
            get
            {
                return _pathSliderNormal;
            }
            set
            {
                if (_imgSliderNormal != null)
                {
                    ImageHelper.releaseImage(_imgSliderNormal);
                }
                try
                {
                    _imgSliderNormal = ImageHelper.FromFileInc(value);;
                    _pathSliderNormal = UIProject.Instance().GetRelativePath(value);
                }
                catch (System.Exception)
                {
                    _imgSliderNormal = DynamicObj.DefaultSliderNormalImg;
                    _pathSliderNormal = Constant.PathSliderNormalImg;
                }
                raisePropChange();
            }
        }

        private Image _imgSliderSelect = DynamicObj.DefaultSliderSelectImg;
        private String _pathSliderSelect = Constant.PathSliderSelectImg;
        [ImageAttribute]
        public String PathSliderSelect
        {
            get
            {
                return _pathSliderSelect;
            }
            set
            {
                if (_imgSliderSelect != null)
                {
                    ImageHelper.releaseImage(_imgSliderSelect);
                }
                try
                {
                    _imgSliderSelect = ImageHelper.FromFileInc(value);;
                    _pathSliderSelect = UIProject.Instance().GetRelativePath(value);
                }
                catch (System.Exception)
                {
                    _imgSliderSelect = DynamicObj.DefaultSliderSelectImg;
                    _pathSliderSelect = Constant.PathSliderSelectImg;
                }
                raisePropChange();
            }
        }

        private SliderStatus _status = SliderStatus.Normal;
        public SliderStatus status
        {
            get
            {
                return _status;
            }
            set
            {
                _status = value;
                raisePropChange();
            }
        }

        private float _progress = 1;
        public float progress
        {
            get
            {
                return _progress;
            }
            set
            {
                _progress = value;
                raisePropChange();
            }
        }


        public TDSliderBar()
        {
            this.Name = Constant.TypeSliderBar;
            this.size = new SizeF(_imgSliderBack.Width + _imgSliderNormal.Width / 2, _imgSliderNormal.Height);
        }

        ~TDSliderBar()
        {
            if (_imgSliderBack != null)
            {
                ImageHelper.releaseImage(_imgSliderBack);
            }
            if (_imgSliderBar != null)
            {
                ImageHelper.releaseImage(_imgSliderBar);
            }
            if (_imgSliderDisable != null)
            {
                ImageHelper.releaseImage(_imgSliderDisable);
            }
            if (_imgSliderNormal != null)
            {
                ImageHelper.releaseImage(_imgSliderNormal);
            }
            if (_imgSliderSelect != null)
            {
                ImageHelper.releaseImage(_imgSliderSelect);
            }
        }
        protected override void paintSelft(object sender, PaintEventArgs e)
        {
            int drawWidth = (int)(_imgSliderBar.Width * _progress);
            int drawHeight = (int)(this.size.Height - _imgSliderBar.Height) / 2;
            e.Graphics.DrawImage(_imgSliderBack, 0, drawHeight);
            drawHeight = (int)(this.size.Height - _imgSliderBar.Height) / 2;
            e.Graphics.DrawImage(_imgSliderBar, 0, drawHeight, new Rectangle(0, 0, drawWidth, _imgSliderBar.Height), GraphicsUnit.Pixel);
            Image drawImage = _imgSliderNormal;
            if (_status == SliderStatus.Select)
            {
                drawImage = _imgSliderSelect;
            }
            else if (_status == SliderStatus.Disable)
            {
                drawImage = _imgSliderDisable;
            }
            drawHeight = (int)(this.size.Height - drawImage.Height) / 2;
            e.Graphics.DrawImage(drawImage, drawWidth - drawImage.Width / 2, drawHeight);

            base.paintSelft(sender, e);
        }

        public override void setAttrToXml(ref XElement xml)
        {
            base.setAttrToXml(ref xml);
            if (!UtilHelper.isEmpty(this.PathSliderBack) && this.PathSliderBack != Constant.PathSliderBackImg)
                xml.SetAttributeValue("PathSliderBack", this.PathSliderBack);
            if (!UtilHelper.isEmpty(this.PathSliderBar) && this.PathSliderBar != Constant.PathSliderBarImg)
             xml.SetAttributeValue("PathSliderBar", this.PathSliderBar);
            if (!UtilHelper.isEmpty(this.PathSliderDisable) && this.PathSliderDisable != Constant.PathSliderDisableImg)
                xml.SetAttributeValue("PathSliderDisable", this.PathSliderDisable);
            if (!UtilHelper.isEmpty(this.PathSliderNormal) && this.PathSliderNormal != Constant.PathSliderNormalImg)
                xml.SetAttributeValue("PathSliderNormal", this.PathSliderNormal);
            if (!UtilHelper.isEmpty(this.PathSliderSelect) && this.PathSliderSelect != Constant.PathSliderSelectImg)
                xml.SetAttributeValue("PathSliderSelect", this.PathSliderSelect);

            if (this.status != SliderStatus.Normal)
                xml.SetAttributeValue("Status", this.status.ToString());
            if (!UtilHelper.isEqual(this.progress, 1))
                xml.SetAttributeValue("Progress", this.progress.ToString(Constant.DefaultSingleFormat));

        }


        public override void getAttrByXml(XElement xml)
        {
            base.getAttrByXml(xml);
            if (xml.Attribute("PathSliderBack") != null)
                this.PathSliderBack = XmlHelper.GetString(xml, "PathSliderBack");
            if (xml.Attribute("PathSliderBar") != null)
                this.PathSliderBar = XmlHelper.GetString(xml, "PathSliderBar");
            if (xml.Attribute("PathSliderDisable") != null)
                this.PathSliderDisable = XmlHelper.GetString(xml, "PathSliderDisable");
            if (xml.Attribute("PathSliderNormal") != null)
                this.PathSliderNormal = XmlHelper.GetString(xml, "PathSliderNormal");
            if (xml.Attribute("PathSliderSelect") != null)
                this.PathSliderSelect = XmlHelper.GetString(xml, "PathSliderSelect");
            SliderStatus curStatus;
            if (Enum.TryParse<SliderStatus>(XmlHelper.GetString(xml, "Status"), out curStatus))
            {
                this.status = curStatus;
            }
            this.progress = XmlHelper.GetFloat(xml, "Progress", 1.0f);
        }
    }
}
