using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using TDEditor.Data;
using TDEditor.Utils;
using TDEditor.Prop;
using System.Xml;
using System.Xml.Linq;

namespace TDEditor.Widgets
{
    public enum ClickPosArena
    {
        PosCenter = 0, //中间，无缩放
        PosNESW = 0x0001, //右上，左下
        PosNS = 0x0002, //上下
        PosNWSE = 0x0004, //左上，右下
        PosWE = 0x0008, //左右
        PosXLeft = 0x0010, //是否点到左边
        PosYUp = 0x0020, //是否点到左边
    }
    public partial class RenderBase : UserControl
    {

        protected List<RenderBase> childItems = new List<RenderBase>();
        private List<RenderBase> recordListItems = new List<RenderBase>();

        public List<RenderBase> ChildItems
        {
            get
            {
                return childItems;
            }
            set
            {
                this.childItems = value;
            }
        }

        private String _uniqueName;

        public String uniqueName
        {
            get
            {
                if (_uniqueName == null)
                {
                    _uniqueName = this.Name + "-" + System.Guid.NewGuid().ToString();
                }
                return _uniqueName;
            }
            set
            {
                _uniqueName = value;
            }
        }
        
        private RenderBase parent = null;

        private PointF _anchorPos = new PointF(0.5f, 0.5f);
        [TypeConverterAttribute(typeof(PointFConverter))]
        public PointF anchorPos
        {
            set
            {
                float offsetX = size.Width * scale.X * (value.X - _anchorPos.X);
                float offsetY = size.Height * scale.Y * (value.Y - _anchorPos.Y);
                _anchorPos = value;
                this.pos = new PointF(_pos.X + offsetX, _pos.Y + offsetY);
                raisePropChange();
            }
            get
            {
                return this._anchorPos;
            }
        }

        private PointF _oriAnchorPos = new PointF(0.5f, 0.5f);
        [TypeConverterAttribute(typeof(PointFConverter))]
        public PointF oriAnchorPos
        {
            set
            {
                this._oriAnchorPos = value;
            }
            get
            {
                return this._oriAnchorPos;
            }
        }

        private PointF _pos;
        [TypeConverterAttribute(typeof(PointFConverter))]
        public PointF pos
        {
            set
            {
                _pos = value;
                raisePropChange();
            }
            get
            {
                return this._pos;
            }
        }

        private PointF _oriPos;
        public PointF oriPos
        {
            set
            {
                _oriPos = value;
            }
            get
            {
                return this._oriPos;
            }
        }

        private PointF _scale = new PointF(1.0f, 1.0f);
        [TypeConverterAttribute(typeof(PointFConverter))]
        public PointF scale
        {
            set
            {
                _scale = value;
                if (Math.Abs(_scale.X - 0) < 0.01)
                {
                    _scale.X = 0.01f;
                }
                if (Math.Abs(_scale.Y - 0) < 0.01)
                {
                    _scale.Y = 0.01f;
                }
                raisePropChange();
            }
            get
            {
                return this._scale;
            }
        }

        private PointF _oriScale;
        public PointF oriScale
        {
            set
            {
                _oriScale = value;
            }
            get
            {
                return this._oriScale;
            }
        }
        private SizeF _size;
        //[TypeConverterAttribute(typeof(SizeFConverter))]
        public virtual SizeF size
        {
            set
            {
                this._size = new SizeF(Math.Max(value.Width, 1), Math.Max(value.Height, 1));
                raisePropChange();
            }
            get
            {
                return this._size;
            }
        }

        private SizeF _oriSize;
        //[TypeConverterAttribute(typeof(SizeFConverter))]
        public virtual SizeF oriSize
        {
            set
            {
                this._oriSize = value;
            }
            get
            {
                return this._oriSize;
            }
        }


        private float _rotation = 0;
        public float rotation
        {
            get
            {
                return _rotation;
            }
            set
            {
                _rotation = value;
                raisePropChange();
            }
        }


