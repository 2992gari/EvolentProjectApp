using BusinessEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessServices
{
    /// <summary>
    /// Contact Service Contract
    /// </summary>
    public interface IContactServices
    {
        ContactEntity GetContactById(int Id);
        IEnumerable<ContactEntity> GetAllContacts();
        int CreateContact(ContactEntity contactEntity);
        bool UpdateContact(int Id, ContactEntity contactEntity);
        bool DeleteContact(int Id);
    }
}
