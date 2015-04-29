using TDEditor.Utils;
using TDEditor.project;
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
using TickDream.WinUI.Docking;
using TDEditor.Widgets;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using ICSharpCode.AvalonEdit.Highlighting;
using System.Windows.Forms.Integration;
using System.Xml.Linq;
using System.Xml;

namespace TDEditor.Dock
{
    public partial class RenderDock : DockContent
    {
        private NumericUpDown widthNumeric;
        private NumericUpDown heightNumeric;
        private ColorSlider sliderScaleScene;
        private NumericUpDown numericScaleScene;
        private TextEditor editorText;
        private RichTextBox richTextBox;
        private bool isInDesignMode = true;

        public RenderDock()
        {
            InitializeComponent();
            this.renderBase1.renderDock = this;
            this.Activated += new EventHandler(eventGotFocus);
            this.Deactivate += new EventHandler(eventLostFocus);
            //this.GotFocus += new EventHandler(eventGotFocus);
            //this.LostFocus += new EventHandler(eventLostFocus);
            initToolStrip();

            editorText = new TextEditor();
            editorText.ShowLineNumbers = true;
            editorText.FontSize = 14;
            editorText.WordWrap = true;
            editorText.SyntaxHighlighting = HighlightingManager.Instance.GetDefinitionByExtension(".xml");

            ElementHost elementHost = new ElementHost();
            elementHost.Dock = DockStyle.Fill;
            elementHost.Child = editorText;
            //elementHost.Visible = false;
            this.dockPanel1.Controls.Add(elementHost);

            //richTextBox = new RichTextBox();
            //richTextBox.Dock = DockStyle.Fill;
            //this.dockPanel1.Controls.Add(richTextBox);

            this.dockPanel1.Visible = false;
            this.renderBase1.Visible = true;

            this.KeyUp += new KeyEventHandler(this.item_KeyUp);

            this.DockHandler.CloseConfirmCallBack = CloseConfirmCallBack;
        }

        private bool CloseConfirmCallBack()
        {
            if (!this.renderBase1.isModify)
            {
                return true;
            }
            if (MessageBox.Show("当前还未保存，是否关闭", "不保存关闭", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
            {
                //if (SaveLayoutToXml())
                //{
                    return true;
                //}
            }
            return false;
        }

        private void item_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.S && e.Modifiers == Keys.Control)
            {
                if (!this.isInDesignMode)
                {
                    SaveTextToXml();
                }
            }
            
        }

        public void setSourceMode(String content = null)
        {
            if (!this.isInDesignMode)
            {
                return;
            }
            //if (content == null)
            //    content = UIHelper.generateSrcByItem(this.renderBase1);
            //XmlDocument xmlDoc = new XmlDocument();//创建一个XML文档对象  
            //xmlDoc.LoadXml(content);//加载XML文档  
            //StringWriter tw = new StringWriter();//定义一个StringWriter  
            //XmlTextWriter tw2 = new XmlTextWriter(tw);//创建一个StringWriter实例的XmlTextWriter  
            //tw2.Formatting = Formatting.Indented;//设置缩进  
            //tw2.Indentation = 1;//设置缩进字数  
            //tw2.IndentChar = '\t';//用\t字符作为缩进  
            //xmlDoc.WriteTo(tw2);  

            if (content != null)
            {
                editorText.Text = content;
                //richTextBox.Text = xmlDoc.ToString();
            }
            else
            {
                editorText.Text = UIHelper.generateSrcByItem(this.renderBase1);
                //richTextBox.Text = UIHelper.generateSrcByItem(this.renderBase1);
            }
            this.isInDesignMode = false;
            this.dockPanel1.Visible = true;
            this.renderBase1.Visible = false;
        }

        public void setDesignMode()
        {
            if (this.isInDesignMode)
            {
                return;
            }
            if (!SetSceneByContent(editorText.Text))
            {
                return;
            }
            this.isInDesignMode = true;
            this.dockPanel1.Visible = false;
            this.renderBase1.Visible = true;
        }

        private void eventGotFocus(object sender, EventArgs e)
        {
            Console.WriteLine("eventGotFocus");
            EventManager.RaiserEvent(Constant.ActiveRenderChange, this, this.renderBase1);
        }

        private void eventLostFocus(object sender, EventArgs e)
        {
            Console.WriteLine("eventLostFocus");
            EventManager.RaiserEvent(Constant.ActiveRenderChange, this, null);
        }

        private void initToolStrip()
        {
            toolStrip1.Items.Add("宽:");
            widthNumeric = new NumericUpDown();
            widthNumeric.Maximum = 2048;
            widthNumeric.ValueChanged += new EventHandler(WidthValueChanged);

            toolStrip1.Items.Add(new ToolStripControlHost(widthNumeric));
            toolStrip1.Items.Add("高:");
            heightNumeric = new NumericUpDown();
            heightNumeric.Maximum = 2048;
            heightNumeric.ValueChanged += new EventHandler(HeightValueChanged);
            toolStrip1.Items.Add(new ToolStripControlHost(heightNumeric));
            toolStrip1.Items.Add(new ToolStripSeparator());
            sliderScaleScene = new ColorSlider();
            sliderScaleScene.Maximum = 500;
            sliderScaleScene.Minimum = 1;
            toolStrip1.Items.Add(new ToolStripControlHost(sliderScaleScene));
            sliderScaleScene.ValueChanged += new EventHandler(SlidersValueChanged);
            numericScaleScene = new NumericUpDown();
            numericScaleScene.Maximum = 500;
            numericScaleScene.Minimum = 1;
            toolStrip1.Items.Add(new ToolStripControlHost(numericScaleScene));
            numericScaleScene.ValueChanged += new EventHandler(NumericScaleSceneChanged);
            SetToolStripInfoByScene();
        }

