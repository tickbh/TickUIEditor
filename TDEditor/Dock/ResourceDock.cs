using TDEditor.Utils;
using TDEditor.project;
using TDEditor.Widgets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TDEditor.Data;
using System.Diagnostics;

namespace TDEditor.Dock
{
    public partial class ResourceDock : ToolWindow
    {
        const int ImageIndexProject = 1;
        const int ImageIndexFloder = 2;
        const int ImageIndexFile = 3;
        const int ImageIndexPng = 4;
        const int ImageIndexXml = 5;

        enum FileType
        {
            Floder,
            File,
            FileNone,
        }

        public ResourceDock()
        {
            InitializeComponent();

            this.treeView1.NodeMouseHover += new TreeNodeMouseHoverEventHandler(this.TreeView1_NodeMouseHover);
            this.treeView1.NodeMouseDoubleClick += new TreeNodeMouseClickEventHandler(this.TreeView1_MouseDoubleClick);
            this.treeView1.NodeMouseClick += new TreeNodeMouseClickEventHandler(this.TreeView1_MouseClick);

            this.treeView1.ItemDrag += new ItemDragEventHandler(this.treeView1_ItemDrag);
            this.treeView1.AllowDrop = true;

            this.treeView1.DragDrop += new DragEventHandler(treeView1_DragDrop);
            this.treeView1.DragEnter += new DragEventHandler(treeView1_DragEnter);

            EventManager.RegisterAudience(Constant.ResourceChange, new EventHandler<object>(this.resourceChange));
            EventManager.RegisterAudience(Constant.ProjectChange, new EventHandler<object>(this.projectChange));
            
            this.treeView1.KeyUp += new KeyEventHandler(this.item_KeyUp);
        }

        private void treeView1_ItemDrag(object sender, ItemDragEventArgs e)
        {
            TreeNode node = e.Item as TreeNode;
            if (node != null)
            {
                String tag = node.Tag as String;
                if (ImageHelper.isImageFile(tag))
                {
                    DoDragDrop(new ControlDDData(Constant.TypeSprite, node.Tag, e.Item, true), DragDropEffects.All);
                }
                else if (tag.EndsWith(".xml"))
                {
                    DoDragDrop(new ControlDDData(Constant.TypePanel, node.Tag, e.Item, false), DragDropEffects.All);
                }
                else
                {
                    DoDragDrop(new ControlDDData(null, null, e.Item), DragDropEffects.All);
                }
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

            EventManager.RaiserEvent(Constant.PreViewImageChange, this, null);


            FileHelper.CutPathToAnother(myNode.Tag as String, DropNode.Tag as String);
            reloadTree();
        }

        private void MoveItemToOther(String srcPath, String destPath)
        {

        }

        private FileType getNodeType(String path)
        {
            if (path == null)
            {
                return FileType.FileNone;
            }

            if (FileHelper.IsFilePath(path))
            {
                return FileType.File;
            }
            else if (FileHelper.IsFloderPath(path))
            {
                return FileType.Floder;
            }

            return FileType.FileNone;
        }



        public void reloadTree()
        {
            try
            {
                this.treeView1.Nodes.Clear();
                TreeNode node = new TreeNode(UIProject.Instance().projectName);
                node.SelectedImageIndex = node.ImageIndex = ImageIndexProject;
                this.treeView1.Nodes.Add(node);
                GetFiles(UIProject.Instance().getProjectPath(), node);
                node.Expand();
            }
            catch { }
        }

        private void GetFiles(string filePath, TreeNode node)
        {
            DirectoryInfo folder = new DirectoryInfo(filePath);
            node.Text = folder.Name;
            node.Tag = folder.FullName;

            FileInfo[] chldFiles = folder.GetFiles("*.*");
            foreach (FileInfo chlFile in chldFiles)
            {
                if (chlFile.Name.StartsWith("."))
                {
                    continue;
                }
                TreeNode chldNode = new TreeNode();
                chldNode.Text = chlFile.Name;
                chldNode.Tag = chlFile.FullName;
                if (ImageHelper.isImageFile(chlFile.Name))
                {
                    chldNode.SelectedImageIndex = chldNode.ImageIndex = ImageIndexPng;
                }
                else
                {
                    chldNode.SelectedImageIndex = chldNode.ImageIndex = ImageIndexFile;
                }
                node.Nodes.Add(chldNode);
            }

            DirectoryInfo[] chldFolders = folder.GetDirectories();
            foreach (DirectoryInfo chldFolder in chldFolders)
            {
                TreeNode chldNode = new TreeNode();
                chldNode.Text = folder.Name;
                chldNode.Tag = folder.FullName;
                chldNode.SelectedImageIndex = chldNode.ImageIndex = ImageIndexFloder;
                node.Nodes.Add(chldNode);
                GetFiles(chldFolder.FullName, chldNode);
            }
        }

        private void item_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                reloadTree();
            }
        }

        private void TreeView1_NodeMouseHover(Object sender, TreeNodeMouseHoverEventArgs e)
        {
            String tag = e.Node.Tag as String;
            if(tag == null) {
                return;
            }
            if (ImageHelper.isImageFile(tag))
            {
                EventManager.RaiserEvent(Constant.PreViewImageChange, this, tag);
            }
        }

        private void openInFloder(object sender, EventArgs e)
        {
            if (!(sender is ToolStripMenuItem))
            {
                return;
            }
            ToolStripMenuItem item = sender as ToolStripMenuItem;
            TreeNode node = item.Tag as TreeNode;
            if (node == null)
            {
                return;
            }
            Process open = new Process();
            open.StartInfo.FileName = "explorer";
            open.StartInfo.Arguments = @"/select," + node.Tag;
            open.Start();
        }

        private void TreeView1_MouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ToolStripMenuItem[] formMenuItemList = new ToolStripMenuItem[1];
                formMenuItemList[0] = new ToolStripMenuItem("打开所在文件夹", null, new EventHandler(this.openInFloder));
                formMenuItemList[0].Tag = e.Node;
                ContextMenuStrip formMenu = new ContextMenuStrip();
                formMenu.Items.AddRange(formMenuItemList);
                this.ContextMenuStrip = formMenu;
            }   
        }

        private void TreeView1_MouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            String tag = e.Node.Tag as String;
            if(tag == null) {
                return;
            }
            if(tag.EndsWith(".xml")) {
                EventManager.RaiserEvent(Constant.OpenLayoutEvent, this, tag);
            }
        }
        
        protected void resourceChange(object sender, object e)
        {
            reloadTree();
        }

        protected void projectChange(object sender, object e)
        {
            reloadTree();
        }
    }
}
