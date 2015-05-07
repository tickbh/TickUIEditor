using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using TDEditor.Data;
using TDEditor.Utils;
using TDEditor.Dock;
using System.Xml.Linq;
using TDEditor.project;
using TDEditor.Recall;
using System.Drawing.Drawing2D;

namespace TDEditor.Widgets
{
    enum MouseOpStatus
    {
        StatusNode,
        StatusSelect,
        StatusMove,
        StatusResize,
    }

    public partial class RenderScene : TDPanel
    {
        private MouseOpStatus mouseStatus = MouseOpStatus.StatusNode;
        private ClickPosArena clickPosArena = ClickPosArena.PosCenter;
        private CommandManager _commandManager = new CommandManager();
        public CommandManager commandManager
        {
            get
            {
                return _commandManager;
            }
        }

        private List<RenderBase> mouseOpItems = new List<RenderBase>();

        private PointF selectStartPos = PointF.Empty;
        private PointF selectCurPos = PointF.Empty;

        private Point DefaultOffsetPos = new Point(100, 100);
        private TDCoord horCoord = new TDCoord(COORD_DIRECTION.DIRECTION_HORIZONTAL);
        private TDCoord verCoord = new TDCoord(COORD_DIRECTION.DIRECTION_VERTICAL);
        public Size _sceneSize = new Size(800, 600);


        private PointF _renderCenterPos;
        public PointF renderCenterPos
        {
            set
            {
                _renderCenterPos = value;
            }
            get
            {
                return _renderCenterPos;
            }
        }
        
        private float _renderScale = 1;
        public float renderScale
        {
            set
            {
                _renderScale = value;
                Invalidate();
            }
            get
            {
                return _renderScale;
            }
        }

        public bool isModify
        {
            get
            {
                return commandManager.IsModify();
            }
        }

        public RenderDock renderDock;

        public Size sceneSize
        {
            get
            {
                return _sceneSize;
            }
            set
            {
                _sceneSize = value;
                _renderCenterPos = new PointF(_sceneSize.Width / 2, _sceneSize.Height / 2);

                DefaultOffsetPos.X = (this.Size.Width - _sceneSize.Width) / 2;
                DefaultOffsetPos.Y = (this.Size.Height - _sceneSize.Height) / 2;
                Invalidate();
            }
        }

        public RenderScene() : base()
        {
            InitializeComponent();

            this.Paint += new PaintEventHandler(this.Item_Paint);

            this.MouseDoubleClick += new MouseEventHandler(this.item_doubleClick);
            this.MouseDown += new MouseEventHandler(this.item_MouseDown);
            this.MouseMove += new MouseEventHandler(this.item_MouseMove);
            this.MouseUp += new MouseEventHandler(this.item_MouseUp);
            this.MouseLeave += new EventHandler(this.item_MouseLeave);
            this.MouseWheel += new MouseEventHandler(this.item_MouseWheel);
            this.SizeChanged += new EventHandler(this.item_SizeChanged);

            this.KeyDown += new KeyEventHandler(this.item_KeyDown);
            this.KeyUp += new KeyEventHandler(this.item_KeyUp);
            this.DoubleBuffered = true;
            this.Name = Constant.TypeScene;
            EventManager.RegisterAudience(Constant.PropChange, new EventHandler<object>(this.itemChange));
        }

        private Point getFixTrans()
        {
            float startX = (DefaultOffsetPos.X + sceneSize.Width * (1 - _renderScale) / 2) +(sceneSize.Width / 2 - _renderCenterPos.X);
            float startY = (DefaultOffsetPos.Y + sceneSize.Height * (1 - _renderScale) / 2) +(sceneSize.Height / 2 - _renderCenterPos.Y);
            return new Point((int)startX, (int)startY);
        }

        private Point scenePointToRender(int X, int Y)
        {
            Point fixTrans = getFixTrans();
            float fixX = X * _renderScale + fixTrans.X;
            float fixY = Y * _renderScale + fixTrans.Y;
            return new Point((int)fixX, (int)fixY);
        }

