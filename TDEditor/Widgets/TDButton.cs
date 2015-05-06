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

    class TDButton : RenderBase
    {
        public bool isScale9
        {
            get;
            set;
        }

        private String _normalText;
        public String mormalText
        {
            get
            {
                return _normalText;
            }
            set
            {
                _normalText = value;
                raisePropChange();
            }
        }

        private String _selectText;
        public String selectText
        {
            get
            {
                return _selectText;
            }
            set
            {
                _selectText = value;
                raisePropChange();
            }
        }
        private String _disableText;
        public String disableText
        {
            get
            {
                return _disableText;
            }
            set
            {
                _disableText = value;
                raisePropChange();
            }
        }


        private ButtonStatus _status;
        public ButtonStatus status
        {
            set
            {
                _status = value;
                raisePropChange();
            }
            get
            {
                return _status;
            }
        }

        private TDScale9 _NormalCanvas = DynamicObj.DefaultBtnNormalImg;
        private String _normalBg = Constant.PathBtnNormalImg;
        [ImageAttribute]
        public String normalBg
        {
            set
            {
                _NormalCanvas = TDScale9.CreateScale9(value);
                if (_NormalCanvas.isMissImage())
                {
                    _NormalCanvas = DynamicObj.DefaultBtnNormalImg;
                    _normalBg = Constant.PathBtnNormalImg;
                }
                else
                {
                    _normalBg = value;
                }
                raisePropChange();
            }
            get
            {
                return _normalBg;
            }
        }
        private TDScale9 _SelectCanvas = DynamicObj.DefaultBtnSelectImg;
        private String _selectBg = Constant.PathBtnSelectImg;
        [ImageAttribute]
        public String selectBg
        {
            set
            {
                _SelectCanvas = TDScale9.CreateScale9(value);
                if (_SelectCanvas.isMissImage())
                {
                    _SelectCanvas = DynamicObj.DefaultBtnSelectImg;
                    _selectBg = Constant.PathBtnSelectImg;
                }
                else
                {
                    _selectBg = value;
                }
                raisePropChange();
            }
            get
            {
                return _selectBg;
            }
        }

        private TDScale9 _DisableCanvas = DynamicObj.DefaultBtnDisableImg;
        private String _disableBg = Constant.PathBtnDisableImg;
        [ImageAttribute]
        public String disableBg
        {
            set
            {
                _DisableCanvas = TDScale9.CreateScale9(value);
                if (_DisableCanvas.isMissImage())
                {
                    _DisableCanvas = DynamicObj.DefaultBtnDisableImg;
                    _disableBg = Constant.PathBtnDisableImg;
                }
                else
                {
                    _disableBg = value;
                }
                raisePropChange();
            }
            get
            {
                return _disableBg;
            }
        }

        private Image _imgNormalLabel;
        private String _normalLabel;
        [ImageAttribute]
        public String normalLabel
        {
            set
            {
                if (_imgNormalLabel != null)
                {
                    ImageHelper.releaseImage(_imgNormalLabel);
                }
                try
                {
                    _imgNormalLabel = ImageHelper.FromFileInc(value);
                    _normalLabel = UIProject.Instance().GetRelativePath(value);
                }
                catch (System.Exception)
                {
                    _imgNormalLabel = null;
                    _normalLabel = "";
                }
                raisePropChange();
            }
            get
            {
                return _normalLabel;
            }
        }
        private Image _imgSelectLabel;
        private String _selectLabel;

        [ImageAttribute]
        public String selectLabel
        {
            set
            {
                if (_imgSelectLabel != null)
                {
                    ImageHelper.releaseImage(_imgSelectLabel);
                }
                try
                {
                    _imgSelectLabel = ImageHelper.FromFileInc(value);
                    _selectLabel = UIProject.Instance().GetRelativePath(value);
                }
                catch (System.Exception)
                {
                    _imgSelectLabel = null;
                    _selectLabel = "";
                }
                raisePropChange();
            }
            get
            {
                return _selectLabel;
            }
        }

        private Image _imgDisableLabel;
        private String _disableLabel;
        [ImageAttribute]
        public String disableLabel
        {
            set
            {
                if (_imgDisableLabel != null)
                {
                    ImageHelper.releaseImage(_imgSelectLabel);
                }
                try
                {
                    _imgDisableLabel = ImageHelper.FromFileInc(value);
                    _disableLabel = UIProject.Instance().GetRelativePath(value);
                }
                catch (System.Exception)
                {
                    _imgDisableLabel = null;
                    _disableLabel = "";
                }
                raisePropChange();
            }
            get
            {
                return _disableLabel;
            }
        }

        private static float defaultFontSize = 18;
        private Font _font;
        public Font font
        {
            set
            {
                _font = value;
                raisePropChange();
            }
            get
            {
                return _font;
            }
        }

        private Brush _brush;
        private Color _color = Color.Black;
        public Color color
        {
            get
            {
                return _color;
            }
            set
            {
                _color = value;
                _brush = new SolidBrush(_color);
                raisePropChange();
            }
        }

        public TDButton()
        {
            this.Name = Constant.TypeButton;
            this.status = ButtonStatus.Normal;
            _font = new Font("Arial", defaultFontSize, FontStyle.Regular);
            this.size = new SizeF(80, 35);
        }

        ~TDButton()
        {

            if (_imgNormalLabel != null)
            {
                ImageHelper.releaseImage(_imgNormalLabel);
            }
            if (_imgSelectLabel != null)
            {
                ImageHelper.releaseImage(_imgSelectLabel);
            }
            if (_imgDisableLabel != null)
            {
                ImageHelper.releaseImage(_imgSelectLabel);
            }
        }

        protected override void paintSelft(object sender, PaintEventArgs e)
        {
            float x = 0;
            float y = 0;
            if (status == ButtonStatus.Normal)
            {
                if (_NormalCanvas != null)
                {
                    _NormalCanvas.DrawScale9(e.Graphics,this.size, x, y);
                }
                if (_imgNormalLabel != null)
                {
                    DrawCenter(e.Graphics, _imgNormalLabel);
                }
                else if (this.mormalText != null)
                {
                    DrawCenter(e.Graphics, _font, this.mormalText, _brush);
                }
            }
            else if (status == ButtonStatus.Select)
            {
                if (_SelectCanvas != null)
                {
                    _SelectCanvas.DrawScale9(e.Graphics, this.size, x, y);
                }
                if (_imgSelectLabel != null)
                {
                    DrawCenter(e.Graphics, _imgSelectLabel);
                }
                else if (this.selectText != null)
                {
                    DrawCenter(e.Graphics, _font, this.selectText, _brush);
                }
 
            }
            else if (status == ButtonStatus.Disable)
            {
                if (_DisableCanvas != null)
                {
                    _DisableCanvas.DrawScale9(e.Graphics, this.size, x, y);
                }
                if (_imgDisableLabel != null)
                {
                    DrawCenter(e.Graphics, _imgDisableLabel);
                }
                else if (this.disableText != null)
                {
                    DrawCenter(e.Graphics, _font, this.disableText, _brush);
                }
            }

            base.paintSelft(sender, e);
        }

        public override void setAttrToXml(ref XElement xml)
        {
            base.setAttrToXml(ref xml);
            if (!UtilHelper.isEqual(this._font.Size, defaultFontSize))
            {
                xml.SetAttributeValue("FontName", this._font.Name);
                xml.SetAttributeValue("FontSize", this._font.Size.ToString(Constant.DefaultSingleIntFormat));
            }
            if(!UtilHelper.isEmpty(this._normalText))
                xml.SetAttributeValue("normalText", this.mormalText);
            if (!UtilHelper.isEmpty(this._selectText))
                xml.SetAttributeValue("selectText", this.selectText);
            if (!UtilHelper.isEmpty(this._disableText))
                xml.SetAttributeValue("disableText", this.disableText);
            if (this.status != ButtonStatus.Normal)
                xml.SetAttributeValue("Status", this.status.ToString());
            if (!UtilHelper.isEmpty(this._normalBg) && this._normalBg != Constant.PathBtnNormalImg)
                xml.SetAttributeValue("normalBg", this.normalBg);
            if (!UtilHelper.isEmpty(this._selectBg) && this._normalBg != Constant.PathBtnSelectImg)
                xml.SetAttributeValue("selectBg", this.selectBg);
            if (!UtilHelper.isEmpty(this._disableBg) && this._normalBg != Constant.PathBtnDisableImg)
                xml.SetAttributeValue("disableBg", this.disableBg);
            if (!UtilHelper.isEmpty(this._normalLabel))
                xml.SetAttributeValue("normalLabel", this.normalLabel);
            if (!UtilHelper.isEmpty(this._selectLabel))
                xml.SetAttributeValue("selectLabel", this.selectLabel);
            if (!UtilHelper.isEmpty(this._disableLabel))
                xml.SetAttributeValue("disableLabel", this.disableLabel);
            if(!UtilHelper.isEqual(this._color, Color.Black))
                xml.SetAttributeValue("Color", UIHelper.ColorToString(this._color));
                xml.SetAttributeValue("IsScale9", this.isScale9);
        }


        public override void getAttrByXml(XElement xml)
        {
            base.getAttrByXml(xml);
            this.font = new Font(XmlHelper.GetString(xml, "FontName", "Arial"), XmlHelper.GetFloat(xml, "FontSize", defaultFontSize), FontStyle.Regular);
            ButtonStatus curStatus;
            if (Enum.TryParse<ButtonStatus>(XmlHelper.GetString(xml, "Status"), out curStatus))
            {
                this.status = curStatus;
            }
            if (xml.Attribute("normalText") != null)
                this.mormalText = XmlHelper.GetString(xml, "normalText");
            if (xml.Attribute("selectText") != null)
                this.selectText = XmlHelper.GetString(xml, "selectText");
            if (xml.Attribute("disableText") != null)
                this.disableText = XmlHelper.GetString(xml, "disableText");
            if (xml.Attribute("normalBg") != null)
                this.normalBg = XmlHelper.GetString(xml, "normalBg");
            if (xml.Attribute("selectBg") != null)
                this.selectBg = XmlHelper.GetString(xml, "selectBg");
            if (xml.Attribute("disableBg") != null)
                this.disableBg = XmlHelper.GetString(xml, "disableBg");
            if (xml.Attribute("normalLabel") != null)
                this.normalLabel = XmlHelper.GetString(xml, "normalLabel");
            if (xml.Attribute("selectLabel") != null)
                this.selectLabel = XmlHelper.GetString(xml, "selectLabel");
            if (xml.Attribute("disableLabel") != null)
                this.disableLabel = XmlHelper.GetString(xml, "disableLabel");
            if (xml.Attribute("Color") != null)
                this.color = UIHelper.StringToColor(XmlHelper.GetString(xml, "Color"));
            if (xml.Attribute("IsScale9") != null)
                this.isScale9 = XmlHelper.GetBool(xml, "IsScale9");
        }
    }
}
