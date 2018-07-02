using DataModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestHelper
{    
        public class ContactComparer : IComparer, IComparer<Contact>
        {
            public int Compare(object expected, object actual)
            {
                var lhs = expected as Contact;
                var rhs = actual as Contact;
                if (lhs == null || rhs == null) throw new InvalidOperationException();
                return Compare(lhs, rhs);
            }
            public int Compare(Contact expected, Contact actual)
            {
                int temp;
                return (temp = expected.ID.CompareTo(actual.ID)) != 0 ? temp : expected.FirstName.CompareTo(actual.FirstName);
            }
        }

    
}
