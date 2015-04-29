using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDEditor.Dock
{
    public partial class ImagePreViewDock : ToolWindow
    {

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // ImagePreViewDock
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(15F, 30F);
            this.ClientSize = new System.Drawing.Size(752, 680);
            this.Font = new System.Drawing.Font("宋体", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.Name = "ImagePreViewDock";
            this.TabText = "预览";
            this.Text = "预览";
            this.ResumeLayout(false);

        }
    }
}
