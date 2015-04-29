using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TDEditor.Utils;
using System.Xml.Linq;

namespace TDEditor.Widgets
{
    public partial class TDText : RenderBase
    {

        private const String defaultText = "Text Label";
        private String _text = defaultText;
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

        public TDText()
        {
            this.Name = Constant.TypeText;
            this._font = new Font(defaultFontName, defaultFontSize, FontStyle.Regular);
            this._brush = new SolidBrush(_color);
            this.size = new SizeF(121, 31);
        }
        protected  override void paintSelft(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawString(_text, _font, this._brush, 0, 0);
            base.paintSelft(sender, e);
        }

        public override void setAttrToXml(ref XElement xml)
        {
            base.setAttrToXml(ref xml);
            if (!UtilHelper.isEmpty(this._text) && this._text != defaultText)
                xml.SetAttributeValue("Text", this._text);
            if (!UtilHelper.isEqual(this._font.Size, defaultFontSize))
                xml.SetAttributeValue("FontSize", this._font.Size.ToString(Constant.DefaultSingleIntFormat));
            if(!UtilHelper.isEqual(this._color, Color.Black))
                xml.SetAttributeValue("Color", UIHelper.ColorToString(this._color));
        }


        public override void getAttrByXml(XElement xml)
        {
            base.getAttrByXml(xml);
            if (xml.Attribute("Text") != null)
                this.text = XmlHelper.GetString(xml, "Text");
            if (xml.Attribute("FontSize") != null)
                this.font = new Font(defaultFontName, XmlHelper.GetFloat(xml, "FontSize"), FontStyle.Regular);
            if(xml.Attribute("Color") != null)
                this.color = UIHelper.StringToColor(XmlHelper.GetString(xml, "Color"));
        }
    }
}