        private Point getFixPoint(int X, int Y)
        {
            float startX = (X - getFixTrans().X) / _renderScale;
            float startY = (Y - getFixTrans().Y) / _renderScale;
            return new Point((int)startX, (int)startY);
        }

        private RectangleF getSelectRect()
        {
            return new RectangleF(Math.Min(selectStartPos.X, selectCurPos.X)
                                   , Math.Min(selectStartPos.Y, selectCurPos.Y)
                                   , Math.Abs(selectStartPos.X - selectCurPos.X)
                                   , Math.Abs(selectStartPos.Y - selectCurPos.Y));
        }

        private void Item_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear(Color.White);
            if (DynamicObj.DefaultSceneBgImg != null)
            {
                TextureBrush brush = new TextureBrush(DynamicObj.DefaultSceneBgImg, System.Drawing.Drawing2D.WrapMode.Tile);
                g.FillRectangle(brush, 0, 0, this.Size.Width, this.Size.Height);
            }

            Point zeroPoint = scenePointToRender(0, 0);

            g.DrawLine(Pens.White, new Point(0, zeroPoint.Y), new Point(this.Size.Width, zeroPoint.Y));
            g.DrawLine(Pens.White, new Point(zeroPoint.X, 0), new Point(zeroPoint.X, this.Size.Height));

            Point fixValue = getFixTrans();
            GraphicsState sate = g.Save();
            g.TranslateTransform(fixValue.X, fixValue.Y);
            g.ScaleTransform(_renderScale, _renderScale);
            g.FillRectangle(new SolidBrush(Color.FromArgb(200, 128, 128, 128)), 0, 0, sceneSize.Width, sceneSize.Height);
            //for (int i = childItems.Count - 1; i >= 0; i-- )
            //{
            //    childItems[i].TDPaint(sender, e);
            //}
            foreach (RenderBase render in childItems)
            {
                render.TDPaint(sender, e);
            }
            if (mouseStatus == MouseOpStatus.StatusSelect)
            {
                Pen pen = new Pen(Color.Black, 1);
                pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                Console.WriteLine("start pos is {0}, second pos is {1}", selectStartPos, selectCurPos);
                RectangleF rect = getSelectRect();
                g.DrawRectangle(pen, rect.X, rect.Y, rect.Width, rect.Height);
            }
            g.Restore(sate);

            Point start = getFixPoint(0, 0);
            Point end = getFixPoint(this.Size.Width, this.Size.Height);
            horCoord.SetPoint(start.X, end.X);
            verCoord.SetPoint(start.Y, end.Y);
            horCoord.DrawHorizontal(g, 0, this.renderDock.getToolBar().Height, this.Size.Width, 30);
            verCoord.DrawVertical(g, 0, 0, 30, this.Size.Height);
        }

        public delegate void DragVoke(String type, object controlData, Point p);


        protected override void Item_DragDrop(object sender, DragEventArgs e)
        {
            string[] formats = e.Data.GetFormats();
            ControlDDData data = (ControlDDData)e.Data.GetData(e.Data.GetFormats()[0]);
            DragVoke voke = new DragVoke(addDragItemType);
            Point point = this.PointToClient(new Point(e.X, e.Y));
            Invoke(voke, new Object[] { data.controlType, data.controlData, getFixPoint(point.X, point.Y) });

            //IAsyncResult asyncResult = BeginInvoke(voke, new Object[] { data.controlType, data.controlData, this.PointToClient(new Point(e.X - DefaultOffsetPos.X, e.Y - DefaultOffsetPos.Y)) });
            //EndInvoke(asyncResult);
            //addDragItemType(data.controlType, data.controlData, this.PointToClient(new Point(e.X - DefaultOffsetPos.X, e.Y - DefaultOffsetPos.Y)));
        }

