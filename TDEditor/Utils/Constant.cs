using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDEditor.Utils
{
    class Constant
    {
        public const String TypeSprite = "Sprite";
        public const String TypeButton = "Button";
        public const String TypeCheckBox = "CheckBox";
        public const String TypeScale9 = "Scale9";
        public const String TypeProgressBar = "ProgressBar";
        public const String TypeSliderBar = "SliderBar";
        public const String TypeText = "Text";
        public const String TypeInput = "Input";
        public const String TypePanel = "Panel";
        public const String TypePage = "Page";
        public const String TypeScene = "Scene";

        public const String ModifyStatusChange = "ModifyStatusChange";
        public const String PropChange = "PropChange";
        public const String PropSelectChange = "PropSelectChange";
        public const String ProjectChange = "ProjectChange";
        public const String PreViewImageChange = "PreViewImageChange";
        public const String ResourceChange = "ResourceChange";
        public const String StatusInfoChange = "StatusInfoChange";
        public const String OpenLayoutEvent = "OpenLayoutEvent";
        public const String ActiveRenderChange = "ActiveRenderChange";
        public const String RenderItemChange = "RenderItemChange";

        public const String PathSceneBgImg = "Images\\grid.png";

        public const String PathMissImg = "Images\\default_miss.png";
        public const String PathSpriteImg = "Images\\Sprite.png";
        public const String PathScale9Img = "Images\\ImageFile.png";

        public const String PathBtnDisableImg = "Images\\Button_Disable.png";
        public const String PathBtnNormalImg = "Images\\Button_Normal.png";
        public const String PathBtnSelectImg = "Images\\Button_Select.png";

        public const String PathCBDisableImg = "Images\\CheckBox_Disable.png";
        public const String PathCBNormalImg = "Images\\CheckBox_Normal.png";
        public const String PathCBSelectImg = "Images\\CheckBox_Select.png";
        public const String PathCBNodeDisableImg = "Images\\CheckBoxNode_Disable.png";
        public const String PathCBNodeNormalImg = "Images\\CheckBoxNode_Normal.png";

        public const String PathSliderBackImg = "Images\\Slider_Back.png";
        public const String PathSliderBarImg = "Images\\Slider_Bar.png";
        public const String PathSliderDisableImg = "Images\\SliderNode_Disable.png";
        public const String PathSliderNormalImg = "Images\\SliderNode_Normal.png";
        public const String PathSliderSelectImg = "Images\\SliderNode_Select.png";

        public const String PathProgressImg = "Images\\ProgressFile.png";

        public const String PathInputBgImg = "Images\\shurukuang.png";

        public const String DefaultSingleFormat = "f2";
        public const String DefaultSingleIntFormat = "f0";

        public static readonly PointF PosF11 = new PointF(1, 1);
        public static readonly PointF PosF05 = new PointF(0.5f, 0.5f);
        public static readonly PointF PosF00 = new PointF(0, 0);

        public static readonly Point Pos11 = new Point(1, 1);
        public static readonly Point Pos00 = new Point(0, 0);
    }
}
