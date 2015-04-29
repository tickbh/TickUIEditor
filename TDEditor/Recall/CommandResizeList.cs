using TDEditor.Widgets;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDEditor.Recall
{
    class CommandResize : CommandBase
    {
        public CommandResize(RenderScene scene, RenderBase render)
            : base(scene, render.uniqueName)
        {
            this._orign = render.oriSize;
            this._finish = render.size;
        }

        public override void Undo()
        {
            RenderBase item = renderScene.getRenderByUniqueName(uniqueName);
            if (item == null)
                return;
            item.size = _orign;
        }
        public override void Redo()
        {
            RenderBase item = renderScene.getRenderByUniqueName(uniqueName);
            if (item == null)
                return;
            item.size = _finish;
        }

        public override bool CheckVaild()
        {
            return _orign != _finish;
        }

        private SizeF _orign;
        private SizeF _finish;
    }
    class CommandResizeList : CommandBase
    {
        List<CommandResize> commands = new List<CommandResize>();
        public CommandResizeList(RenderScene scene, List<RenderBase> lists)
            :base(scene, "")
        {
            foreach (RenderBase render in lists)
            {
                commands.Add(new CommandResize(scene, render));
            }
        }

        public override void Undo()
        {
            foreach (CommandResize command in commands)
            {
                command.Undo();
            }
        }
        public override void Redo()
        {
            foreach (CommandResize command in commands)
            {
                command.Redo();
            }
        }
        public override bool CheckVaild()
        {
            foreach (CommandResize command in commands)
            {
                if (command.CheckVaild())
                {
                    return true;
                }
            }
            return false;
        }
    }
}
