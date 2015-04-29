using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Forms;
using System.IO;
using TDEditor.Customization;
using TickDream.WinUI.Docking;
using TDEditor.Dock;
using Lextm.SharpSnmpLib;
using TDEditor.Utils;
using TDEditor.project;

namespace TDEditor
{
    public partial class MainForm : Form
    {
        private bool m_bSaveLayout = true;
        private DeserializeDockContent m_deserializeDockContent;
        private ObjectDock m_objectDock;
        private ResourceDock m_resourceDock;
        private PropDock m_propDock;
        private AnimateDock m_animateDock;
        private ImagePreViewDock m_imagePreViewDock;

        public MainForm()
        {
            InitializeComponent();

            CreateStandardControls();

            UIProject.Instance().openDefaultProject();

            DynamicObj.initDynamic();
            m_deserializeDockContent = new DeserializeDockContent(GetContentFromPersistString);
            
            vS2012ToolStripExtender1.DefaultRenderer = _system;
            vS2012ToolStripExtender1.VS2012Renderer = _custom;

            EventManager.RegisterAudience(Constant.OpenLayoutEvent, new EventHandler<object>(this.OpenRender));
            EventManager.RegisterAudience(Constant.StatusInfoChange, new EventHandler<object>(this.StatusInfoChange));
            EventManager.RegisterAudience(Constant.ProjectChange, new EventHandler<object>(this.ProjectChange));

        }

        #region Methods
        private RenderDock FindRenderByTabText(string text)
        {
            foreach (IDockContent content in dockPanel.Documents)
                if (content.DockHandler.TabText == text)
                    return content as RenderDock;

            return null;
        }

        private RenderDock FindRender(String panelPath)
        {
            foreach (IDockContent content in dockPanel.Documents)
            {
                RenderDock dock = content as RenderDock;
                if (dock != null && dock.getScene().panelPath == panelPath)
                {
                    return dock;
                }
            }
            return null;
        }

        private void StatusInfoChange(object sender, object e)
        {
            String text = e as String;
            if (text == null)
            {
                return;
            }
            this.tipInfo.Text = text;
        }

        private void ProjectChange(object sender, object e)
        {
            CloseAllDocuments();
        }

        private void OpenRender(object sender, object e)
        {
            String path = e as String;
            if (path == null)
            {
                return;
            }
            String panelPath = UIProject.Instance().GetRelativePath(path);
            RenderDock dock = FindRender(panelPath);
            if (dock != null)
            {
                dock.Show();
                return;
            }
            RenderDock renderDoc = new RenderDock();
            renderDoc.SetScenePath(path);
            renderDoc.Show(dockPanel);
        }

        private RenderDock CreateNewRender()
        {
            RenderDock renderDoc = new RenderDock();

            int count = 1;
            string text = "Document" + count.ToString();
            while (FindRenderByTabText(text) != null)
            {
                count++;
                text = "Document" + count.ToString();
            }
            renderDoc.Text = renderDoc.TabText = text;
            return renderDoc;
        }

        private RenderDock CreateNewRender(string text)
        {
            RenderDock dummyDoc = new RenderDock();
            dummyDoc.Text = dummyDoc.TabText = text;
            return dummyDoc;
        }

        private RenderDock getActiveRenderDock()
        {
            foreach (IDockContent document in dockPanel.DocumentsToArray())
            {
                if (document.DockHandler.IsActivated)
                {
                    if (document is RenderDock)
                    {
                        return document as RenderDock;
                    }
                }
            }
            return null;
        }

        private void CloseAllDocuments()
        {
            foreach (IDockContent document in dockPanel.DocumentsToArray())
            {
                document.DockHandler.Close();
            }
        }

