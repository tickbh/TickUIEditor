using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDEditor.Data
{
    class ControlDDData
    {
        public ControlDDData()
        {

        }
        public ControlDDData(String controlType, object controlData, object extNode = null, bool isImage = false)
        {
            this.controlType = controlType;
            this.controlData = controlData;
            this.extNode = extNode;
            this.isImage = isImage;
        }
        public String controlType;
        public object controlData;
        public object extNode;
        public bool isImage;
    }
}
