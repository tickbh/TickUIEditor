namespace TDEditor.Dock
{
    partial class RenderDock
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RenderDock));
            TickDream.WinUI.Docking.DockPanelSkin dockPanelSkin2 = new TickDream.WinUI.Docking.DockPanelSkin();
            TickDream.WinUI.Docking.AutoHideStripSkin autoHideStripSkin2 = new TickDream.WinUI.Docking.AutoHideStripSkin();
            TickDream.WinUI.Docking.DockPanelGradient dockPanelGradient4 = new TickDream.WinUI.Docking.DockPanelGradient();
            TickDream.WinUI.Docking.TabGradient tabGradient8 = new TickDream.WinUI.Docking.TabGradient();
            TickDream.WinUI.Docking.DockPaneStripSkin dockPaneStripSkin2 = new TickDream.WinUI.Docking.DockPaneStripSkin();
            TickDream.WinUI.Docking.DockPaneStripGradient dockPaneStripGradient2 = new TickDream.WinUI.Docking.DockPaneStripGradient();
            TickDream.WinUI.Docking.TabGradient tabGradient9 = new TickDream.WinUI.Docking.TabGradient();
            TickDream.WinUI.Docking.DockPanelGradient dockPanelGradient5 = new TickDream.WinUI.Docking.DockPanelGradient();
            TickDream.WinUI.Docking.TabGradient tabGradient10 = new TickDream.WinUI.Docking.TabGradient();
            TickDream.WinUI.Docking.DockPaneStripToolWindowGradient dockPaneStripToolWindowGradient2 = new TickDream.WinUI.Docking.DockPaneStripToolWindowGradient();
            TickDream.WinUI.Docking.TabGradient tabGradient11 = new TickDream.WinUI.Docking.TabGradient();
            TickDream.WinUI.Docking.TabGradient tabGradient12 = new TickDream.WinUI.Docking.TabGradient();
            TickDream.WinUI.Docking.DockPanelGradient dockPanelGradient6 = new TickDream.WinUI.Docking.DockPanelGradient();
            TickDream.WinUI.Docking.TabGradient tabGradient13 = new TickDream.WinUI.Docking.TabGradient();
            TickDream.WinUI.Docking.TabGradient tabGradient14 = new TickDream.WinUI.Docking.TabGradient();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.design = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.source = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.reset = new System.Windows.Forms.ToolStripButton();
            this.renderBase1 = new TDEditor.Widgets.RenderScene();
            this.vS2003Theme1 = new TickDream.WinUI.Docking.VS2003Theme();
            this.dockPanel1 = new TickDream.WinUI.Docking.DockPanel();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.design,
            this.toolStripSeparator1,
            this.source,
            this.toolStripSeparator2,
            this.reset});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(414, 27);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // design
            // 
            this.design.Image = ((System.Drawing.Image)(resources.GetObject("design.Image")));
            this.design.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.design.Name = "design";
            this.design.Size = new System.Drawing.Size(59, 24);
            this.design.Text = "设计";
            this.design.Click += new System.EventHandler(this.design_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
            // 
            // source
            // 
            this.source.Image = ((System.Drawing.Image)(resources.GetObject("source.Image")));
            this.source.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.source.Name = "source";
            this.source.Size = new System.Drawing.Size(59, 24);
            this.source.Text = "源码";
            this.source.Click += new System.EventHandler(this.source_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 27);
            // 
            // reset
            // 
            this.reset.Image = ((System.Drawing.Image)(resources.GetObject("reset.Image")));
            this.reset.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.reset.Name = "reset";
            this.reset.Size = new System.Drawing.Size(59, 24);
            this.reset.Text = "重设";
            this.reset.Click += new System.EventHandler(this.reset_Click);
            // 
            // renderBase1
            // 
            this.renderBase1.AllowDrop = true;
            this.renderBase1.anchorPos = ((System.Drawing.PointF)(resources.GetObject("renderBase1.anchorPos")));
            this.renderBase1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.renderBase1.Location = new System.Drawing.Point(0, 0);
            this.renderBase1.Name = "renderBase1";
            this.renderBase1.oriAnchorPos = ((System.Drawing.PointF)(resources.GetObject("renderBase1.oriAnchorPos")));
            this.renderBase1.oriPos = ((System.Drawing.PointF)(resources.GetObject("renderBase1.oriPos")));
            this.renderBase1.oriScale = ((System.Drawing.PointF)(resources.GetObject("renderBase1.oriScale")));
            this.renderBase1.oriSize = new System.Drawing.SizeF(0F, 0F);
            this.renderBase1.pos = ((System.Drawing.PointF)(resources.GetObject("renderBase1.pos")));
            this.renderBase1.renderCenterPos = ((System.Drawing.PointF)(resources.GetObject("renderBase1.renderCenterPos")));
            this.renderBase1.renderScale = 1F;
            this.renderBase1.rotation = 0F;
            this.renderBase1.scale = ((System.Drawing.PointF)(resources.GetObject("renderBase1.scale")));
            this.renderBase1.sceneSize = new System.Drawing.Size(800, 600);
            this.renderBase1.size = new System.Drawing.SizeF(100F, 100F);
            this.renderBase1.Size = new System.Drawing.Size(414, 324);
            this.renderBase1.TabIndex = 0;
            this.renderBase1.Tag = "";
            this.renderBase1.uniqueName = "Scene-ccec412c-ae25-4144-a04c-9a7e9fd60778";
            this.renderBase1.visible = true;
            // 
            // dockPanel1
            // 
            this.dockPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dockPanel1.Location = new System.Drawing.Point(0, 27);
            this.dockPanel1.Name = "dockPanel1";
            this.dockPanel1.Size = new System.Drawing.Size(414, 297);
            dockPanelGradient4.EndColor = System.Drawing.SystemColors.ControlLight;
            dockPanelGradient4.StartColor = System.Drawing.SystemColors.ControlLight;
            autoHideStripSkin2.DockStripGradient = dockPanelGradient4;
            tabGradient8.EndColor = System.Drawing.SystemColors.Control;
            tabGradient8.StartColor = System.Drawing.SystemColors.Control;
            tabGradient8.TextColor = System.Drawing.SystemColors.ControlDarkDark;
            autoHideStripSkin2.TabGradient = tabGradient8;
            autoHideStripSkin2.TextFont = new System.Drawing.Font("微软雅黑", 9F);
            dockPanelSkin2.AutoHideStripSkin = autoHideStripSkin2;
            tabGradient9.EndColor = System.Drawing.SystemColors.ControlLightLight;
            tabGradient9.StartColor = System.Drawing.SystemColors.ControlLightLight;
            tabGradient9.TextColor = System.Drawing.SystemColors.ControlText;
            dockPaneStripGradient2.ActiveTabGradient = tabGradient9;
            dockPanelGradient5.EndColor = System.Drawing.SystemColors.Control;
            dockPanelGradient5.StartColor = System.Drawing.SystemColors.Control;
            dockPaneStripGradient2.DockStripGradient = dockPanelGradient5;
            tabGradient10.EndColor = System.Drawing.SystemColors.ControlLight;
            tabGradient10.StartColor = System.Drawing.SystemColors.ControlLight;
            tabGradient10.TextColor = System.Drawing.SystemColors.ControlText;
            dockPaneStripGradient2.InactiveTabGradient = tabGradient10;
            dockPaneStripSkin2.DocumentGradient = dockPaneStripGradient2;
            dockPaneStripSkin2.TextFont = new System.Drawing.Font("微软雅黑", 9F);
            tabGradient11.EndColor = System.Drawing.SystemColors.ActiveCaption;
            tabGradient11.LinearGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            tabGradient11.StartColor = System.Drawing.SystemColors.GradientActiveCaption;
            tabGradient11.TextColor = System.Drawing.SystemColors.ActiveCaptionText;
            dockPaneStripToolWindowGradient2.ActiveCaptionGradient = tabGradient11;
            tabGradient12.EndColor = System.Drawing.SystemColors.Control;
            tabGradient12.StartColor = System.Drawing.SystemColors.Control;
            tabGradient12.TextColor = System.Drawing.SystemColors.ControlText;
            dockPaneStripToolWindowGradient2.ActiveTabGradient = tabGradient12;
            dockPanelGradient6.EndColor = System.Drawing.SystemColors.ControlLight;
            dockPanelGradient6.StartColor = System.Drawing.SystemColors.ControlLight;
            dockPaneStripToolWindowGradient2.DockStripGradient = dockPanelGradient6;
            tabGradient13.EndColor = System.Drawing.SystemColors.InactiveCaption;
            tabGradient13.LinearGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            tabGradient13.StartColor = System.Drawing.SystemColors.GradientInactiveCaption;
            tabGradient13.TextColor = System.Drawing.SystemColors.InactiveCaptionText;
            dockPaneStripToolWindowGradient2.InactiveCaptionGradient = tabGradient13;
            tabGradient14.EndColor = System.Drawing.Color.Transparent;
            tabGradient14.StartColor = System.Drawing.Color.Transparent;
            tabGradient14.TextColor = System.Drawing.SystemColors.ControlDarkDark;
            dockPaneStripToolWindowGradient2.InactiveTabGradient = tabGradient14;
            dockPaneStripSkin2.ToolWindowGradient = dockPaneStripToolWindowGradient2;
            dockPanelSkin2.DockPaneStripSkin = dockPaneStripSkin2;
            this.dockPanel1.Skin = dockPanelSkin2;
            this.dockPanel1.TabIndex = 2;
            // 
            // RenderDock
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(414, 324);
            this.Controls.Add(this.dockPanel1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.renderBase1);
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "RenderDock";
            this.TabText = "渲染";
            this.Text = "RenderDock";
            this.Load += new System.EventHandler(this.RenderDock_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Widgets.RenderScene renderBase1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton design;
        private System.Windows.Forms.ToolStripButton source;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton reset;
        private TickDream.WinUI.Docking.VS2003Theme vS2003Theme1;
        private TickDream.WinUI.Docking.DockPanel dockPanel1;


    }
}