        private IDockContent GetContentFromPersistString(string persistString)
        {
            if (persistString == typeof(ObjectDock).ToString())
                return m_objectDock;
            else if (persistString == typeof(ResourceDock).ToString())
                return m_resourceDock;
            else if (persistString == typeof(PropDock).ToString())
                return m_propDock;
            else if (persistString == typeof(ImagePreViewDock).ToString())
                return m_imagePreViewDock;
            else if (persistString == typeof(AnimateDock).ToString())
                return m_animateDock;
            else
            {
                // DummyDoc overrides GetPersistString to add extra information into persistString.
                // Any DockContent may override this value to add any needed information for deserialization.

                string[] parsedStrings = persistString.Split(new char[] { ',' });
                if (parsedStrings.Length != 3)
                    return null;

                if (parsedStrings[0] != typeof(RenderDock).ToString() || parsedStrings[2] == string.Empty)
                    return null;
                if (!File.Exists(UIProject.Instance().GetRealFile(parsedStrings[2])))
                    return null;
                RenderDock dummyDoc = FindRenderByTabText(parsedStrings[1]);
                if (dummyDoc != null)
                {
                    return null;
                }
                dummyDoc = new RenderDock();
                dummyDoc.TabText = parsedStrings[1];
                dummyDoc.SetScenePath(parsedStrings[2]);
                return dummyDoc;
            }
        }

        private void CloseAllContents()
        {
            m_objectDock.DockPanel = null;
            m_propDock.DockPanel = null;
            m_resourceDock.DockPanel = null;
            m_imagePreViewDock.DockPanel = null;
            m_animateDock.DockPanel = null;
            CloseAllDocuments();
        }

        private readonly ToolStripRenderer _system = new ToolStripProfessionalRenderer();
        private readonly ToolStripRenderer _custom = new VS2012ToolStripRenderer();
        
        private void EnableVS2012Renderer(bool enable)
        {
            vS2012ToolStripExtender1.SetEnableVS2012Style(this.mainMenu, enable);
            vS2012ToolStripExtender1.SetEnableVS2012Style(this.toolBar, enable);
        }

        private void SetDocumentStyle(object sender, System.EventArgs e)
        {
            DocumentStyle oldStyle = dockPanel.DocumentStyle;
            DocumentStyle newStyle = DocumentStyle.SystemMdi;

            if (oldStyle == newStyle)
                return;

            if (oldStyle == DocumentStyle.SystemMdi || newStyle == DocumentStyle.SystemMdi)
                CloseAllDocuments();

            dockPanel.DocumentStyle = newStyle;
            menuItemLayoutByCode.Enabled = (newStyle != DocumentStyle.SystemMdi);
            toolBarButtonLayoutByCode.Enabled = (newStyle != DocumentStyle.SystemMdi);
        }

        private AutoHideStripSkin _autoHideStripSkin;
        private DockPaneStripSkin _dockPaneStripSkin;

        private void SetDockPanelSkinOptions(bool isChecked)
        {
            if (isChecked)
            {
                // All of these options may be set in the designer.
                // This is not a complete list of possible options available in the skin.

                AutoHideStripSkin autoHideSkin = new AutoHideStripSkin();
                autoHideSkin.DockStripGradient.StartColor = Color.AliceBlue;
                autoHideSkin.DockStripGradient.EndColor = Color.Blue;
                autoHideSkin.DockStripGradient.LinearGradientMode = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal;
                autoHideSkin.TabGradient.StartColor = SystemColors.Control;
                autoHideSkin.TabGradient.EndColor = SystemColors.ControlDark;
                autoHideSkin.TabGradient.TextColor = SystemColors.ControlText;
                autoHideSkin.TextFont = new Font("Showcard Gothic", 10);

                _autoHideStripSkin = dockPanel.Skin.AutoHideStripSkin;
                dockPanel.Skin.AutoHideStripSkin = autoHideSkin;

                DockPaneStripSkin dockPaneSkin = new DockPaneStripSkin();
                dockPaneSkin.DocumentGradient.DockStripGradient.StartColor = Color.Red;
                dockPaneSkin.DocumentGradient.DockStripGradient.EndColor = Color.Pink;

                dockPaneSkin.DocumentGradient.ActiveTabGradient.StartColor = Color.Green;
                dockPaneSkin.DocumentGradient.ActiveTabGradient.EndColor = Color.Green;
                dockPaneSkin.DocumentGradient.ActiveTabGradient.TextColor = Color.White;

                dockPaneSkin.DocumentGradient.InactiveTabGradient.StartColor = Color.Gray;
                dockPaneSkin.DocumentGradient.InactiveTabGradient.EndColor = Color.Gray;
                dockPaneSkin.DocumentGradient.InactiveTabGradient.TextColor = Color.Black;

                dockPaneSkin.TextFont = new Font("SketchFlow Print", 10);

                _dockPaneStripSkin = dockPanel.Skin.DockPaneStripSkin;
                dockPanel.Skin.DockPaneStripSkin = dockPaneSkin;
            }
            else
            {
                if (_autoHideStripSkin != null)
                {
                    dockPanel.Skin.AutoHideStripSkin = _autoHideStripSkin;
                }

                if (_dockPaneStripSkin != null)
                {
                    dockPanel.Skin.DockPaneStripSkin = _dockPaneStripSkin;
                }
            }

        }

