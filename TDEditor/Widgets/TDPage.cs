using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using TDEditor.Utils;

namespace TDEditor.Widgets
{
    public class TDPage : RenderBase
    {
        static Color lightBlue = Color.FromArgb(200, 0xAD, 0xD8, 0xE6);
        private RenderBase _renderItem = null;
        public RenderBase renderItem
        {
            get
            {
                return _renderItem;
            }
            set
            {
                _renderItem = value;
                raisePropChange();
            }
        }

        const int defaultMax = 9;
        private int _max = defaultMax;
        public int max
        {
            get
            {
                return _max;
            }
            set
            {
                _max = value;
                raisePropChange();
            }
        }
        const int defaultCol = 3;
        private int _col = defaultCol;
        public int col
        {
            get
            {
                return _col;
            }
            set
            {
                _col = value;
                raisePropChange();
            }
        }
        const int defaultRow = 3;
        private int _row = defaultRow;
        public int row
        {
            get
            {
                return _row;
            }
            set
            {
                _row = value;
                raisePropChange();
            }
        }
        const int defaultPadCol = 3;
        private int _padCol = defaultPadCol;
        public int padCol
        {
            get
            {
                return _padCol;
            }
            set
            {
                _padCol = value;
                raisePropChange();
            }
        }
        const int defaultPadRow = 3;
        private int _padRow = defaultPadRow;
        public int padRow
        {
            get
            {
                return _padRow;
            }
            set
            {
                _padRow = value;
                raisePropChange();
            }
        }

        public String rederPath
        {
            get
            {
                if (_renderItem != null && _renderItem is TDPanel)
                {
                    return (_renderItem as TDPanel).panelPath;
                }
                return "";
            }
            set
            {
                _renderItem = UIHelper.CEGenerateItemByName(Constant.TypePanel, value, this);
                raisePropChange();
            }
        }

        private PointF getPointByIndex(int index)
        {
            if (_renderItem == null)
            {
		        return new PointF(0, 0);
	        }
            RectangleF itemRect = _renderItem.boxAtParent();
	        int row = index / _col;
            int col = index % _col;
            return new PointF(-itemRect.X + col * (_renderItem.size.Width + _padCol), -itemRect.Y + row * (_renderItem.size.Height + _padRow));
        }

        public TDPage()
        {
            this.size = new SizeF(200, 200);
            this.Name = Constant.TypePage;
        }


        protected override void paintSelft(object sender, PaintEventArgs e)
        {
            e.Graphics.FillRectangle(new SolidBrush(lightBlue), 0, 0, this.size.Width, this.size.Height);
            for (int j = 0; j < _row; j++)
            {
                for (int i = 0; i < _col; i++)
                {
                    int index = j + i * _row;
                    if (index >= _max)
                    {
                        continue;
                    }
                    PointF point = getPointByIndex(index);
                    if (_renderItem != null)
                    {
                        e.Graphics.TranslateTransform(point.X, point.Y);
                        _renderItem.TDPaint(sender, e);
                        e.Graphics.TranslateTransform(-point.X, -point.Y);
                    }
                }
            }
            base.paintSelft(sender, e);
        }

        public override void setAttrToXml(ref XElement xml)
        {
            base.setAttrToXml(ref xml);
            if (_col != defaultCol)
                xml.SetAttributeValue("Col", this._col);
            if (_row != defaultRow)
                xml.SetAttributeValue("Row", this._row);
            if (_padRow != defaultPadRow)
                xml.SetAttributeValue("PadRow", this._padRow);
            if (_padCol != defaultPadCol)
                xml.SetAttributeValue("PadCol", this._padCol);
            if (_max != defaultMax)
                xml.SetAttributeValue("Max", this._max);
            if (_renderItem != null)
            {
                XElement node = new XElement("RenderItem");
                _renderItem.setAttrToXml(ref node);
                xml.Add(node);
            }
        }


        public override void getAttrByXml(XElement xml)
        {
            base.getAttrByXml(xml);
            if (xml.Attribute("Col") != null)
                this._col = XmlHelper.GetInt(xml, "Col");
            if (xml.Attribute("Row") != null)
                this._row = XmlHelper.GetInt(xml, "Row");
            if (xml.Attribute("PadCol") != null)
                this._padCol = XmlHelper.GetInt(xml, "PadCol");
            if (xml.Attribute("PadRow") != null)
                this._padRow = XmlHelper.GetInt(xml, "PadRow");
            if (xml.Attribute("Max") != null)
                this._max = XmlHelper.GetInt(xml, "Max");
            if (xml.Element("RenderItem") != null)
                this._renderItem = UIHelper.CEGenerateViewByXml(xml.Element("RenderItem"), this);
        }

    }
}
