using API.ContactManager.Filters;
using BusinessEntities;
using BusinessServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace API.ContactManager.Controllers
{
    public class ContactController : ApiController
    {
        private readonly IContactServices _contactServices;

        #region Public Constructor

        /// <summary>
        /// Public constructor to initialize contact service instance
        /// </summary>
        public ContactController(IContactServices contactServices)
        {
            _contactServices = contactServices;
        }

        #endregion

        // GET api/contact
        public HttpResponseMessage Get()
        {
            var contacts = _contactServices.GetAllContacts();
            var contactEntities = contacts as List<ContactEntity> ?? contacts.ToList();
            if (contactEntities.Any())
                return Request.CreateResponse(HttpStatusCode.OK, contactEntities);
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Contact not found");
        }

        // GET api/contact/2
        public HttpResponseMessage Get(int id)
        {
            var contact = _contactServices.GetContactById(id);
            if (contact != null)
                return Request.CreateResponse(HttpStatusCode.OK, contact);
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No contact found for this id");
        }

        // POST api/contact
        [ValidateModel]
        public int Post([FromBody] ContactEntity contactEntity)
        {                        
            return _contactServices.CreateContact(contactEntity);
        }

        // PUT api/contact/1

        [ValidateModel]
        public bool Put(int id, [FromBody]ContactEntity contactEntity)
        {
            if (id > 0)
            {
                return _contactServices.UpdateContact(id, contactEntity);
            }
            return false;
        }

        // DELETE api/contact/4
        public bool Delete(int id)
        {
            if (id > 0)
                return _contactServices.DeleteContact(id);
            return false;
        }

    }
}
