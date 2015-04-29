using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDEditor.Utils
{
    class NestException : Exception
    {
        public NestException(string message) : base(message)
        {
            
        }
    }
}