        public void SetToolStripInfoByScene()
        {
            widthNumeric.Value = this.renderBase1.sceneSize.Width;
            heightNumeric.Value = this.renderBase1.sceneSize.Height;
            resetScaleValue();
        }

        private void WidthValueChanged(object sender, EventArgs e)
        {
            this.renderBase1.sceneSize = new Size( (int)(sender as NumericUpDown).Value, this.renderBase1.sceneSize.Height);
        }

        private void HeightValueChanged(object sender, EventArgs e)
        {
            this.renderBase1.sceneSize = new Size(this.renderBase1.sceneSize.Width, (int)(sender as NumericUpDown).Value);
        }

        private void NumericScaleSceneChanged(object sender, EventArgs e)
        {
            sliderScaleScene.Value = (int)(sender as NumericUpDown).Value;
            this.renderBase1.renderScale = (float)numericScaleScene.Value / 100;
        }

        private void SlidersValueChanged(object sender, EventArgs e)
        {
            numericScaleScene.Value = (sender as ColorSlider).Value;
            this.renderBase1.renderScale = (float)numericScaleScene.Value / 100;
            String Text = String.Format("{0}, ValueChange: {1}\r\n", (sender as ColorSlider).Name, (sender as ColorSlider).Value);
            Console.WriteLine(Text);
        }

        public void resetScaleValue()
        {
            sliderScaleScene.Value = (int)Math.Round(100.0 * this.renderBase1.renderScale);
            numericScaleScene.Value = (int)Math.Round(100.0 * this.renderBase1.renderScale);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            Update();
        }


        private void RenderDock_Load(object sender, EventArgs e)
        {
            
        }

        protected override string GetPersistString()
        {
            // Add extra information into the persist string for this document
            // so that it is available when deserialized.
            return GetType().ToString() + "," + this.TabText + "," + this.renderBase1.panelPath;
        }

        public RenderScene getScene()
        {
            return this.renderBase1;
        }

        public ToolStrip getToolBar()
        {
            return this.toolStrip1;
        }

        public void SetScenePath(String path)
        {
            this.renderBase1.SetScenePath(path);
            this.Text = "Name:" + this.renderBase1.panelPath;
            this.TabText = this.renderBase1.panelPath;
            String absPath = UIProject.Instance().GetRealFile(path);
            String content = FileHelper.GetFullContent(absPath);
            SetSceneByContent(content);
        }

        public bool SetSceneByContent(String content)
        {
            XElement xml = null;
            try
            {
                xml = XElement.Parse(content);
                if (xml == null)
                {
                    return false;
                }
                UIHelper.loadViewToScene(xml, this.renderBase1);
                SetToolStripInfoByScene();
                return true;
            }
            catch (XmlException e)
            {
                EventManager.RaiserEvent(Constant.StatusInfoChange, this, e.Message);
                setSourceMode(content);
            }
            return false;
        }

        public void SaveTextToXml()
        {
            String realPath = UIProject.Instance().GetRealFile(this.renderBase1.panelPath);
            if (File.Exists(realPath))
            {
                editorText.Save(realPath);
                return;
            }
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.FilterIndex = 1;
            dialog.Filter = "xml文件(*.xml)|*.xml";
            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            realPath = dialog.FileName;
            if (UIProject.Instance().FileInProject(realPath))
            {
                editorText.Save(realPath);
                EventManager.RaiserEvent(Constant.ResourceChange, this, null);
                return;
            }
            MessageBox.Show("请选择项目底下的文件");
        }

        public bool SaveLayoutToXml()
        {
            String realPath = UIProject.Instance().GetRealFile(this.renderBase1.panelPath);
            if (File.Exists(realPath))
            {
                this.renderBase1.saveLayoutToXml(realPath);
                return true;
            }
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.FilterIndex = 1;
            dialog.Filter = "xml文件(*.xml)|*.xml";
            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return false;
            }
            realPath = dialog.FileName;
            if (UIProject.Instance().FileInProject(realPath))
            {
                this.renderBase1.saveLayoutToXml(realPath);
                EventManager.RaiserEvent(Constant.ResourceChange, this, null);
                return true;
            }
            MessageBox.Show("请选择项目底下的文件");
            return false;
        }

        private void reset_Click(object sender, EventArgs e)
        {
            this.renderBase1.renderScale = 1;
            this.renderBase1.sceneSize = this.renderBase1.sceneSize;
            resetScaleValue();
        }


        private void design_Click(object sender, EventArgs e)
        {
            setDesignMode();
        }

        private void source_Click(object sender, EventArgs e)
        {
            setSourceMode();
        }
    }
}
