using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxesProject.Exceptions
{
    internal class LimitQuantityExceiption : Exception
    {
        public LimitQuantityExceiption() { }
        public LimitQuantityExceiption(string message) : base(message) { }
        public LimitQuantityExceiption(string paramName, string message) : base(message) { }
        public LimitQuantityExceiption(string message, Exception innerException) : base(message, innerException) { }
    }
}
