using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace TDEditor.Utils
{
    class XmlHelper
    {

        public static String GetString(XElement xml, String key, String value = "")
        {
            XAttribute node = xml.Attribute(key);
            if (node == null)
            {
                return value;
            }
            return node.Value;
        }

        public static float GetFloat(XElement xml, String key, float value = 0)
        {
            try
            {
                value = Single.Parse(GetString(xml, key));
            }
            catch 
            {
            }
            return value;
        }

        public static bool GetBool(XElement xml, String key, bool value = true)
        {
            try
            {
                value = Boolean.Parse(GetString(xml, key));
            }
            catch
            {
            }
            return value;
        }

        public static int GetInt(XElement xml, String key, int value = 0)
        {
            try
            {
                value = int.Parse(GetString(xml, key));
            }
            catch
            {
            }
            return value;
        }

        public static XElement Parse(String content)
        {
            try
            {
                return XElement.Parse(content);
            }
            catch (XmlException ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
            return null;
        }
        
    }
}
