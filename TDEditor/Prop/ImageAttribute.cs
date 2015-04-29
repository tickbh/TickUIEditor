using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDEditor.Prop
{
    class ImageAttribute : Attribute
    {

        public ImageAttribute()
        {

        }

        public bool IsImage
        {
            get
            {
                return true;
            }
        }
    }
}
