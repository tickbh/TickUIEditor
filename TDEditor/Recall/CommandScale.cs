using TDEditor.Widgets;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDEditor.Recall
{

    public class CommandScale : CommandBase
    {

        public CommandScale(RenderScene scene, RenderBase render)
            : base(scene, render.uniqueName)
        {
            this._orign = render.oriScale;
            this._finish = render.scale;
        }

        public override void Undo()
        {
            RenderBase item = renderScene.getRenderByUniqueName(uniqueName);
            if (item == null)
                return;
            item.scale = _orign;
        }
        public override void Redo()
        {
            RenderBase item = renderScene.getRenderByUniqueName(uniqueName);
            if (item == null)
                return;
            item.scale = _finish;
        }

        public override bool CheckVaild()
        {
            return _orign != _finish;
        }

        private PointF _orign;
        private PointF _finish;
    }
}
