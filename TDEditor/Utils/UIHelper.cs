using TDEditor.project;
using TDEditor.Widgets;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TDEditor.Utils
{
    class UIHelper
    {
        public static RenderBase CEGenerateItemByName(String type, String path = null, RenderBase parent = null)
        {
            RenderBase item = null;
            try
            {
                if (parent != null)
                    parent.recordChildItems();
                item = generateItemByName(type, path, parent);
            }
            catch (NestException ex)
            {
                if (parent != null)
                    parent.recoverChildItems();
            	EventManager.RaiserEvent(Constant.StatusInfoChange, null, String.Format("{0} 文件不能嵌套使用", ex.Message));
            }
            return item;
        }

        private static RenderBase generateItemByName(String type, String path = null, RenderBase parent = null)
        {
            RenderBase item = null;
            bool isComplete = true;
            switch (type)
            {
                case Constant.TypeSprite:
                    item = new TDSprite();
                    break;
                case Constant.TypeSliderBar:
                    item = new TDSliderBar();
                    break;
                case Constant.TypeScale9:
                    item = new TDScale9();
                    break;
                case Constant.TypeButton:
                    item = new TDButton();
                    break;
                case Constant.TypeCheckBox:
                    item = new TDCheckBox();
                    break;
                case Constant.TypeInput:
                    item = new TDInput();
                    break;
                case Constant.TypeProgressBar:
                    item = new TDProgressBar();
                    break;
                case Constant.TypeText:
                    item = new TDText();
                    break;
                case Constant.TypePanel:
                    if (path == null || path.Length == 0)
                    {
                        item = new TDPanel();
                    }
                    else
                    {
                        item = generateViewByPath(path, parent);
                    }
                    isComplete = true;
                    break;
                case Constant.TypePage:
                    item = new TDPage();
                    break;
                default:
                    item = null;
                    break;
            }
            if (parent != null && item != null && item.getParent() == null)
            {
                item.setParent(parent);
            }
            if (item != null && isComplete)
            {
                item.loadComplete();
            }

            return item;
        }

        private static RenderBase generateViewBySrc(String src, RenderBase parent)
        {
            return generateViewByXml(XmlHelper.Parse(src), parent);
        }

        public static RenderBase CEGenerateViewByXml(XElement xml, RenderBase parent)
        {
            RenderBase item = null;
            try
            {
                if (parent != null)
                    parent.recordChildItems();
                item = generateViewByXml(xml, parent);
            }
            catch (NestException ex)
            {
                if (parent != null)
                    parent.recoverChildItems();
                EventManager.RaiserEvent(Constant.StatusInfoChange, null, String.Format("{0} 文件不能嵌套使用", ex.Message));
            }
            return item;
        }
             
        private static RenderBase generateViewByXml(XElement xml, RenderBase parent)
        {
            if (xml == null)
            {
                return null;
            }
            RenderBase item = null;

            String name = XmlHelper.GetString(xml, "Name");
            String path = XmlHelper.GetString(xml, "Path");
            if (name.Length > 0)
            {
                if (parent != null && path.Length > 0 && parent.checkParentPathNest(path))
                {
                    throw new NestException(UIProject.Instance().GetRelativePath(path));
                }
                item = generateItemByName(name, path, parent);
                if (item == null)
                {
                    return item;
                }
                item.getAttrByXml(xml);
                //XElement child = xml.Element("Child");
                //if (child != null)
                //{
                //    foreach (XElement element in child.Elements("Node"))
                //    {
                //        generateViewByXml(element, item);
                //    }
                //}
                item.loadComplete();
            }

            return item;
        }

        public static XElement generateXmlByItem(RenderBase item)
        {
            XElement xml = new XElement("Node");
            item.setAttrToXml(ref xml);
            return xml;
        }

        public static String generateSrcByItem(RenderBase item)
        {
            XElement xml = generateXmlByItem(item);
            return xml.ToString();
        }
        
         private static RenderBase generateViewByPath(String path, RenderBase parent = null)
        {
            String absPath = UIProject.Instance().GetRealFile(path);
            String content = FileHelper.GetFullContent(absPath);
            XElement xml = XmlHelper.Parse(content);
            if (xml == null)
            {
                return null;
            }
            if (parent != null && path.Length > 0 && parent.checkParentPathNest(UIProject.Instance().GetRelativePath(absPath)))
            {
                throw new NestException(UIProject.Instance().GetRelativePath(absPath));
            }
            xml.SetAttributeValue("Name", Constant.TypePanel);
            RenderBase render = generateViewByXml(xml, parent);
            if (render is TDPanel)
            {
                (render as TDPanel).panelPath = UIProject.Instance().GetRelativePath(absPath);
            }
            return render;
        }

        public static void loadViewToScene(String path, RenderScene scene)
        {
            scene.removeAllChild();
            String absPath = UIProject.Instance().GetRealFile(path);
            String content = FileHelper.GetFullContent(absPath);
            XElement xml = XmlHelper.Parse(content);
            if (xml == null)
            {
                return;
            }
            scene.getAttrByXml(xml);
            scene.Refresh();
        }

        public static void loadViewToScene(XElement xml, RenderScene scene)
        {
            scene.removeAllChild();
            scene.getAttrByXml(xml);
            scene.Refresh();
        }

        public static Color StringToColor(String str)
        {
            str = str.Replace("#", "");
            UInt32 num = 0;
            try
            {
                num =  Convert.ToUInt32(str.Replace("#", ""), 16);
            }
            catch
            {
            	return Color.Black;
            }
            if (str.Length == 6)
            {
                return Color.FromArgb(255, (int)num >> 16 & 0xff, (int)num >> 8 & 0xff, (int)num & 0xff);
            } 
            return Color.FromArgb((int)num >> 24 & 0xff, (int)num >> 16 & 0xff, (int)num >> 8 & 0xff, (int)num & 0xff);
        }

        public static String ColorToString(Color color)
        {
            int args = color.ToArgb();
            if (color.A == 255)
            {
                return "#" + color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2");
            }
            else
            {
                return "#" + color.A.ToString("X2") + color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2");
            }
        }
    }
}
