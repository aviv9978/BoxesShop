using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagerDLL
{
    internal class SDHeightVal
    {
        public LinkedListNode<SLExpirationVal> NodeStorage { get; set; }
        public int Quantity { get; set; }
        public SDHeightVal(int quantity, LinkedListNode<SLExpirationVal> nordeStorage)
        {
            Quantity = quantity;
            NodeStorage = nordeStorage;
        }
    }
}
