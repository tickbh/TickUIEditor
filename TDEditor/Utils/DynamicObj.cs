using TDEditor.Dock;
using TDEditor.Widgets;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace TDEditor.Utils
{
    class DynamicObj
    {
        public static ObjectDock objectDock;
        public static ImagePreViewDock imagePreViewDock;
        public static ResourceDock resourceDock;
        public static PropDock propDock;
        public static AnimateDock animateDock;
        private static Image _MissImage;
        public static Image MissImage
        {
            get
            {
                return _MissImage;
            }
        }

        public static Image DefaultSpriteImage;
        public static Image DefaultScale9Image;
        public static TDScale9 DefaultBtnDisableImg;
        public static TDScale9 DefaultBtnNormalImg;
        public static TDScale9 DefaultBtnSelectImg;
        public static Image DefaultCBDisableImg;
        public static Image DefaultCBNormalImg;
        public static Image DefaultCBSelectImg;
        public static Image DefaultCBNodeDisableImg;
        public static Image DefaultCBNodeNormalImg;
        public static Image DefaultSliderBackImg;
        public static Image DefaultSliderBarImg;
        public static Image DefaultSliderDisableImg;
        public static Image DefaultSliderNormalImg;
        public static Image DefaultSliderSelectImg;
        public static Image DefaultProgressImg;

        public static Image DefaultSceneBgImg;
        public static Image DefaultInputBgImg;

        public static XmlDocument propXmlDoc = new XmlDocument();

        static DynamicObj()
        {

        }

        public static void initDynamic()
        {
            String startPath = Application.StartupPath + "\\";
            propXmlDoc.Load(Application.StartupPath + "\\Resources\\CustomProperty.xml");

            DefaultSceneBgImg = Image.FromFile(startPath + Constant.PathSceneBgImg);

            _MissImage = Image.FromFile(startPath + Constant.PathMissImg);
            DefaultSpriteImage = Image.FromFile(startPath + Constant.PathSpriteImg);
            DefaultScale9Image = Image.FromFile(startPath + Constant.PathScale9Img);


            DefaultBtnDisableImg = TDScale9.CreateScale9(startPath + Constant.PathBtnDisableImg);
            DefaultBtnNormalImg = TDScale9.CreateScale9(startPath + Constant.PathBtnNormalImg);
            DefaultBtnSelectImg = TDScale9.CreateScale9(startPath + Constant.PathBtnSelectImg);


            DefaultCBDisableImg = Image.FromFile(startPath + Constant.PathCBDisableImg);
            DefaultCBNormalImg = Image.FromFile(startPath + Constant.PathCBNormalImg);
            DefaultCBSelectImg = Image.FromFile(startPath + Constant.PathCBSelectImg);
            DefaultCBNodeDisableImg = Image.FromFile(startPath + Constant.PathCBNodeDisableImg);
            DefaultCBNodeNormalImg = Image.FromFile(startPath + Constant.PathCBNodeNormalImg);

            DefaultSliderBackImg = Image.FromFile(startPath + Constant.PathSliderBackImg);
            DefaultSliderBarImg = Image.FromFile(startPath + Constant.PathSliderBarImg);
            DefaultSliderDisableImg = Image.FromFile(startPath + Constant.PathSliderDisableImg);
            DefaultSliderNormalImg = Image.FromFile(startPath + Constant.PathSliderNormalImg);
            DefaultSliderSelectImg = Image.FromFile(startPath + Constant.PathSliderSelectImg);

            DefaultProgressImg = Image.FromFile(startPath + Constant.PathProgressImg);
            DefaultInputBgImg = Image.FromFile(startPath + Constant.PathInputBgImg);
        }
    }
}
