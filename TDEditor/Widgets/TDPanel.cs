using TDEditor.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using System.ComponentModel;

namespace TDEditor.Widgets
{
    public class TDPanel : RenderBase
    {
        static Color lightBlue = Color.FromArgb(200, 0xAD, 0xD8, 0xE6);

        private String _panelPath = "";
        
        [ReadOnlyAttribute(true)]
        public String panelPath
        {
            set
            {
                _panelPath = value;
            }
            get
            {
                return _panelPath;
            }
        }
        public TDPanel()
        {
            this.size = new SizeF(100, 100);
            this.Name = Constant.TypePanel;
        }
        protected override void paintSelft(object sender, PaintEventArgs e)
        {

            e.Graphics.FillRectangle(new SolidBrush(lightBlue), 0, 0, this.size.Width, this.size.Height);
            //for (int i = childItems.Count - 1; i >= 0; i--)
            //{
            //    childItems[i].TDPaint(sender, e);
            //}
            foreach (RenderBase render in childItems)
            {
                render.TDPaint(sender, e);
            }
            base.paintSelft(sender, e);
        }

        public override void setAttrToXml(ref XElement xml)
        {
            base.setAttrToXml(ref xml);
            if (_panelPath.Length != 0 && !(this is RenderScene))
            {
                xml.SetAttributeValue("Path", _panelPath);
                return;
            }
            XElement child = new XElement("Child");
            xml.Add(child);
            foreach (RenderBase render in childItems)
            {
                XElement node = new XElement("Node");
                render.setAttrToXml(ref node);
                child.Add(node);
            }
        }


        public override void getAttrByXml(XElement xml)
        {
            base.getAttrByXml(xml);
            XElement child = xml.Element("Child");
            if (child == null)
            {
                return;
            }
            foreach (XElement node in child.Elements("Node"))
            {
                UIHelper.CEGenerateViewByXml(node, this);
            }
            //String path = XmlHelper.GetString(xml, "Path");
            //if (path.Length == 0)
            //{

            //}
            //else
            //{

            //}

        }
        public RenderBase getRenderByUniqueName(String uniqueName)
        {
            RenderBase item = null;
            if (this.uniqueName == uniqueName)
            {
                return this;
            }
            foreach (RenderBase render in childItems)
            {
                if (render.uniqueName == uniqueName)
                {
                    item = render;
                    break;
                }
                if (render is TDPanel)
                {
                    item = (render as TDPanel).getRenderByUniqueName(uniqueName);
                    if (item != null)
                    {
                        break;
                    }
                }
            }
            return item;
        }
    }
}