        private void addDragItemType(String type, object controlData, Point p)
        {
            clearSelectStatus();
            //RenderBase clickItem = getClickItem(new PointF(p.X, p.Y), null);
            ClickPosArena arena;
            TDPanel panel = RenderBase.GetPanelNode(getClickItem(p, out arena));
            RenderBase item = null;
            try
            {
                item = UIHelper.CEGenerateItemByName(type, controlData as String, panel == null ? this : panel);
            }
            catch (NestException ex)
            {
                EventManager.RaiserEvent(Constant.StatusInfoChange, this, String.Format("{0} 文件不能嵌套 {1} 文件", this.panelPath, ex.Message));
            }
            if (item != null)
            {
                if (type == Constant.TypeSprite && controlData is String)
                {
                    (item as TDSprite).ImagePath = controlData as String;
                }
                DynamicObj.propDock.SetSelectItem(item);
                if (panel != null)
                {
                    PointF panelPos = panel.startPos();
                    item.pos = new PointF(p.X - panelPos.X, p.Y - panelPos.Y);
                }
                else
                {
                    item.pos = p;
                }
                commandManager.AddCommand(new CommandAdd(this, item));
                checkModifyStatus();
            }

            EventManager.RaiserEvent(Constant.RenderItemChange, this, null);
            Refresh();
        }

        protected override void item_MouseDown(object sender, MouseEventArgs e)
        {

            bool isClickControl = (Control.ModifierKeys & Keys.Control) == Keys.Control;
            bool isClickAlt = (Control.ModifierKeys & Keys.Alt) == Keys.Alt;
            if (e.Button == MouseButtons.Left)
            {
                selectStartPos = selectCurPos = getFixPoint(e.X, e.Y);
                if (isClickControl)
                {
                    RenderBase clickItem = getClickItem(selectStartPos, out clickPosArena);
                    if (clickItem != null)
                    {
                        clickItem.isSelect = !clickItem.isSelect;
                    }
                }
                else
                {
                    RenderBase clickItem = getSelectClickItem(selectStartPos, out clickPosArena);
                    if (clickItem == null || isClickAlt)
                    {
                        clickItem = getClickItem(selectStartPos, out clickPosArena);
                    }
                    if (clickItem != null && clickItem.isSelect && clickPosArena != ClickPosArena.PosCenter)
                    {
                        clearSelectStatus();
                        clickItem.isSelect = true;
                        mouseOpItems.Clear();
                        mouseOpItems.Add(clickItem);
                        mouseStatus = MouseOpStatus.StatusResize;
                        setCursorByArena(clickPosArena);
                    }
                    else if (clickItem != null)
                    {
                        if (clickItem.isSelect)
                        {
                            mouseOpItems = getSelectItems();
                        }
                        else
                        {
                            clearSelectStatus();
                            mouseOpItems.Clear();
                            clickItem.isSelect = true;
                            mouseOpItems.Add(clickItem);
                        }
                        clickItem.raiseSelctChange();
                        mouseStatus = MouseOpStatus.StatusMove;
                    }
                    else
                    {
                        clearSelectStatus();
                        mouseStatus = MouseOpStatus.StatusSelect;
                    }
                }
                Invalidate();
                recordOriStatus(mouseOpItems);
            }
        }

        //pbimg＂鼠标移动＂事件处理方法
        protected override void item_MouseMove(object sender, MouseEventArgs e)
        {
            Thread.Sleep(30);//减少cpu占用率
            selectCurPos = getFixPoint(e.X, e.Y);
            horCoord.setCurPoint((int)selectCurPos.X);
            verCoord.setCurPoint((int)selectCurPos.Y);
            if (mouseStatus == MouseOpStatus.StatusSelect)
            {
                clearSelectStatus();
                checkSelectStatus(getSelectRect());
            }
            else if (mouseStatus == MouseOpStatus.StatusMove)
            {
                SizeF moveSize = new SizeF(selectCurPos.X - selectStartPos.X, selectCurPos.Y - selectStartPos.Y);
                selectStartPos = selectCurPos;
                foreach (RenderBase render in mouseOpItems)
                {
                    render.pos += moveSize;
                }
            }
            else if (mouseStatus == MouseOpStatus.StatusResize)
            {
                SizeF moveSize = new SizeF(selectCurPos.X - selectStartPos.X, selectCurPos.Y - selectStartPos.Y);
                selectStartPos = selectCurPos;
                foreach (RenderBase render in mouseOpItems)
                {
                    render.changeMoveSize(clickPosArena, moveSize);

                }
            }
            else
            {
                ClickPosArena arena;
                RenderBase clickItem = getClickItem(selectCurPos, out arena);
                if (clickItem != null && clickItem.isSelect && arena != ClickPosArena.PosCenter)
                {
                    setCursorByArena(arena);
                }
                else
                {
                    setCursorByArena(ClickPosArena.PosCenter);
                }
            }
            Refresh();
        }

