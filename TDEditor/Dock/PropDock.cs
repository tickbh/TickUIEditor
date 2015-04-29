using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TDEditor.Widgets;
using TDEditor.Utils;
using System.Xml;
using TDEditor.Prop;
using System.Reflection;
using TDEditor.Data;
using TDEditor.project;

namespace TDEditor.Dock
{
    public partial class PropDock : ToolWindow
    {
        public PropDock()
        {
            InitializeComponent();

            EventManager.RegisterAudience(Constant.PropChange, new EventHandler<object>(itemChange));
            EventManager.RegisterAudience(Constant.ProjectChange, new EventHandler<object>(projectChange));
            EventManager.RegisterAudience(Constant.PropSelectChange, new EventHandler<object>(itemSelect));

            this.propGrid.AllowDrop = true;
            this.propGrid.DragDrop += new DragEventHandler(propertyGrid1_DragDrop);
            this.propGrid.DragEnter += new DragEventHandler(propertyGrid1_DragEnter);
            this.propGrid.SelectedGridItemChanged += new SelectedGridItemChangedEventHandler(propGrid_SelectChange);
        }


        void propGrid_SelectChange(object sender, SelectedGridItemChangedEventArgs e)
        {
            if (e.NewSelection != null && e.NewSelection.PropertyDescriptor != null && this.propGrid.SelectedObject is CustomProperty)
            {
                Attribute attr = e.NewSelection.PropertyDescriptor.Attributes[typeof(ImageAttribute)];
                if (attr != null)
                {
                    EventManager.RaiserEvent(Constant.PreViewImageChange, this.propGrid, e.NewSelection.Value);
                }
            }

            
        }
        void propertyGrid1_DragEnter(object sender, DragEventArgs e)
        {
            ControlDDData data = e.Data.GetData(typeof(ControlDDData)) as ControlDDData;
            if (data != null && data.isImage)
            {
                e.Effect = e.AllowedEffect;
            }
            else
                e.Effect = DragDropEffects.None;
        }



        void propertyGrid1_DragDrop(object sender, DragEventArgs e)
        {
            ControlDDData data = e.Data.GetData(typeof(ControlDDData)) as ControlDDData;
            object propertyGridView = GetPropertyGridView(this.propGrid);
            GridItemCollection allGridEntries = GetAllGridEntries(propertyGridView);
            int top = GetTop(propertyGridView);
            int itemHeight = GetCachedRowHeight(propertyGridView);
            VScrollBar scrollBar = GetVScrollBar(propertyGridView);


            GridItem item = GetItemAtPoint(allGridEntries, top, itemHeight, scrollBar.Value, this.propGrid.PointToClient(new Point(e.X, e.Y)));
            if (item != null && item.PropertyDescriptor != null && this.propGrid.SelectedObject is CustomProperty)
            {
                Attribute attr = item.PropertyDescriptor.Attributes[typeof(ImageAttribute)];
                if (attr != null)
                {
                    item.PropertyDescriptor.SetValue((this.propGrid.SelectedObject as CustomProperty).GetPropertyOwner(null), UIProject.Instance().GetRelativePath(data.controlData as String));
                }
            }
            this.propGrid.Refresh();
        }



        /// <summary>

        /// Get the private PropertyGridView from the PropertyGrid

        /// </summary>

        /// <param name="propertyGrid"></param>

        /// <returns></returns>

        object GetPropertyGridView(PropertyGrid propertyGrid)
        {

            foreach (Control c in propertyGrid.Controls)
            {

                if (c.GetType().Name == "PropertyGridView")

                    return c;

            }

            return null;

        }



        /// <summary>

        /// Get the Grid Items collection

        /// </summary>

        /// <param name="propertyGridView"></param>

        /// <returns></returns>

        GridItemCollection GetAllGridEntries(object propertyGridView)
        {

            FieldInfo fi = propertyGridView.GetType().GetField("allGridEntries", BindingFlags.NonPublic | BindingFlags.Instance);

            return (GridItemCollection)fi.GetValue(propertyGridView);

        }



        /// <summary>

        /// Get the Top Value of the propertyGridView within the PropertyGrid

        /// </summary>

        /// <param name="propertyGridView"></param>

        /// <returns></returns>

        int GetTop(object propertyGridView)
        {

            Control ctrl = (Control)propertyGridView;

            return ctrl.Top;

        }



        /// <summary>

        /// Item the griditem height

        /// </summary>

        /// <param name="propertyGridView"></param>

        /// <returns></returns>

        int GetCachedRowHeight(object propertyGridView)
        {

            FieldInfo fi = propertyGridView.GetType().GetField("cachedRowHeight", BindingFlags.NonPublic | BindingFlags.Instance);

            return (int)fi.GetValue(propertyGridView);

        }



        /// <summary>

        /// Get the Vertical scroll bar

        /// </summary>

        /// <param name="propertyGridView"></param>

        /// <returns></returns>

        VScrollBar GetVScrollBar(object propertyGridView)
        {

            FieldInfo fi = propertyGridView.GetType().GetField("scrollBar", BindingFlags.NonPublic | BindingFlags.Instance);

            return (VScrollBar)fi.GetValue(propertyGridView);

        }



        /// <summary>

        /// Calculate and return the item at the point

        /// </summary>

        /// <param name="items"></param>

        /// <param name="top"></param>

        /// <param name="itemHeight"></param>

        /// <param name="scrollItems"></param>

        /// <param name="pt"></param>

        /// <returns></returns>

        GridItem GetItemAtPoint(GridItemCollection items, int top, int itemHeight, int scrollItems, Point pt)
        {
            int idx = (pt.Y - top) / (itemHeight + 1);
            idx += scrollItems;
            if (idx >= items.Count)
            {
                return null;
            }
            GridItem item = items[idx];

            return item;

        }


        public PropertyGrid PropGrid
        {
            get { return propGrid; }
        }

        public void SetSelectItem(RenderBase render)
        {
            if (render == null)
            {
                propGrid.SelectedObject = null;
                return;
            }
            XmlNode tmpXNode = DynamicObj.propXmlDoc.SelectSingleNode("Components/Component[@Name='" + render.Name + "']");
            XmlNodeList tmpXPropLst = null;
            if (tmpXNode != null)
            {
                tmpXPropLst = tmpXNode.SelectNodes("Propertys/Property");
            }
            CustomProperty cp = new CustomProperty(render, tmpXPropLst);
            tmpXNode = DynamicObj.propXmlDoc.SelectSingleNode("Components/Component[@Name='Base']");
            if (tmpXNode != null)
            {
                tmpXPropLst = tmpXNode.SelectNodes("Propertys/Property");
                cp.AddProperty(tmpXPropLst);
            }
            propGrid.SelectedObject = cp;
        }

        protected void itemChange(object sender, object e)
        {
            if(this.Focused)
            {
                return;
            }
            if (sender == this.propGrid.SelectedObject
                || (this.propGrid.SelectedObject is CustomProperty && sender == ((CustomProperty)this.propGrid.SelectedObject).GetPropertyOwner(null)))
            {
                this.propGrid.Refresh();
            }
            Console.WriteLine("leave");
        }

        protected void projectChange(object sender, object e)
        {
            SetSelectItem(null);
        }
        

        protected void itemSelect(object sender, object e)
        {
            if (sender is RenderBase)
            {
                SetSelectItem((RenderBase)sender);
            }
            else
            {
                SetSelectItem(null);
            }
        }
    }
}
