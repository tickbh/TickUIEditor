using TDEditor.Utils;
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
    class TDInput : RenderBase
    {

        private String _text;
        public String text
        {
            get
            {
                return _text;
            }
            set
            {
                _text = value;
                raisePropChange();
            }
        }

        public String _emptyText;
        public String emptyText
        {
            get
            {
                return _emptyText;
            }
            set
            {
                _emptyText = value;
                raisePropChange();
            }
        }

        private const float defaultFontSize = 14;
        private const String defaultFontName = "Arial";
        private Font _font;
        public Font font
        {
            get
            {
                return _font;
            }
            set
            {
                _font = value;
                raisePropChange();
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

        private bool _isPassword = false;
        public bool isPassword
        {
            get
            {
                return _isPassword;
            }
            set
            {
                _isPassword = value;
                raisePropChange();
            }
        }

        private const int defaultLimitNum = 10;
        private int _limitNum = defaultLimitNum;
        public int limitNum
        {
            get
            {
                return _limitNum;
            }
            set
            {
                _limitNum = value;
                raisePropChange();
            }
        }

        private Image _bgImage = DynamicObj.DefaultInputBgImg;
        private String _inputBg = Constant.PathInputBgImg;
        [ImageAttribute]
        public String inputBg
        {
            set
            {
                _bgImage = ImageHelper.FromFileInc(value);
                if (_bgImage == null)
                {
                    _bgImage = DynamicObj.DefaultInputBgImg;
                    _inputBg = Constant.PathInputBgImg;
                }
                else
                {
                    _inputBg = value;
                }
                raisePropChange();
            }
            get
            {
                return _inputBg;
            }
        }

        public TDInput()
        {
            this.Name = Constant.TypeInput;
            this._emptyText = "Input Label";
            this._font = new Font(defaultFontName, defaultFontSize, FontStyle.Regular);
            this._brush = new SolidBrush(_color);
            this.size = new SizeF(121, 31);
        }
        protected override void paintSelft(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(_bgImage, new Rectangle(0, 0, _bgImage.Width, _bgImage.Height));
            if (_text != null && _text.Length > 0)
            {
                String drawText = _text;
                if (isPassword)
                {
                    drawText = new String('*', _text.Length);
                }
                if (drawText.Length > _limitNum)
                {
                    e.Graphics.DrawString(drawText.Substring(0, _limitNum), this._font, this._brush, 0, 0);
                }
                else
                {
                    e.Graphics.DrawString(drawText, this._font, this._brush, 0, 0);
                }
            }
            else
            {
                e.Graphics.DrawString(_emptyText, this._font, Brushes.LightBlue, 0, 0);
            }
            base.paintSelft(sender, e);
        }

        public override void setAttrToXml(ref XElement xml)
        {
            base.setAttrToXml(ref xml);
            if (this._inputBg != Constant.PathInputBgImg)
                xml.SetAttributeValue("InputBg", this._inputBg);
            if (!UtilHelper.isEmpty(this._text))
                xml.SetAttributeValue("Text", this._text);
            if (!UtilHelper.isEmpty(this._emptyText))
                xml.SetAttributeValue("EmptyText", this._emptyText);
            if (!UtilHelper.isEqual(this._font.Size, defaultFontSize))
                xml.SetAttributeValue("FontSize", this._font.Size.ToString(Constant.DefaultSingleIntFormat));
            if(!UtilHelper.isEqual(this._color, Color.Black))
                xml.SetAttributeValue("Color", UIHelper.ColorToString(this._color));
            if (!this._isPassword) 
                xml.SetAttributeValue("IsPassword", this._isPassword.ToString());
            if (this._limitNum != defaultLimitNum)
                xml.SetAttributeValue("LimitNum", this._limitNum.ToString());
        }


        public override void getAttrByXml(XElement xml)
        {
            base.getAttrByXml(xml);
            if (xml.Attribute("InputBg") != null)
                this.text = XmlHelper.GetString(xml, "InputBg");
            if (xml.Attribute("Text") != null)
                this.text = XmlHelper.GetString(xml, "Text");
            if (xml.Attribute("EmptyText") != null)
                this.emptyText = XmlHelper.GetString(xml, "EmptyText");
            if (xml.Attribute("FontSize") != null)
                this.font = new Font(defaultFontName, XmlHelper.GetFloat(xml ,"FontSize"), FontStyle.Regular);
            if (xml.Attribute("Color") != null)
                this.color = UIHelper.StringToColor(XmlHelper.GetString(xml, "Color"));
            this.isPassword = XmlHelper.GetBool(xml, "IsPassword", false);
            this.limitNum = XmlHelper.GetInt(xml, "LimitNum", defaultLimitNum);
        }
    }
}
