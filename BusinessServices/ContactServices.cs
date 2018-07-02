using DataModel.DbOperation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using AutoMapper;
using System.Text;
using System.Threading.Tasks;
using DataModel;
using BusinessEntities;

namespace BusinessServices
{
    /// <summary>
    /// contact services specific CRUD operations
    /// </summary>
    public class ContactServices : IContactServices
    {

        private readonly DbOperation _dbOperation;
       

        /// <summary>
        /// Public constructor.
        /// </summary>
        public ContactServices(DbOperation dbOperation)
        {
            _dbOperation = dbOperation;
        }

        /// <summary>
        /// Fetches contact details by id
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public ContactEntity GetContactById(int Id)
        {
            var contact = _dbOperation.ContactRepository.GetByID(Id);
            if (contact != null)
            {
                var config = new MapperConfiguration(cfg => {
                    cfg.CreateMap<Contact, ContactEntity>();
                });

                IMapper mapper = config.CreateMapper();                               
                var conatctModel = mapper.Map<Contact, ContactEntity>(contact);
                return conatctModel;
            }
            return null;
        }

        /// <summary>
        /// Fetches all the contacts.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ContactEntity> GetAllContacts()
        {
            var contacts = _dbOperation.ContactRepository.GetAll().ToList();
            if (contacts.Any())
            {
                var config = new MapperConfiguration(cfg => {
                    cfg.CreateMap<Contact, ContactEntity>();
                });

                IMapper mapper = config.CreateMapper();
                var conatctModel = mapper.Map<List<Contact>, List<ContactEntity>>(contacts);
                return conatctModel;
            }
            return null;
        }

        /// <summary>
        /// Creates a Contact
        /// </summary>
        /// <param name="contactEntity"></param>
        /// <returns></returns>
        public int CreateContact(ContactEntity contactEntity)
        {
            using (var scope = new TransactionScope())
            {                
                var contact = new Contact
                {                                         
                    FirstName = contactEntity.FirstName,
                    LastName = contactEntity.LastName,
                    Address = contactEntity.Address,
                    Email = contactEntity.Email,
                    PhoneNumber = contactEntity.PhoneNumber,
                    Status = contactEntity.Status.ToString()
                };
                _dbOperation.ContactRepository.Insert(contact);
                _dbOperation.Save();
                scope.Complete();
                return contact.ID;
            }
        }

        /// <summary>
        /// Updates a contact
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="contactEntity"></param>
        /// <returns></returns>
        public bool UpdateContact(int Id, ContactEntity contactEntity)
        {
            var success = false;
            if (contactEntity != null)
            {
                using (var scope = new TransactionScope())
                {
                    var contact = _dbOperation.ContactRepository.GetByID(Id);
                    if (contact != null)
                    {
                        contact.FirstName = contactEntity.FirstName;
                        contact.LastName = contactEntity.LastName;
                        contact.Address = contactEntity.Address;
                        contact.Email = contactEntity.Email;
                        contact.PhoneNumber = contactEntity.PhoneNumber;
                        contact.Status = contactEntity.Status.ToString();
                        _dbOperation.ContactRepository.Update(contact);
                        _dbOperation.Save();
                        scope.Complete();
                        success = true;
                    }
                }
            }
            return success;
        }

        /// <summary>
        /// Deletes a particular contact
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public bool DeleteContact(int Id)
        {
            var success = false;
            if (Id > 0)
            {
                using (var scope = new TransactionScope())
                {
                    var contact = _dbOperation.ContactRepository.GetByID(Id);
                    if (contact != null)
                    {                        
                        _dbOperation.ContactRepository.Delete(contact);
                        _dbOperation.Save();
                        scope.Complete();
                        success = true;
                    }
                }
            }
            return success;
        }

        
    }
}