        #endregion

        #region Event Handlers

        private void menuItemExit_Click(object sender, System.EventArgs e)
        {
            Close();
        }

        private void menuItemAbout_Click(object sender, System.EventArgs e)
        {
            AboutDialog aboutDialog = new AboutDialog();
            aboutDialog.ShowDialog(this);
        }

        private void menuItemNew_Click(object sender, System.EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.Description = "请选择文件路径";
            DialogResult result = folderBrowserDialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                UIProject.Instance().openProjectPath(folderBrowserDialog.SelectedPath);
            }
        }
        private void objectDockToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_objectDock.Show(dockPanel);
        }

        private void resourceDockToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_resourceDock.Show(dockPanel);
        }

        private void propDockToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_propDock.Show(dockPanel);
        }

        private void PreViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_imagePreViewDock.Show(dockPanel);
        }

        private void menuItemOpen_Click(object sender, System.EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            DialogResult result = folderBrowserDialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                String configFile = folderBrowserDialog.SelectedPath + "/.project";
                if (!File.Exists(configFile))
                {
                    MessageBox.Show("该项目不存在");
                    return;
                }
                UIProject.Instance().openProjectPath(folderBrowserDialog.SelectedPath);
            }
        }

        private void menuItemFile_Popup(object sender, System.EventArgs e)
        {
            if (dockPanel.DocumentStyle == DocumentStyle.SystemMdi)
            {
                menuItemClose.Enabled = 
                    menuItemCloseAll.Enabled =
                    menuItemSave.Enabled = (ActiveMdiChild != null);
            }
            else
            {
                menuItemClose.Enabled = (dockPanel.ActiveDocument != null);
                menuItemCloseAll.Enabled =
                    menuItemSave.Enabled = (dockPanel.DocumentsCount > 0);
            }
        }

        private void menuItemClose_Click(object sender, System.EventArgs e)
        {
            if (dockPanel.DocumentStyle == DocumentStyle.SystemMdi)
                ActiveMdiChild.Close();
            else if (dockPanel.ActiveDocument != null)
                dockPanel.ActiveDocument.DockHandler.Close();
        }

        private void menuItemCloseAll_Click(object sender, System.EventArgs e)
        {
            CloseAllDocuments();
        }

        private void MainForm_Load(object sender, System.EventArgs e)
        {
            string configFile = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "DockPanel.config");
            if (File.Exists(configFile) && new FileInfo(configFile).Length > 128)
            {
                dockPanel.LoadFromXml(configFile, m_deserializeDockContent);
            }
            else
            {
                menuItemLayoutByCode_Click(null, null);
            }
                
        }

        private void MainForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string configFile = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "DockPanel.config");
            if (m_bSaveLayout)
                dockPanel.SaveAsXml(configFile);
            //else if (File.Exists(configFile))
            //    File.Delete(configFile);
        }

        private void toolBarButtonNew_Click(object sender, EventArgs e)
        {
            RenderDock dummyDoc = CreateNewRender();
            if (dockPanel.DocumentStyle == DocumentStyle.SystemMdi)
            {
                dummyDoc.MdiParent = this;
                dummyDoc.Show();
            }
            else
                dummyDoc.Show(dockPanel);
        }

        private void menuItemToolBar_Click(object sender, System.EventArgs e)
        {
            toolBar.Visible = menuItemToolBar.Checked = !menuItemToolBar.Checked;
        }

        private void menuItemStatusBar_Click(object sender, System.EventArgs e)
        {
            statusBar.Visible = menuItemStatusBar.Checked = !menuItemStatusBar.Checked;
        }

        private void toolBar_ButtonClick(object sender, System.Windows.Forms.ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem == toolBarButtonLayoutByCode)
                menuItemLayoutByCode_Click(null, null);
            else if (e.ClickedItem == toolBarButtonSave)
                menuItemSave_click(null, null);
            else if (e.ClickedItem == toolBarButtonCopy)
                menuItemCopy_Click(null, null);
            else if (e.ClickedItem == toolBarButtonPaste)
                menuItemPaste_Click(null, null);
        }

        private void menuItemNewWindow_Click(object sender, System.EventArgs e)
        {
            MainForm newWindow = new MainForm();
            newWindow.Text = newWindow.Text + " - New";
            newWindow.Show();
        }

        private void menuItemLockLayout_Click(object sender, System.EventArgs e)
        {
            dockPanel.AllowEndUserDocking = !dockPanel.AllowEndUserDocking;
        }

        private void menuItemLayoutByCode_Click(object sender, System.EventArgs e)
        {
            dockPanel.SuspendLayout(true);

            CloseAllContents();

            CreateStandardControls();
            m_resourceDock.reloadTree();

            m_objectDock.Show(dockPanel, DockState.DockLeft);
            m_resourceDock.Show(m_objectDock.Pane, DockAlignment.Bottom, 0.6);
            m_animateDock.Show(m_resourceDock.Pane, DockAlignment.Bottom, 0.4);

            //m_imagePreViewDock.Show(m_resourceDock.Pane, DockAlignment.Bottom, 0.4);

            //m_imagePreViewDock.Show(m_resourceDock.Pane, DockAlignment.Bottom, 0.4);
            //m_animateDock.Show(dockPanel, DockState.DockBottom);

            m_propDock.Show(dockPanel, DockState.DockRight);
            m_imagePreViewDock.Show(m_propDock.Pane, DockAlignment.Bottom, 0.3);

            dockPanel.ResumeLayout(true, true);
        }

        private void CreateStandardControls()
        {
            m_objectDock = new ObjectDock();
            m_propDock = new PropDock();
            m_resourceDock = new ResourceDock();
            m_imagePreViewDock = new ImagePreViewDock();
            m_animateDock = new AnimateDock();

            DynamicObj.objectDock = m_objectDock;
            DynamicObj.propDock = m_propDock;
            DynamicObj.resourceDock = m_resourceDock;
            DynamicObj.imagePreViewDock = m_imagePreViewDock;
            DynamicObj.animateDock = m_animateDock;
        }

        private void menuItemLayoutByXml_Click(object sender, System.EventArgs e)
        {
            dockPanel.SuspendLayout(true);

            // In order to load layout from XML, we need to close all the DockContents
            CloseAllContents();

            CreateStandardControls();
            m_resourceDock.reloadTree();

            Assembly assembly = Assembly.GetAssembly(typeof(MainForm));
            Stream xmlStream = assembly.GetManifestResourceStream("TDEditor.Resources.DockPanel.xml");
            dockPanel.LoadFromXml(xmlStream, m_deserializeDockContent);
            xmlStream.Close();

            dockPanel.ResumeLayout(true, true);
        }

        private void menuItemCopy_Click(object sender, EventArgs e)
        {
            RenderDock activeDock = getActiveRenderDock();
            if (activeDock != null)
            {
                activeDock.getScene().CopySelectItem();
            }
        }

        private void menuItemPaste_Click(object sender, EventArgs e)
        {
            RenderDock activeDock = getActiveRenderDock();
            if (activeDock != null)
            {
                activeDock.getScene().PasteItemByClip();
            }
        }

        private void menuItemSave_click(object sender, System.EventArgs e)
        {
            RenderDock activeDock = getActiveRenderDock();
            if (activeDock != null)
            {
                activeDock.SaveLayoutToXml();
            }
        }

        private void menuItemSaveAll_Click(object sender, EventArgs e)
        {
            foreach (IDockContent document in dockPanel.DocumentsToArray())
            {
                if (document is RenderDock)
                {
                    (document as RenderDock).SaveLayoutToXml();
                }
            }
        }

        private void exitWithoutSavingLayout_Click(object sender, EventArgs e)
        {
            m_bSaveLayout = false;
            Close();
            m_bSaveLayout = true;
        }
        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RenderDock activeDock = getActiveRenderDock();
            if (activeDock != null)
            {
                activeDock.getScene().commandUndo();
            }
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RenderDock activeDock = getActiveRenderDock();
            if (activeDock != null)
            {
                activeDock.getScene().commandRedo();
            }
        }
        private void lockLayoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dockPanel.AllowEndUserDocking = !dockPanel.AllowEndUserDocking;
            lockLayoutToolStripMenuItem.Checked = !dockPanel.AllowEndUserDocking;
        }

        #endregion







    }
}