using TDEditor.Widgets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TDEditor.Utils;

namespace TDEditor.Recall
{
    class CommadnDel : CommandBase {
        private String parentUnique;
        private XElement xml;

        public CommadnDel(RenderScene scene, RenderBase opItem)
            : base(scene, opItem.uniqueName)
        {
            xml = UIHelper.generateXmlByItem(opItem);
            parentUnique = opItem.getParent().uniqueName;
        }

        public override void Undo()
        {
            RenderBase item = renderScene.getRenderByUniqueName(parentUnique);
            if (item == null)
                return;
            RenderBase undoItem = UIHelper.CEGenerateViewByXml(xml, item);
            if(undoItem != null)
                undoItem.uniqueName = this.uniqueName;
        }
        public override void Redo()
        {
            RenderBase item = renderScene.getRenderByUniqueName(uniqueName);
            if (item == null)
                return;
            item.removeFromParent();
        }

        public override bool CheckVaild()
        {
            return true;
        }

    }

    class CommandDelList : CommandBase
    { 
        List<CommadnDel> commands = new List<CommadnDel>();
        public CommandDelList(RenderScene scene, List<RenderBase> lists)
            :base(scene, "")
        {
            foreach (RenderBase render in lists)
            {
                commands.Add(new CommadnDel(scene, render));
            }
        }

        public override void Undo()
        {
            foreach (CommadnDel command in commands)
            {
                command.Undo();
            }
        }
        public override void Redo()
        {
            foreach (CommadnDel command in commands)
            {
                command.Redo();
            }
        }
        public override bool CheckVaild()
        {
            return commands.Count > 0;
        }
    }
}