        protected override void item_MouseUp(object sender, MouseEventArgs e)
        {
            if (mouseStatus == MouseOpStatus.StatusSelect)
            {
                clearSelectStatus();
                checkSelectStatus(getSelectRect());
                mouseStatus = MouseOpStatus.StatusNode;
                Refresh();
                if (this.CountSelectNum() == 0)
                {
                    EventManager.RaiserEvent(Constant.PropSelectChange, null, null);
                }
                else if (this.CountSelectNum() == 1)
                {
                    EventManager.RaiserEvent(Constant.PropSelectChange, getFirstSelectItem(), null);
                }
            }
            else if (mouseStatus == MouseOpStatus.StatusMove)
            {
                mouseStatus = MouseOpStatus.StatusNode;
                commandManager.AddCommand(new CommandMoveList(this, mouseOpItems));
                checkModifyStatus();
                Refresh();
            }
            else if (mouseStatus == MouseOpStatus.StatusResize)
            {
                setCursorByArena(ClickPosArena.PosCenter);
                commandManager.AddCommand(new CommandResizeList(this, mouseOpItems));
                checkModifyStatus();
                mouseStatus = MouseOpStatus.StatusNode;
            }

            //ImagePreForm imageForm = new ImagePreForm("e:\\1.png");
            //imageForm.Location = new Point((int)pos.X, (int)pos.Y);
            //imageForm.Show();
            
            //ImageToolTip curToolsTip = new ImageToolTip("e:\\1.png");
            ////curToolsTip.Show("fdas", this);
            ////this.treeView1.ShowNodeToolTips = true;
            //curToolsTip.SetToolTip(this, "my resource pre view");
        }

        public void selectItemByUniqueName(String uniqueName)
        {
            RenderBase render = this.getRenderByUniqueName(uniqueName);
            if (render == null)
                return;
            clearSelectStatus();
            render.isSelect = true;
            this.Invalidate();
            EventManager.RaiserEvent(Constant.PropSelectChange, render, null);
        }

        protected void item_MouseWheel(object sender, MouseEventArgs e)
        {
            Point point = getFixPoint(e.X, e.Y);
            zoomChange(e.Delta, point);
        }

        protected void item_doubleClick(object sender, MouseEventArgs e)
        {
            Point pos = getFixPoint(e.X, e.Y);
            RenderBase clickItem = getClickItem(pos, out clickPosArena);
            if (clickItem == null)
            {
                return;
            }
            if (clickItem is TDPage)
            {
                EventManager.RaiserEvent(Constant.PropSelectChange, (clickItem as TDPage).renderItem, null);
            }
            else if (clickItem is TDPanel)
            {
                EventManager.RaiserEvent(Constant.OpenLayoutEvent, this, (clickItem as TDPanel).panelPath);
            }
        }

        protected void item_SizeChanged(object sender, EventArgs e)
        {
            DefaultOffsetPos.X = (this.Size.Width - _sceneSize.Width) / 2;
            DefaultOffsetPos.Y = (this.Size.Height - _sceneSize.Height) / 2;
            Refresh();
        }

        protected override void item_MouseLeave(object sender, EventArgs e)
        {
            mouseStatus = MouseOpStatus.StatusNode;
            Console.WriteLine("leave");
        }

