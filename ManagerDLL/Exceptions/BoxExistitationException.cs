using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxesProject
{
    internal class BoxExistitationException : Exception
    {
        public BoxExistitationException() { }
        public BoxExistitationException(string message) : base(message) { }
        public BoxExistitationException(string paramName, string message) : base(message) { }
        public BoxExistitationException(string message, Exception innerException) : base(message, innerException) { }
    }
}
