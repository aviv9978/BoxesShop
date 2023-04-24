using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxesDLL
{
    internal struct Box
    {
        public double X { get; set; }
        public double Y { get; set; }
        public Box(double x, double y)
        {
            X = x;
            Y = y;
        }
    }
}
