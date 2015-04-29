using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDEditor.Utils
{
    class UtilHelper
    {
        public static bool isEqual(float a, float b)
        {
            return Math.Abs(a - b) < 0.0001f;
        }

        public static bool isEmpty(String a)
        {
            return a == null || a.Length == 0;
        }

        public static bool isEqual(Color a, Color b)
        {
            return a.A == b.A && a.R == b.R && a.G == b.G && a.B == b.B;
        }
    }
}
