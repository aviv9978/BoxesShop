using BoxesDLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagerDLL
{
    internal class SLExpirationVal
    {
        public Box CurrentBox { get; set; }
        public DateTime StartingDate { get; set; }
        public SLExpirationVal(Box currentBox, DateTime startingDate) : base()
        {
            CurrentBox = currentBox;
            StartingDate = startingDate; // usually it will be today current date, but for the project
                                         //  i need it to be according to manager's current date
                                         //(So when we move day in manager and add a box it will be according to the "new day"
        }
    }
}