        public void zoomChange(int delta, Point point) {
            _renderScale += delta > 0 ? 0.1f : -0.1f;
            _renderScale = Math.Max(Math.Min(_renderScale, 4.9f), 0.2f);
            //float stepX = this.sceneSize.Width * 0.1f / 2;
            //float stepY = this.sceneSize.Height * 0.1f / 2;
            float stepX = _renderScale * 10;
            float stepY = _renderScale * 10;
            if (Math.Abs(_renderCenterPos.X - point.X) < stepX)
            {
                _renderCenterPos.X = point.X;
            }
            else
            {
                _renderCenterPos.X += point.X > renderCenterPos.X ? stepX : -stepX;
            }
            if (Math.Abs(_renderCenterPos.Y - point.Y) < stepY)
            {
                _renderCenterPos.Y = point.Y;
            }
            else
            {
                _renderCenterPos.Y += point.Y > renderCenterPos.Y ? stepY : -stepY;
            }
            _renderCenterPos.X = Math.Min(Math.Max(0, _renderCenterPos.X), this.sceneSize.Width);
            _renderCenterPos.Y = Math.Min(Math.Max(0, _renderCenterPos.Y), this.sceneSize.Height);
            this.renderDock.resetScaleValue();
            Refresh();
        }
        protected void itemChange(object sender, object e)
        {
            if (this.Focused)
            {
                return;
            }
            Invalidate();
            Console.WriteLine("leave");
        }

        public void checkModifyStatus()
        {
            if (this.panelPath.Length > 0)
            {
                this.renderDock.TabText = this.panelPath + (isModify ? "*" : "");
            }
            else
            {
                String tabText = this.renderDock.TabText;
                if (tabText.EndsWith("*"))
                {
                    tabText = tabText.Substring(0, tabText.Length - 1);
                }
                this.renderDock.TabText = tabText + (isModify ? "*" : "");

            }
        }

