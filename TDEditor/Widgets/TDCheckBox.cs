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


    class TDCheckBox : RenderBase
    {

        private Image _imgNormalBg = DynamicObj.DefaultCBNormalImg;
        private String _imgNormalBgPath = Constant.PathCBNormalImg;

        [ImageAttribute]
        public String ImgNormalBgPath
        {
            get
            {
                return _imgNormalBgPath;
            }
            set
            {
                if (_imgNormalBg != null)
                {
                    ImageHelper.releaseImage(_imgNormalBg);
                }
                try
                {
                    _imgNormalBg = ImageHelper.FromFileInc(value);
                    _imgNormalBgPath = UIProject.Instance().GetRelativePath(value);
                }
                catch (System.Exception)
                {
                    _imgNormalBg = DynamicObj.DefaultCBNormalImg;
                    _imgNormalBgPath = Constant.PathCBNormalImg;
                }
                raisePropChange();
            }
        }

        private Image _imgSelectBg = DynamicObj.DefaultCBSelectImg;
        private String _imgSelectBgPath = Constant.PathCBSelectImg;
        [ImageAttribute]
        public String ImgSelectBgPath
        {
            get
            {
                return _imgSelectBgPath;
            }
            set
            {
                if (_imgSelectBg != null)
                {
                    ImageHelper.releaseImage(_imgDisableBg);
                }
                try
                {
                    _imgSelectBg = ImageHelper.FromFileInc(value);
                    _imgSelectBgPath = UIProject.Instance().GetRelativePath(value);
                }
                catch (System.Exception)
                {
                    _imgSelectBg = DynamicObj.DefaultCBSelectImg;
                    _imgSelectBgPath = Constant.PathCBSelectImg;
                }
                raisePropChange();
            }
        }

        private Image _imgDisableBg = DynamicObj.DefaultCBDisableImg;
        private String _imgDisableBgPath = Constant.PathCBDisableImg;
        [ImageAttribute]
        public String ImgDisableBgPath
        {
            get
            {
                return _imgDisableBgPath;
            }
            set
            {
                if (_imgDisableBg != null)
                {
                    ImageHelper.releaseImage(_imgDisableBg);
                }
                try
                {
                    _imgDisableBg = ImageHelper.FromFileInc(value);
                    _imgDisableBgPath = value;
                }
                catch (System.Exception)
                {
                    _imgDisableBg = DynamicObj.DefaultCBDisableImg;
                    _imgDisableBgPath = Constant.PathCBDisableImg;
                }
                raisePropChange();
            }
        }

        private Image _imgNodeDisable = DynamicObj.DefaultCBNodeDisableImg;
        private String _imgNodeDisablePath = Constant.PathCBNodeDisableImg;
        [ImageAttribute]
        public String ImgNodeDisablePath
        {
            get
            {
                return _imgNodeDisablePath;
            }
            set
            {
                if (_imgNodeDisable != null)
                {
                    ImageHelper.releaseImage(_imgNodeDisable);
                }
                try
                {
                    _imgNodeDisable = ImageHelper.FromFileInc(value);
                    _imgNodeDisablePath = value;
                }
                catch (System.Exception)
                {
                    _imgNodeDisable = DynamicObj.DefaultCBNodeDisableImg;
                    _imgNodeDisablePath = Constant.PathCBNodeDisableImg;
                }
                raisePropChange();
            }
        }

        private Image _imgNodeNormal = DynamicObj.DefaultCBNodeNormalImg;
        private String _imgNodeNormalPath = Constant.PathCBNodeNormalImg;
        [ImageAttribute]
        public String ImageNodeNormalPath
        {
            get
            {
                return _imgNodeNormalPath;
            }
            set
            {
                if (_imgNodeNormal != null)
                {
                    ImageHelper.releaseImage(_imgNodeNormal);
                }
                try
                {
                    _imgNodeNormal = ImageHelper.FromFileInc(value); ;
                    _imgNodeNormalPath = value;
                }
                catch (System.Exception)
                {
                    _imgNodeNormal = DynamicObj.DefaultCBNodeNormalImg;
                    _imgNodeNormalPath = Constant.PathCBNodeNormalImg;
                }
                raisePropChange();
            }
        }

        private CheckStatus _status = CheckStatus.Enable;
        public CheckStatus status
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

        private bool _isCheck = true;
        public bool isCheck
        {
            get
            {
                return _isCheck;
            }
            set
            {
                _isCheck = value;
                raisePropChange();
            }
        }

        public TDCheckBox()
        {
            this.Name = Constant.TypeCheckBox;
            this.size = new SizeF(40, 40);
        }

        ~TDCheckBox()
        {
            if (_imgNormalBg != null)
            {
                ImageHelper.releaseImage(_imgNormalBg);
            }
            if (_imgSelectBg != null)
            {
                ImageHelper.releaseImage(_imgSelectBg);
            }
            if (_imgDisableBg != null)
            {
                ImageHelper.releaseImage(_imgDisableBg);
            }
            if (_imgNodeNormal != null)
            {
                ImageHelper.releaseImage(_imgNodeNormal);
            }
            if (_imgNodeDisable != null)
            {
                ImageHelper.releaseImage(_imgNodeDisable);
            }
        }

        protected override void paintSelft(object sender, PaintEventArgs e)
        {
            if (status == CheckStatus.Enable)
            {
                DrawCenter(e.Graphics, _imgNormalBg);
                if (_isCheck)
                {
                    DrawCenter(e.Graphics, _imgNodeNormal);
                }
            }
            else
            {
                DrawCenter(e.Graphics, _imgDisableBg);
                if (_isCheck)
                {
                    DrawCenter(e.Graphics, _imgNodeDisable);
                }
            }
            base.paintSelft(sender, e);
        }

        public override void setAttrToXml(ref XElement xml)
        {
            base.setAttrToXml(ref xml);
            if (!UtilHelper.isEmpty(this.ImgNormalBgPath) && this.ImgNormalBgPath != Constant.PathCBNormalImg)
                xml.SetAttributeValue("ImgNormalBg", this.ImgNormalBgPath);
            if (!UtilHelper.isEmpty(this.ImgSelectBgPath) && this.ImgSelectBgPath != Constant.PathCBSelectImg)
                xml.SetAttributeValue("ImgSelectBg", this.ImgSelectBgPath);
            if (!UtilHelper.isEmpty(this.ImgDisableBgPath) && this.ImgDisableBgPath != Constant.PathCBDisableImg)
                xml.SetAttributeValue("ImgDisableBg", this.ImgDisableBgPath);
            if (!UtilHelper.isEmpty(this.ImgNodeDisablePath) && this.ImgNodeDisablePath != Constant.PathCBNodeDisableImg)
                xml.SetAttributeValue("ImgNodeDisable", this.ImgNodeDisablePath);
            if (!UtilHelper.isEmpty(this.ImageNodeNormalPath) && this.ImageNodeNormalPath != Constant.PathCBNodeNormalImg)
                xml.SetAttributeValue("ImgNodeNormal", this.ImageNodeNormalPath);
            if (this.status != CheckStatus.Enable)
                xml.SetAttributeValue("Status", this.status.ToString());
            if (this.isCheck != true)
                xml.SetAttributeValue("IsCheck", this.isCheck);
        }


        public override void getAttrByXml(XElement xml)
        {
            base.getAttrByXml(xml);
            if (xml.Attribute("ImgNormalBg") != null)
                this.ImgNormalBgPath = XmlHelper.GetString(xml, "ImgNormalBg");
            if (xml.Attribute("ImgSelectBg") != null)
             this.ImgSelectBgPath = XmlHelper.GetString(xml, "ImgSelectBg");
            if (xml.Attribute("ImgDisableBg") != null)
                this.ImgDisableBgPath = XmlHelper.GetString(xml, "ImgDisableBg");
            if (xml.Attribute("ImgNodeDisable") != null)
                this.ImgNodeDisablePath = XmlHelper.GetString(xml, "ImgNodeDisable");
            if (xml.Attribute("ImgNodeNormal") != null)
                this.ImageNodeNormalPath = XmlHelper.GetString(xml, "ImgNodeNormal");
            CheckStatus curStatus;
            if (Enum.TryParse<CheckStatus>(XmlHelper.GetString(xml, "Status"), out curStatus))
            {
                this.status = curStatus;
            }
            this.isCheck = XmlHelper.GetBool(xml, "IsCheck", true);
        }
    }
}
