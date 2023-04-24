using BoxesProject;
using BoxesProject.Exceptions;
using ManagerDLL;
using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BoxesDLL
{
    public class Manager
    {
        //Summery//
        //Manager contains a sorted dictionory which his keys are boxes width (X),
        // the values are another Sorted dictionory which his keys are boxes height (Y) and
        // the values are box's quantity in storage and a node for the expiration LinkedList.
        // The exiration LinkedList values are a class which holds a box (to recognize faster the box in the Sorted Dictinory),
        // and the datetime of the box.
        // To know if the box is 'expired' (wasn't in use and need to be thrown) the LinkedList is sorted by the first
        // who got into the list to the last one. When a day past, the LinkedList is being checked by the next rule:
        // First box is being checked, if it's being expired (wasn't in use[Quantity hasn't changed] for 50 days)
        //If so, the box is being removed from the LinkedList and the Sorted Dictionry.
        //The LinkedList keep to the next box and checks him as well. If the box expired she does the same and over and over.
        //The LinkedList stops checks and moving to next box when the box she checks is not expired.
        // (That's why the LinkedList is sorted by the date of the boxes[first box which joined the list is in the last position of the list.])
        // Max quantity for a type of box is 50.
        //Summery//

        public Manager()
        {
            _currentDay = DateTime.Today;

            _xKeys = new SortedDictionary<double, SortedDictionary<double, SDHeightVal>>();
            _expirationLinkedList = new LinkedList<SLExpirationVal>();
        }
        public Manager(double x, double y, int quantity) : this()
        {
            addNewBox(new Box(x, y), quantity);
        }
        public bool DoesBoxExisted(double x, double y)
        {
            if (_xKeys.ContainsKey(x))
                if (_xKeys[x].ContainsKey(y))
                    return true;
            return false;
        }
        public int GetBoxQuantity(double x, double y)
        {
            if (!DoesBoxExisted(x, y))
                throw new BoxExistitationException($"Box ({x},{y}) is not existed.");

            var selectedX = _xKeys[x];
            return selectedX[y].Quantity;
        }
        public void AddNewBoxToStorage(double x, double y, int quantity)
        {
            if (DoesBoxExisted(x, y))
                throw new BoxExistitationException("Box already existed in storage.");
            addNewBox(new Box(x, y), quantity);
        }
        public void AddQuantity(double x, double y, int quantity)
        {
            if (!DoesBoxExisted(x, y))
                throw new BoxExistitationException("Box doesn't exist in storage.");
            var currentX = _xKeys[x];
            if (currentX[y].Quantity + quantity > _maxQuantity)
                throw new LimitQuantityExceiption("New quantity can't be over the limit.");
            currentX[y].Quantity += quantity;
            updateBoxInLinkedList(x, y, currentX);
        }
        public string DayPast()
        {
            StringBuilder sb = new StringBuilder("Expired boxes:\n");
            _currentDay = _currentDay.AddDays(1);
            int count = _expirationLinkedList.Count;
            for (int i = 0; i < count; i++)
            {
                if (_currentDay.Subtract(_expirationLinkedList.First.Value.StartingDate).TotalDays > _maxDayPast)
                {
                    var current = _expirationLinkedList.First();
                    var YKeys = _xKeys[current.CurrentBox.X];
                    YKeys.Remove(current.CurrentBox.Y);
                    _expirationLinkedList.RemoveFirst();
                    sb.Append($"Box ({current.CurrentBox.X},{current.CurrentBox.Y}) has been expired and removed.\n");
                }
                else
                    return sb.ToString();
            }
            return sb.ToString();
        } //Removing expired boxes and returning which boxes were removed
        public bool RequiredStorageValidation(double x, double y, int quantity)
        {
            int copyOfRequiredQuantity = quantity;
            var filteredWidths = _xKeys.Where(Width => Width.Key <= x * _boxRangePercentage && Width.Key >= x);
            foreach (var width in filteredWidths)
            {
                var heightDict = width.Value.Where(height => height.Key <= y * _boxRangePercentage && height.Key >= y);
                foreach (var height in heightDict)
                {
                    if (height.Value.Quantity == 0)
                        break;

                    if (height.Value.Quantity >= copyOfRequiredQuantity)
                        return true;
                    else copyOfRequiredQuantity -= height.Value.Quantity;
                }
            }
            if (copyOfRequiredQuantity > 0)
                return false;
            return true;
        } // Use this function before any purchasing to know if stroage has the required quantity
        public string AlternativeQuantityBoxOffer(double x, double y, int quantity)
        {
            int copyOfRequiredQuantity = quantity;
            AlternativeQuantityOffer = 0;
            StringBuilder sb = new StringBuilder();
            sb.Append("Here is our offer with the quantity we do have:\n");
            if (DoesBoxExisted(x, y))
                if (GetBoxQuantity(x, y) > 0)
                {
                    sb.Append($"{GetBoxQuantity(x, y)} boxes of ({x},{y}) size.\n");
                    copyOfRequiredQuantity -= GetBoxQuantity(x, y);
                    AlternativeQuantityOffer += GetBoxQuantity(x, y);
                }
            var filteredWidths = _xKeys.Where(Width => Width.Key <= x * _boxRangePercentage && Width.Key > x);
            foreach (var width in filteredWidths)
            {
                var heightDict = width.Value.Where(height => height.Key <= y * _boxRangePercentage && height.Key >= y);
                foreach (var height in heightDict)
                {
                    if (height.Value.Quantity == 0)
                        break;

                    sb.Append($"{height.Value.Quantity} boxes of ({width.Key},{height.Key}) size.\n");
                    copyOfRequiredQuantity -= height.Value.Quantity;
                    AlternativeQuantityOffer += height.Value.Quantity;
                }
            }
            if (AlternativeQuantityOffer == 0)
                return "No alternative offer can be offered.";

            sb.Append($"Total alternative quantity we have to offer: {AlternativeQuantityOffer}");
            return sb.ToString();
        } // Alternative offer in case not enough quantity in storage
        public string BoxOffer(double x, double y, int quantity)
        {
            if (RequiredStorageValidation(x, y, quantity) == false)
                throw new NotEnoughQuantityExceiption("Purchase couldn't be completed based on the required quantity.");

            int copyOfRequiredQuantity = quantity;
            StringBuilder sb = new StringBuilder();
            sb.Append("Here is our offer:\n");

            var filteredWidths = _xKeys.Where(Width => Width.Key <= x * _boxRangePercentage && Width.Key >= x);
            foreach (var width in filteredWidths)
            {
                var heightDict = width.Value.Where(height => height.Key <= y * _boxRangePercentage && height.Key >= y);
                foreach (var height in heightDict)
                {
                    if (height.Value.Quantity == 0)
                        break;
                    if (height.Value.Quantity < copyOfRequiredQuantity)
                    {
                        sb.Append($"{height.Value.Quantity} boxes of ({width.Key},{height.Key}) size.\n");
                        copyOfRequiredQuantity -= height.Value.Quantity;
                    }
                    else
                    {
                        sb.Append($"{copyOfRequiredQuantity} boxes of ({width.Key},{height.Key}) size.\n");
                        return sb.ToString();

                    }
                }
            }
            return sb.ToString();
        } // Offer in case of enough quantity in storage
        public string FinalePurchase(double x, double y, int quantity)
        {
            if (RequiredStorageValidation(x, y, quantity) == false)
                throw new NotEnoughQuantityExceiption("Purchase couldn't be completed based on the required quantity.");

            int copyOfRequiredQuantity = quantity;
            StringBuilder warningSb = new StringBuilder();
            var filteredWidths = _xKeys.Where(Width => Width.Key <= x * _boxRangePercentage && Width.Key >= x); // find all relative widths
            foreach (var width in filteredWidths)
            {
                var heightDict = width.Value.Where(height => height.Key <= y * _boxRangePercentage && height.Key >= y); //checking only relative heights
                foreach (var height in heightDict)
                {
                    if (height.Value.Quantity == 0)
                        break;

                    int earlyBoxQuantity = height.Value.Quantity;

                    if (earlyBoxQuantity >= copyOfRequiredQuantity) //in case of box's quantity was enough to complete order
                    {
                        height.Value.Quantity -= copyOfRequiredQuantity;
                        updateBoxInLinkedList(width.Key, height.Key, width.Value);

                        if (height.Value.Quantity == 0)
                            warningSb.Append($"Pay attention: None amount of ({width.Key},{height.Key}) box has left.\n");
                        else if (height.Value.Quantity <= _minBoxesToWarn)
                            warningSb.Append($"Pay attention: {height.Value.Quantity} amount of ({width.Key},{height.Key}) box has left.\n");
                        return $"Purchase completed\n{warningSb.ToString()}";
                    }
                    else
                    {
                        height.Value.Quantity = 0;
                        updateBoxInLinkedList(width.Key, height.Key, width.Value);
                        warningSb.Append($"Pay attention: None amount of ({width.Key},{height.Key}) box has left.\n");
                        copyOfRequiredQuantity -= earlyBoxQuantity;
                    }

                }
            }
            return warningSb.ToString(); // the function won't ever get here,it's just for the demand on returning string
        } //After verifying offer with client based on box's sizes and quantity in storage.
        public int AlternativeQuantityOffer { get; private set; }
        public bool NewQuantityValidation(double width, double height, int quantity)
        {
            if (GetBoxQuantity(width, height) + quantity > _maxQuantity)
                return false;
            return true;
        }
        public int GetMaxQuantityLimit()
        {
            return _maxQuantity;
        }

        private void updateBoxInLinkedList(double x, double y, SortedDictionary<double, SDHeightVal> currentX)
        {
            _expirationLinkedList.Remove(currentX[y].NodeStorage);
            currentX[y].NodeStorage.Value.StartingDate = _currentDay; //update date(usally to current date but for this project...)
            _expirationLinkedList.AddLast(currentX[y].NodeStorage);
        }
        private void addNewBox(Box b1, int quantity)
        {
            var YKeys = new SortedDictionary<double, SDHeightVal>();
            var node = new LinkedListNode<SLExpirationVal>(new SLExpirationVal(b1, _currentDay));
            SDHeightVal bYValue = new SDHeightVal(quantity, node);
            YKeys.Add(b1.Y, bYValue);
            if (_xKeys.ContainsKey(b1.X))
                _xKeys[b1.X].Add(b1.Y, bYValue);
            else _xKeys.Add(b1.X, YKeys);
            _expirationLinkedList.AddLast(node);
        }


        private const int _maxQuantity = 50;
        private const int _maxDayPast = 2;
        private const int _minBoxesToWarn = 3;
        private const double _boxRangePercentage = 1.25;
        private DateTime _currentDay;
        private SortedDictionary<double, SortedDictionary<double, SDHeightVal>> _xKeys;
        private LinkedList<SLExpirationVal> _expirationLinkedList;
    }
}