        protected override bool IsInputKey(Keys key)
        {
            switch (key)
            {
                case Keys.Up:
                case Keys.Down:
                case Keys.Right:
                case Keys.Left:
                    return true;
            }
            return base.IsInputKey(key);
        }
 

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            Console.WriteLine("OnKeyPress");
        }
        
        private void item_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if ( (e.KeyCode == Keys.Left || e.KeyCode == Keys.Right || e.KeyCode == Keys.Up || e.KeyCode == Keys.Down) && e.Modifiers == Keys.Shift)
            {
                List<RenderBase> selectItems = getSelectItems();
                foreach (RenderBase item in selectItems)
                {
                    item.ProcessKeyEvent(e);
                }
                Invalidate();
            }
            //else if (e.KeyCode == Keys.C && e.Modifiers == Keys.Control)
            //{
            //    CopySelectItem();
            //}
            //else if (e.KeyCode == Keys.V && e.Modifiers == Keys.Control)
            //{
            //    PasteItemByClip();
            //}
            else if (e.KeyCode == Keys.R && e.Modifiers == Keys.Control)
            {
                UIHelper.loadViewToScene("E:/1.xml", this);
            }
            else if (e.KeyCode == Keys.F5 && e.Modifiers == Keys.Control)
            {
                UIHelper.loadViewToScene(this.panelPath, this);
            }
            else if (e.KeyCode == Keys.Delete)
            {
                List<RenderBase> items = getSelectItems();
                if (items.Count > 0)
                {
                    commandManager.AddCommand(new CommandDelList(this, items));
                    checkModifyStatus();
                    foreach (RenderBase render in items)
                    {
                        render.removeFromParent();
                    }
                    Invalidate();
                    EventManager.RaiserEvent(Constant.RenderItemChange, this, null);
                }
                
                
                
            }
        }

        private void item_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left || e.KeyCode == Keys.Right || e.KeyCode == Keys.Up || e.KeyCode == Keys.Down)
            {
                List<RenderBase> selectItems = getSelectItems();
                foreach (RenderBase item in selectItems)
                {
                    item.ProcessKeyEvent(e);
                }
                Invalidate();
            }
        }

        private void item_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            Console.WriteLine("item_KeyPress");
        }

        public void saveLayoutToXml(String path)
        {
            this.SetScenePath(path);
            FileHelper.SaveFileContent(path, UIHelper.generateXmlByItem(this).ToString());

            commandManager.Save();
            checkModifyStatus();
        }
        public void SetScenePath(String path)
        {
            this.panelPath = UIProject.Instance().GetRelativePath(path);
            this.renderDock.TabText = FileHelper.GetFileName(this.panelPath);
        }

        private void RenderScene_Load(object sender, EventArgs e)
        {
            this.Name = Constant.TypeScene;
        }

        public void recordOriStatus(List<RenderBase> items)
        {
            foreach (RenderBase render in items)
            {
                render.recordOriStatus();
            }
        }

        public void CopySelectItem()
        {
            List<RenderBase> selectItems = getSelectItems();
            if (selectItems.Count == 0)
            {
                return;
            }
            XElement xml = new XElement("Copy");
            foreach (RenderBase render in selectItems)
            {
                xml.Add(UIHelper.generateXmlByItem(render));
            }
            Clipboard.SetData(DataFormats.Text, xml.ToString());//复制内容到剪切板
        }

        public void PasteItemByClip()
        {
            String content = Clipboard.GetData(DataFormats.Text) as String;
            if (content == null)
            {
                return;
            }
            XElement xml = XmlHelper.Parse(content);
            if (xml == null || xml.Name != "Copy")
            {
                return;
            }
            clearSelectStatus();
            foreach (XElement node in xml.Elements("Node"))
            {
                RenderBase render = UIHelper.CEGenerateViewByXml(node, this);
                render.pos = new PointF(render.pos.X + 5, render.pos.Y + 5);
                render.isSelect = true;
            }
            Invalidate();
        }

        public void commandUndo()
        {
            CommandBase command = commandManager.Undo();
            if (command == null)
                return;
            if (command is CommandDelList)
                EventManager.RaiserEvent(Constant.RenderItemChange, this, null);
            checkModifyStatus();
            Invalidate();
        }

        public void commandRedo()
        {
            CommandBase command = commandManager.Redo();
            if (command == null)
                return;
            if (command is CommandDelList)
                EventManager.RaiserEvent(Constant.RenderItemChange, this, null);
            checkModifyStatus();
            Invalidate();
        }

        public void changePosByUniQue(String srcUni, String destUni)
        {
            RenderBase src = getRenderByUniqueName(srcUni);
            RenderBase dest = getRenderByUniqueName(destUni);
            if (src == null || dest == null || src is RenderScene || src == dest)
            {
                return;
            }

            TDPanel parent = dest.getParent() as TDPanel;
            if (parent == null)
            {
                return;
            }
            List<RenderBase> childs = new List<RenderBase>();
            foreach (RenderBase child in parent.ChildItems)
            {
                if (child == dest)
                {
                    childs.Add(child);
                    childs.Add(src);
                }
                else if (child != src)
                    childs.Add(child);
            }
            if (src.getParent() != dest.getParent())
            {
                src.removeFromParent();
                src.setParent(dest.getParent());
            }
            parent.ChildItems = childs;
            Invalidate();

        }

        public override void setAttrToXml(ref XElement xml)
        {
            base.setAttrToXml(ref xml);
            xml.SetAttributeValue("Width", this._sceneSize.Width.ToString(Constant.DefaultSingleIntFormat));
            xml.SetAttributeValue("Height", this._sceneSize.Height.ToString(Constant.DefaultSingleIntFormat));
        }


        public override void getAttrByXml(XElement xml)
        {
            base.getAttrByXml(xml);
            this._sceneSize.Width = XmlHelper.GetInt(xml, "Width");
            this._sceneSize.Height = XmlHelper.GetInt(xml, "Height");
            EventManager.RaiserEvent(Constant.RenderItemChange, this, null);
        }
    }
}