        private bool _visible = false;
        public bool visible
        {
            get
            {
                return _visible;
            }
            set
            {
                _visible = value;
                raisePropChange();
            }
        }
        private String _tag = "";
        new public String Tag
        {
            get
            {
                return _tag;
            }
            set
            {
                _tag = value;
            }
        }

        private int _id = 0;
        public int Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }

        private String _event = "";
        public String EventStr
        {
            get
            {
                return _event;
            }
            set
            {
                _event = value;
            }
        }
        public bool isSelect = false;

        //范围限制
        public bool isLimitRange = true;
        public RenderBase()
        {
            this.DragDrop += new DragEventHandler(Item_DragDrop);
            this.DragEnter += new DragEventHandler(Item_DragEnter);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        public void loadComplete()
        {
            visible = true;
        }

        protected void paintCursorRect(Graphics g)
        {
            if (!isSelect)
                return;
            RectangleF rect = boundingBox();
            Pen pen = new Pen(Color.Black);
            g.DrawLine(pen, rect.X + rect.Width - 9, rect.Y + rect.Height, rect.X + rect.Width, rect.Y + rect.Height - 9);
            g.DrawLine(pen, rect.X + rect.Width - 6, rect.Y + rect.Height, rect.X + rect.Width, rect.Y + rect.Height - 6);
            g.DrawLine(pen, rect.X + rect.Width - 3, rect.Y + rect.Height, rect.X + rect.Width, rect.Y + rect.Height - 3);

            g.DrawLine(pen, rect.X + 9, rect.Y + rect.Height, rect.X, rect.Y + rect.Height - 9);
            g.DrawLine(pen, rect.X + 6, rect.Y + rect.Height, rect.X, rect.Y + rect.Height - 6);
            g.DrawLine(pen, rect.X + 3, rect.Y + rect.Height, rect.X, rect.Y + rect.Height - 3);

            g.DrawLine(pen, rect.X + 9, rect.Y, rect.X, rect.Y + 9);
            g.DrawLine(pen, rect.X + 6, rect.Y, rect.X, rect.Y + 6);
            g.DrawLine(pen, rect.X + 3, rect.Y, rect.X, rect.Y + 3);

            g.DrawLine(pen, rect.X + rect.Width - 9, rect.Y, rect.X + rect.Width, rect.Y + 9);
            g.DrawLine(pen, rect.X + rect.Width - 6, rect.Y, rect.X + rect.Width, rect.Y + 6);
            g.DrawLine(pen, rect.X + rect.Width - 3, rect.Y, rect.X + rect.Width, rect.Y + 3);

            g.DrawRectangle(new Pen(Color.Black), 0, 0, size.Width, size.Height);
        }

        protected virtual void paintSelft(object sender, PaintEventArgs e)
        {
            e.Graphics.ResetClip();
            paintCursorRect(e.Graphics);
        }

        public virtual void TDPaint(object sender, PaintEventArgs e)
        {
            GraphicsState transState = e.Graphics.Save();
            e.Graphics.TranslateTransform(pos.X, pos.Y);
            float offsetX = -size.Width * scale.X * anchorPos.X;
            float offsetY = -size.Height * scale.Y * anchorPos.Y;
            e.Graphics.TranslateTransform(offsetX, offsetY);
            e.Graphics.ScaleTransform(scale.X, scale.Y);
            e.Graphics.RotateTransform(rotation);
            e.Graphics.SetClip(boundingBox(), CombineMode.Intersect);
            if (!visible)
            {
                e.Graphics.ResetClip();
                paintCursorRect(e.Graphics);
            }
            else
            {
                paintSelft(sender, e);
            }
            e.Graphics.Restore(transState);
        }

        protected virtual void Item_DragDrop(object sender, DragEventArgs e)
        {
            Console.WriteLine("drag");
        }

        protected virtual void Item_DragEnter(object sender, DragEventArgs e)
        {
            object data = e.Data.GetData(typeof(ControlDDData));
            if (data == null || (data as ControlDDData).controlType == null)
            {
                return;
            }
            e.Effect = DragDropEffects.Copy;
        }

        public void removeFromParent()
        {
            if (this.parent != null)
            {
                this.parent.removeChild(this);
            }
        }

        public void removeChild(RenderBase child)
        {
            child.parent = null;
            childItems.Remove(child);
        }

        public void removeAllChild()
        {
            foreach (RenderBase render in childItems)
            {
                render.parent = null;
            }
            childItems.Clear();
        }

        public void addChild(RenderBase child)
        {
            Debug.Assert(child != null && child.parent == null);
            this.childItems.Add(child);
            child.parent = this;
        }

        public void setParent(RenderBase parent)
        {
            Debug.Assert(this.parent == null && parent != null);
            parent.addChild(this);
        }

        public RenderBase getParent()
        {
            return this.parent;
        }

        public RectangleF boundingBox()
        {
            return new RectangleF(0, 0, size.Width, size.Height);
        }

        public RectangleF boxAtParent()
        {
            PointF start = startPos();
            return new RectangleF(start.X, start.Y, size.Width * Math.Abs(scale.X), size.Height * Math.Abs(scale.Y));
        }

        public PointF startPos()
        {
            float startX = pos.X - scale.X * size.Width * anchorPos.X + (scale.X < 0 ? scale.X * size.Width : 0);
            float startY = pos.Y - scale.Y * size.Height * anchorPos.Y + (scale.Y < 0 ? scale.Y * size.Height : 0);
            return new PointF(startX, startY);
        }

        public bool isIntersect(RectangleF rect)
        {
            return boxAtParent().IntersectsWith(rect);
        }

        public List<RenderBase> getIntersectItem(RectangleF rect)
        {
            List<RenderBase> items = new List<RenderBase>();
            //foreach (RenderBase render in childItems)
            //{
            for (int i = childItems.Count - 1; i >= 0; i--)
            {
                RenderBase render = childItems[i];
                if (render.isIntersect(rect))
                {
                    items.Add(render);
                }
            }
            return items;
        }

        public List<RenderBase> getSelectItems()
        {
            List<RenderBase> items = new List<RenderBase>();
            //foreach (RenderBase render in childItems)
            //{
            for (int i = childItems.Count - 1; i >= 0; i--)
            {
                RenderBase render = childItems[i];
                if (render.isSelect)
                {
                    items.Add(render);
                }
                else if (isCanDeepPanel(render))
                {
                    foreach (RenderBase select in render.getSelectItems())
                    {
                        items.Add(select);
                    }
                }
            }
            return items;
        }

        public bool isCanDeepPanel(RenderBase render)
        {
            if (render is TDPanel && (render as TDPanel).panelPath.Length == 0)
            {
                return true;
            }
            return false;
        }

        public void clearSelectStatus()
        {
            foreach (RenderBase render in childItems)
            {
                render.clearSelectStatus();
            }
            this.isSelect = false;
        }

        public ClickPosArena calcClickArena(PointF pos)
        {
            ClickPosArena arena = ClickPosArena.PosCenter;
            float actionWidth = size.Width / 10;
            float actionHeight = size.Height / 10;
            RectangleF rect = boxAtParent();

            if (pos.X < rect.X || pos.X > rect.X + rect.Width
                || pos.Y < rect.Y || pos.Y > rect.Y + rect.Height)
            {
                arena = ClickPosArena.PosCenter;
            }
            else if (pos.X <= rect.X + actionWidth && pos.Y <= rect.Y + actionHeight)
            {
                arena = ClickPosArena.PosNWSE | ClickPosArena.PosXLeft | ClickPosArena.PosYUp;
            }
            else if (pos.X >= rect.X + rect.Width - actionWidth && pos.Y >= rect.Y + rect.Height - actionHeight)
            {
                arena = ClickPosArena.PosNWSE;
            }
            else if (pos.X <= rect.X + actionWidth && pos.Y >= rect.Y + rect.Height - actionHeight)
            {
                arena = ClickPosArena.PosNESW | ClickPosArena.PosXLeft;
            }
            else if (pos.X >= rect.X + rect.Width - actionWidth && pos.Y <= rect.Y + actionHeight)
            {
                arena = ClickPosArena.PosNESW | ClickPosArena.PosYUp;
            }
            else if (pos.X <= rect.X + actionWidth)
            {
                arena = ClickPosArena.PosWE | ClickPosArena.PosXLeft;
            }
            else if (pos.X >= rect.X + rect.Width - actionWidth)
            {
                arena = ClickPosArena.PosWE;
            }
            else if (pos.Y >= rect.Y + rect.Height - actionHeight)
            {
                arena = ClickPosArena.PosNS;
            }
            else if (pos.Y <= rect.Y + actionHeight)
            {
                arena = ClickPosArena.PosNS | ClickPosArena.PosYUp;
            }
            return arena;
        }

        public RenderBase getClickItem(PointF pos, out ClickPosArena arena)
        {
            arena = ClickPosArena.PosCenter;
            //foreach (RenderBase render in childItems)
            //{
            for (int i = childItems.Count - 1; i >= 0; i-- )
            {
                RenderBase render = childItems[i];
                RectangleF rect = render.boxAtParent();
                if (rect.Contains(pos))
                {
                    if (isCanDeepPanel(render))
                    {
                        PointF relaPos = new PointF(pos.X - rect.X, pos.Y - rect.Y);
                        RenderBase clickItem = render.getClickItem(relaPos, out arena);
                        if (clickItem != null)
                        {
                            arena = clickItem.calcClickArena(relaPos);
                            return clickItem;
                        }
                    }
                    arena = render.calcClickArena(pos);
                    return render;
                }
            }
            return null;
        }

        public RenderBase getSelectClickItem(PointF pos, out ClickPosArena arena)
        {
            arena = ClickPosArena.PosCenter;
            //foreach (RenderBase render in childItems)
            //{
            for (int i = childItems.Count - 1; i >= 0; i--)
            {
                RenderBase render = childItems[i];
                RectangleF rect = render.boxAtParent();
                if (isCanDeepPanel(render))
                {
                    PointF relaPos = new PointF(pos.X - rect.X, pos.Y - rect.Y);
                    RenderBase clickItem = render.getSelectClickItem(relaPos, out arena);
                    if (clickItem != null)
                    {
                        arena = clickItem.calcClickArena(relaPos);
                        return clickItem;
                    }
                }
                else if (render.isSelect && rect.Contains(pos))
                {
                    arena = render.calcClickArena(pos);
                    return render;
                }
            }
            return null;
        }

        public void checkSelectStatus(RectangleF rect, bool deep = false)
        {
            foreach (RenderBase render in childItems)
            {
                if (render.isIntersect(rect))
                {
                    render.isSelect = true;
                }
            }
        }

        public RenderBase getFirstSelectItem()
        {
            //foreach (RenderBase render in childItems)
            //{
            for (int i = childItems.Count - 1; i >= 0; i--)
            {
                RenderBase render = childItems[i];
                if (render.isSelect)
                {
                    return render;
                }
            }
            return null;
        }

        public int CountSelectNum()
        {
            int num = 0;
            foreach (RenderBase render in childItems)
            {
                if (render.isSelect)
                {
                    num++;
                }
                num += render.CountSelectNum();
            }
            return num;
        }

        protected virtual void item_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isSelect = true;
            }
        }

        //pbimg＂鼠标移动＂事件处理方法
        protected virtual void item_MouseMove(object sender, MouseEventArgs e)
        {
            //Thread.Sleep(6);//减少cpu占用率
            //if (isStartSelct)
            //{
            //    selectCurPos = new Point(e.X, e.Y);
            //    Refresh();
            //}
        }

        //pbimg＂鼠标松开＂事件处理方法
        protected virtual void item_MouseUp(object sender, MouseEventArgs e)
        {
            //if (isStartSelct)
            //{
            //    isStartSelct = false;
            //    Refresh();
            //}
        }

        protected virtual void item_MouseLeave(object sender, EventArgs e)
        {
            //isStartSelct = false;
            //Console.WriteLine("leave");
        }

        protected void setCursorByArena(ClickPosArena arena)
        {
            if ((arena & ClickPosArena.PosNESW) != 0)
            {
                this.Cursor = Cursors.SizeNESW;
            }
            else if ((arena & ClickPosArena.PosNS) != 0)
            {
                this.Cursor = Cursors.SizeNS;
            }
            else if ((arena & ClickPosArena.PosNWSE) != 0)
            {
                this.Cursor = Cursors.SizeNWSE;
            }
            else if ((arena & ClickPosArena.PosWE) != 0)
            {
                this.Cursor = Cursors.SizeWE;
            }
            else
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        public void ProcessKeyEvent(KeyEventArgs e)
        {
            PointF curPos = this.pos;

            int step = 1;
            if (e.Modifiers == Keys.Shift)
            {
                step *= 10;
            }
            switch (e.KeyCode)
            {
                case Keys.Left:
                    curPos.X = curPos.X - step;
                    break;
                case Keys.Up:
                    curPos.Y = curPos.Y - step;
                    break;
                case Keys.Right:
                    curPos.X = curPos.X + step;
                    break;
                case Keys.Down:
                    curPos.Y = curPos.Y + step;
                    break;
                default:
                    break;
            }
            this.pos = curPos;
        }

        public void DrawCenter(Graphics p, Image img)
        {
            if (img == null)
            {
                return;
            }
            float offsetX = (this.size.Width - img.Width) / 2;
            float offsetY = (this.size.Height - img.Height) / 2;
            p.DrawImage(img, offsetX, offsetY);
        }

        public void DrawCenter(Graphics p, Font font, String text, Brush brush = null)
        {
            SizeF size = p.MeasureString(text, font, (int)this.size.Width);
            float offsetX = (this.size.Width - size.Width) / 2;
            float offsetY = (this.size.Height - size.Height) / 2;
            p.DrawString(text, font, brush == null ? Brushes.Black : brush, offsetX, offsetY);
        }

        public void changeMoveSize(ClickPosArena clickPosArena, SizeF moveSize)
        {
            if ((clickPosArena & ClickPosArena.PosNS) != 0)
            {
                moveSize.Width = 0;
            }
            else if ((clickPosArena & ClickPosArena.PosWE) != 0)
            {
                moveSize.Height = 0;
            }
            if ((clickPosArena & ClickPosArena.PosXLeft) != 0)
            {
                moveSize.Width = -moveSize.Width;
            }
            if ((clickPosArena & ClickPosArena.PosYUp) != 0)
            {
                moveSize.Height = -moveSize.Height;
            }
            this.size += moveSize;
        }


        public void raisePropChange()
        {
            if (this.parent == null)
            {
                return;
            }
            EventManager.RaiserEvent(Constant.PropChange, this, null);
        }

        public void raiseSelctChange()
        {
            if (this.parent == null)
            {
                return;
            }
            EventManager.RaiserEvent(Constant.PropSelectChange, this, null);
        }

        public virtual String getAttrSource()
        {
            XElement xml = new XElement("Node");
            setAttrToXml(ref xml);
            return xml.ToString();
        }

        public virtual void setAttrToXml(ref XElement xml)
        {
            String format = Constant.DefaultSingleFormat;
            xml.SetAttributeValue("Name", this.Name);
            xml.SetAttributeValue("X", this._pos.X.ToString(format));
            xml.SetAttributeValue("Y", this._pos.Y.ToString(format));
            xml.SetAttributeValue("Width", this._size.Width.ToString(format));
            xml.SetAttributeValue("Height", this._size.Height.ToString(format));
            if (this._scale != Constant.PosF11)
            {
                xml.SetAttributeValue("ScaleX", this._scale.X.ToString(format));
                xml.SetAttributeValue("ScaleY", this._scale.Y.ToString(format));
            }
            if (this._anchorPos != Constant.PosF05)
            {
                xml.SetAttributeValue("AnchorX", this._anchorPos.X.ToString(format));
                xml.SetAttributeValue("AnchorY", this._anchorPos.Y.ToString(format));
            }

            if (!this._visible)
            {
                xml.SetAttributeValue("Visible", this._visible.ToString());
            }

            if (this._tag.Length > 0)
            {
                xml.SetAttributeValue("Tag", this._tag);
            }

            if (this._event.Length > 0)
            {
                xml.SetAttributeValue("Event", this._event);
            }

            if (this._id != 0)
            {
                xml.SetAttributeValue("Id", this._id);
            }
        }

        public virtual void setAttrBySource(String src) {
            XElement xml = XmlHelper.Parse(src);
            if (xml == null)
            {
                return;
            }
            getAttrByXml(xml);
        }

        public virtual void getAttrByXml(XElement xml)
        {
            this._pos = new PointF(XmlHelper.GetFloat(xml, "X"), XmlHelper.GetFloat(xml, "Y"));
            this._size = new SizeF(XmlHelper.GetFloat(xml, "Width"), XmlHelper.GetFloat(xml, "Height"));
            if (xml.Attribute("ScaleX") != null || xml.Attribute("ScaleY") != null)
            {
                this._scale = new PointF(XmlHelper.GetFloat(xml, "ScaleX", 1), XmlHelper.GetFloat(xml, "ScaleY", 1));
            }
            if (xml.Attribute("AnchorX") != null || xml.Attribute("AnchorY") != null)
            {
                this._anchorPos = new PointF(XmlHelper.GetFloat(xml, "AnchorX", 0.5f), XmlHelper.GetFloat(xml, "AnchorY", 0.5f));
            }
            if (xml.Attribute("Visible") != null)
            {
                this._visible = XmlHelper.GetBool(xml, "Visible", true);
            }

            if (xml.Attribute("Tag") != null)
            {
                this._tag = XmlHelper.GetString(xml, "Tag");
            }

            if (xml.Attribute("Event") != null)
            {
                this._event = XmlHelper.GetString(xml, "Event");
            }
            if (xml.Attribute("Id") != null)
            {
                this._id = XmlHelper.GetInt(xml, "Id");
            }

        }

        public bool checkParentPathNest(String checkPath)
        {
            RenderBase parent = this;
            do 
            {
                if (parent is TDPanel)
                {
                    if (checkPath == (parent as TDPanel).panelPath)
                    {
                        return true;
                    }
                }
                parent = parent.parent;
            } while (parent != null);
            return false;
        }

        public void recordOriStatus()
        {
            _oriPos = pos;
            _oriAnchorPos = anchorPos;
            _oriScale = scale;
            _oriSize = size;
        }

        public RenderScene getRenderScene()
        {
            RenderBase parent = this;
            do
            {
                if (parent is RenderScene)
                {
                    return parent as RenderScene;
                }
                parent = parent.parent;
            } while (parent != null);
            return null;
        }

        public void recordChildItems()
        {
            recordListItems.Clear();
            foreach (RenderBase child in childItems)
            {
                recordListItems.Add(child);
            }
        }

        public void recoverChildItems()
        {
            this.removeAllChild();
            foreach (RenderBase child in recordListItems)
            {
                this.addChild(child);
            }
        }

        public static TDPanel GetPanelNode(RenderBase render)
        {
            if (render == null)
            {
                return null;
            }
            RenderBase parent = render;
            do 
            {
                if (parent is TDPanel && !(parent is RenderScene))
                {
                    return parent as TDPanel;
                }
            } while ((parent = parent.parent) != null);
            
            return null;
        }
    }
}
