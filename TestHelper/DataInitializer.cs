using DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestHelper
{
    public class DataInitializer
    {
        /// <summary>
        /// Dummy contacts
        /// </summary>
        /// <returns></returns>
        public static List<Contact> GetAllContacts()
        {
            var contacts = new List<Contact>
                               {
                                   new Contact()
                                   {
                                       FirstName = "Tommy",LastName= "Sey",Address= "UK 16",
                                     Email ="Tommy.Sey@gmail.com",PhoneNumber= "7799654599",
                                     Status="Inactive"
                                   },
                                   
                                   new Contact()
                                   {
                                     FirstName = "Sam",LastName= "Puri",Address= "USA 16",
                                     Email ="Sam.Sey@gmail.com",PhoneNumber= "7799654599",
                                     Status="Active"
                                   },
                                   new Contact()
                                   {
                                       FirstName = "Jal",LastName= "Band",Address= "SA 16",
                                     Email ="Band.Jal@gmail.com",PhoneNumber= "8899654599",
                                     Status="Inactive"
                                   },

                               };
            return contacts;
        }
    }
}
