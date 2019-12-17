using System;
using System.Windows.Forms;

namespace BTNs__Race
{
    class CompareButtons : Button, IComparable
    {
        public int CompareTo(object obj)
        {
            CompareButtons btn = (CompareButtons)obj;

            if (this.Location.X > btn.Location.X)
                return -1;
            if (this.Location.X < btn.Location.X)
                return 1;

            return 0;
        }
    }
}
