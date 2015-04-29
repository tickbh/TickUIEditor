using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Xml;
using System.Windows.Forms;
using TDEditor.Widgets;
using TDEditor.Recall;
using TDEditor.Utils;

namespace TDEditor.Prop
{
    class CustomProperty : ICustomTypeDescriptor
    {
        //当前选择对象
        private object mCurrentSelectObject;
        private Dictionary<string, string> mObjectAttribs = new Dictionary<string, string>();
        private Dictionary<string, string> mObjectGroup = new Dictionary<string, string>();
        public CustomProperty(object pSelectObject, XmlNodeList pObjectPropertys)
        {
            mCurrentSelectObject = pSelectObject;
            AddProperty(pObjectPropertys);
        }

        public void AddProperty(XmlNodeList pObjectPropertys) {
            if (pObjectPropertys == null)
            {
                return;
            }
            XmlNode tmpXNode;
            IEnumerator tmpIe = pObjectPropertys.GetEnumerator();
            while (tmpIe.MoveNext())
            {
                tmpXNode = tmpIe.Current as XmlNode;
                mObjectAttribs.Add(tmpXNode.Attributes["Name"].Value, tmpXNode.Attributes["Caption"].Value);
                mObjectGroup.Add(tmpXNode.Attributes["Name"].Value, tmpXNode.Attributes["Group"].Value);
            }
        }
        #region ICustomTypeDescriptor Members
        public AttributeCollection GetAttributes()
        {
            return TypeDescriptor.GetAttributes(mCurrentSelectObject);
        }
        public string GetClassName()
        {
            return TypeDescriptor.GetClassName(mCurrentSelectObject);
        }
        public string GetComponentName()
        {
            return TypeDescriptor.GetComponentName(mCurrentSelectObject);
        }
        public TypeConverter GetConverter()
        {
            return TypeDescriptor.GetConverter(mCurrentSelectObject);
        }
        public EventDescriptor GetDefaultEvent()
        {
            return TypeDescriptor.GetDefaultEvent(mCurrentSelectObject);
        }
        public PropertyDescriptor GetDefaultProperty()
        {
            return TypeDescriptor.GetDefaultProperty(mCurrentSelectObject);
        }
        public object GetEditor(Type editorBaseType)
        {
            return TypeDescriptor.GetEditor(mCurrentSelectObject, editorBaseType);
        }
        public EventDescriptorCollection GetEvents(Attribute[] attributes)
        {
            return TypeDescriptor.GetEvents(mCurrentSelectObject, attributes);
        }
        public EventDescriptorCollection GetEvents()
        {
            return TypeDescriptor.GetEvents(mCurrentSelectObject);
        }
        public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            List<CustomPropertyDescriptor> tmpPDCLst = new List<CustomPropertyDescriptor>();
            PropertyDescriptorCollection tmpPDC = TypeDescriptor.GetProperties(mCurrentSelectObject, attributes);
            IEnumerator tmpIe = tmpPDC.GetEnumerator();
            CustomPropertyDescriptor tmpCPD;
            PropertyDescriptor tmpPD;
            while (tmpIe.MoveNext())
            {
                tmpPD = tmpIe.Current as PropertyDescriptor;
                if (mObjectAttribs.ContainsKey(tmpPD.Name))
                {
                    tmpCPD = new CustomPropertyDescriptor(mCurrentSelectObject, tmpPD);
                    tmpCPD.SetDisplayName(mObjectAttribs[tmpPD.Name]);
                    if (mObjectGroup.ContainsKey(tmpPD.Name))
                    {
                        tmpCPD.SetCategory(mObjectGroup[tmpPD.Name]);
                    }
                    else
                    {
                        tmpCPD.SetCategory(tmpPD.Category + "中文");
                    }
                    tmpPDCLst.Add(tmpCPD);
                }
            }
            return new PropertyDescriptorCollection(tmpPDCLst.ToArray());
        }
        public PropertyDescriptorCollection GetProperties()
        {
            return TypeDescriptor.GetProperties(mCurrentSelectObject);
        }
        public object GetPropertyOwner(PropertyDescriptor pd)
        {
            return mCurrentSelectObject;
        }
        #endregion
        class CustomPropertyDescriptor : PropertyDescriptor
        {
            private PropertyDescriptor mProp;
            private object mComponent;

            public CustomPropertyDescriptor(object pComponent, PropertyDescriptor pPD)
                : base(pPD)
            {
                mCategory = base.Category;
                mDisplayName = base.DisplayName;
                mProp = pPD;
                mComponent = pComponent;
            }
            private string mCategory;
            public override string Category
            {
                get { return mCategory; }
            }
            private string mDisplayName;
            public override string DisplayName
            {
                get { return mDisplayName; }
            }
            public void SetDisplayName(string pDispalyName)
            {
                mDisplayName = pDispalyName;
            }
            public void SetCategory(string pCategory)
            {
                mCategory = pCategory;
            }
            public override bool CanResetValue(object component)
            {
                return mProp.CanResetValue(component);
            }

            public override Type ComponentType
            {
                get { return mProp.ComponentType; }
            }

            public override object GetValue(object component)
            {
                return mProp.GetValue(component);
            }

            public override bool IsReadOnly
            {
                get { return mProp.IsReadOnly; }
            }

            public override Type PropertyType
            {
                get { return mProp.PropertyType; }
            }
            public override void ResetValue(object component) { mProp.ResetValue(component); }
            public override void SetValue(object component, object value) {

                if (component is RenderBase)
                {
                    RenderBase render = component as RenderBase;
                    RenderScene scene = render.getRenderScene();
                    render.recordOriStatus();
                    mProp.SetValue(component, value);
                    if (scene == null)
                    {
                        return;   
                    }
                    if (mProp.Name == "pos")
                    {
                        scene.commandManager.AddCommand(new CommandMove(scene, render));
                    }
                    else if (mProp.Name == "size")
                    {
                        scene.commandManager.AddCommand(new CommandResize(scene, render));
                    }
                    else if (mProp.Name == "anchorPos")
                    {
                        scene.commandManager.AddCommand(new CommandAnchorPos(scene, render));
                    }
                    else if (mProp.Name == "scale")
                    {
                        scene.commandManager.AddCommand(new CommandScale(scene, render));
                    }
                    else if (mProp.Name == "Tag")
                    {
                        EventManager.RaiserEvent(Constant.RenderItemChange, scene, null);

                    }


                    scene.checkModifyStatus();

                }
                else
                {
                    mProp.SetValue(component, value); 
                }
            }
            public override bool ShouldSerializeValue(object component)
            {
                return mProp.ShouldSerializeValue(component);
            }
        }
    }
}