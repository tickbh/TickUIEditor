using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using TDEditor.Widgets;
using TDEditor.Utils;

namespace TDEditor.Recall
{
    public class CommandAdd : CommandBase 
    {
        private String parentUnique;
        private XElement xml;

        public CommandAdd(RenderScene scene, RenderBase opItem)
            : base(scene, opItem.uniqueName)
        {
            xml = UIHelper.generateXmlByItem(opItem);
            parentUnique = opItem.getParent().uniqueName;
        }

        public override void Undo()
        {
            RenderBase item = renderScene.getRenderByUniqueName(uniqueName);
            if (item == null)
                return;
            item.removeFromParent();
        }
        public override void Redo()
        {
            RenderBase item = renderScene.getRenderByUniqueName(parentUnique);
            if (item == null)
                return;
            RenderBase undoItem = UIHelper.CEGenerateViewByXml(xml, item);
            if (undoItem != null)
                undoItem.uniqueName = this.uniqueName;
        }

        public override bool CheckVaild()
        {
            return true;
        }
    }
}
