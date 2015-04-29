using TDEditor.Widgets;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDEditor.Recall
{
    public class CommandMove : CommandBase
    {

        public CommandMove(RenderScene scene, RenderBase render)
            : base(scene, render.uniqueName)
        {
            this._orign = render.oriPos;
            this._finish = render.pos;
        }

        public override void Undo()
        {
            RenderBase item = renderScene.getRenderByUniqueName(uniqueName);
            if (item == null)
                return;
            item.pos = _orign;
        }
        public override void Redo()
        {
            RenderBase item = renderScene.getRenderByUniqueName(uniqueName);
            if (item == null)
                return;
            item.pos = _finish;
        }
        public override bool CheckVaild()
        {
            return _orign != _finish;
        }

        private PointF _orign;
        private PointF _finish;
    }

    class CommandMoveList : CommandBase
    {
        List<CommandMove> commands = new List<CommandMove>();
        public CommandMoveList(RenderScene scene, List<RenderBase> lists)
            :base(scene, "")
        {
            foreach (RenderBase render in lists)
            {
                commands.Add(new CommandMove(scene, render));
            }
        }

        public override void Undo()
        {
            foreach (CommandMove command in commands)
            {
                command.Undo();
            }
        }
        public override void Redo()
        {
            foreach (CommandMove command in commands)
            {
                command.Redo();
            }
        }
        public override bool CheckVaild()
        {
            foreach (CommandMove command in commands)
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
