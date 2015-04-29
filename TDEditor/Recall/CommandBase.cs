using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDEditor.Widgets;

namespace TDEditor.Recall
{
    public class CommandBase
    {
        public CommandBase(RenderScene scene, String uniqueName)
        {
            this.renderScene = scene;
            this.uniqueName = uniqueName;
        }
        public virtual void Undo()
        {

        }
        public virtual void Redo()
        {

        }

        public virtual bool CheckVaild()
        {
            return true;
        }

        public RenderScene renderScene;
        public String uniqueName;
    }
}
