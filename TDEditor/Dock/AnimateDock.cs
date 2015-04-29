using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TDEditor.Widgets;
using TDEditor.Utils;
using TDEditor.Data;

namespace TDEditor.Dock
{
    public partial class AnimateDock : ToolWindow
    {
        private RenderDock actionDock = null;
        private RenderScene renderScene = null;
        
        const int ImageIndexProject = 1;
        const int ImageIndexFloder = 2;
        const int ImageIndexFile = 3;
        const int ImageIndexPng = 4;
        const int ImageIndexXml = 5;

        public AnimateDock()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.treeView1.NodeMouseClick += new TreeNodeMouseClickEventHandler(this.TreeView1_MouseClick);

            this.treeView1.ItemDrag += new ItemDragEventHandler(this.treeView1_ItemDrag);
            this.treeView1.AllowDrop = true;
            this.treeView1.DragDrop += new DragEventHandler(treeView1_DragDrop);
            this.treeView1.DragEnter += new DragEventHandler(treeView1_DragEnter);
            this.treeView1.ShowLines = true;
            EventManager.RegisterAudience(Constant.ActiveRenderChange, new EventHandler<object>(this.activeRenderChange));
            EventManager.RegisterAudience(Constant.RenderItemChange, new EventHandler<object>(this.renderItemChange));

            this.treeView1.KeyUp += new KeyEventHandler(this.item_KeyUp);
        }

        private void item_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                reloadSceneInfo();
            }
        }

        private void treeView1_ItemDrag(object sender, ItemDragEventArgs e)
        {
            TreeNode node = e.Item as TreeNode;
            if (node != null)
            {
                DoDragDrop(new ControlDDData(null, null, e.Item), DragDropEffects.All);
            }
        }

        private void treeView1_DragEnter(object sender, DragEventArgs e)
        {
            ControlDDData data = e.Data.GetData(typeof(ControlDDData)) as ControlDDData;
            if (data != null && data.extNode is TreeNode)
            {
                e.Effect = DragDropEffects.All;
            }
            else
                e.Effect = DragDropEffects.None;
        }

        private void treeView1_DragDrop(object sender, DragEventArgs e)
        {
            if (renderScene == null)
            {
                return;
            }
            TreeNode myNode = null;
            ControlDDData data = e.Data.GetData(typeof(ControlDDData)) as ControlDDData;
            if (data != null && data.extNode is TreeNode)
            {
                myNode = data.extNode as TreeNode;
            }
            else
                return;
            Point Position = Point.Empty;
            Position.X = e.X;
            Position.Y = e.Y;
            Position = treeView1.PointToClient(Position);
            TreeNode DropNode = this.treeView1.GetNodeAt(Position);
            if (DropNode == null || DropNode.Parent == myNode || DropNode == myNode)
            {
                return;
            }

            renderScene.changePosByUniQue(myNode.Tag as String, DropNode.Tag as String);
            reloadSceneInfo();
        }

        public void reloadSceneInfo() {
            this.treeView1.Nodes.Clear();
            if (renderScene == null)
            {
                return;
            }
            addNodeInfo(null, renderScene);
        }

        public void addNodeInfo(TreeNode node, RenderBase render)
        {
            TreeNode chldNode = new TreeNode();
            chldNode.Text = render.Name + (render.Tag.Length > 0 ? (":" + render.Tag) : (""));
            chldNode.Tag = render.uniqueName;
            chldNode.SelectedImageIndex = chldNode.ImageIndex = ImageIndexPng;

            if (render is RenderScene)
            {
                this.treeView1.Nodes.Add(chldNode);

                //List<RenderBase> childItems = (render as TDPanel).ChildItems;
                //for (int i = childItems.Count - 1; i >= 0; i-- )
                //{
                //    addNodeInfo(chldNode, childItems[i]);
                //}
                foreach (RenderBase item in (render as TDPanel).ChildItems)
                {
                    addNodeInfo(chldNode, item);
                }
                chldNode.Expand();
            }
            else if (render is TDPanel)
            {
                if ((render as TDPanel).panelPath.Length == 0)
                {
                    chldNode.SelectedImageIndex = chldNode.ImageIndex = ImageIndexPng;
                    //List<RenderBase> childItems = (render as TDPanel).ChildItems;
                    //for (int i = childItems.Count - 1; i >= 0; i--)
                    //{
                    //    addNodeInfo(chldNode, childItems[i]);
                    //}
                    foreach (RenderBase item in (render as TDPanel).ChildItems)
                    {
                        addNodeInfo(chldNode, item);
                    }
                }
                else
                {
                    chldNode.Text = chldNode.Text + ":" + (render as TDPanel).panelPath;
                }
                if (node != null)
                {
                    node.Nodes.Add(chldNode);
                }
            }
            else
            {
                chldNode.SelectedImageIndex = chldNode.ImageIndex = ImageIndexPng;
                if (node != null)
                {
                    node.Nodes.Add(chldNode);
                }
            }
        }

        protected void renderItemChange(object sender, object e)
        {
            reloadSceneInfo();
        }

        protected void activeRenderChange(object sender, object e)
        {
            RenderDock dock = sender as RenderDock;
            RenderScene scene = e as RenderScene;
            if (dock != null && scene != null)
            {
                this.actionDock = dock;
                this.renderScene = scene;
            }
            else if (scene == null && dock == this.actionDock)
            {
                this.actionDock = null;
                this.renderScene = null;
            }
            reloadSceneInfo();
        }

        private void TreeView1_MouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            String tag = e.Node.Tag as String;
            if (tag == null || renderScene == null)
                return;
            renderScene.selectItemByUniqueName(tag);

            if (e.Button == MouseButtons.Right)
            {
                //ToolStripMenuItem[] formMenuItemList = new ToolStripMenuItem[1];
                //formMenuItemList[0] = new ToolStripMenuItem("打开所在文件夹", null, new EventHandler(this.openInFloder));
                //formMenuItemList[0].Tag = e.Node;
                //ContextMenuStrip formMenu = new ContextMenuStrip();
                //formMenu.Items.AddRange(formMenuItemList);
                //this.ContextMenuStrip = formMenu;
            }
            else
            {

            }
        }
    }
}
