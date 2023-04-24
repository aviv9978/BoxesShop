using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxesProject.Exceptions
{
    internal class NotEnoughQuantityExceiption : Exception
    {
        public NotEnoughQuantityExceiption() { }
        public NotEnoughQuantityExceiption(string message) : base(message) { }
        public NotEnoughQuantityExceiption(string paramName, string message) : base(message) { }
        public NotEnoughQuantityExceiption(string message, Exception innerException) : base(message, innerException) { }
    }
}
