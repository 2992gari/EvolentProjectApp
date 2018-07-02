using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.Hosting;
using BusinessEntities;
using BusinessServices;
using DataModel;
using Moq;
using NUnit.Framework;
using Newtonsoft.Json;
using API.ContactManager.Controllers;
using TestHelper;
using ApiController.Tests.ErrorHelper;
using DataModel.DbOperation;
using DataModel.GenericRepository;

namespace ApiController.Tests
{
    [TestFixture]
    public class ContactControllerTest
    {
        #region Variables
        private IContactServices _contactService;                
        private List<Contact>_contacts;                               
        private HttpResponseMessage _response;     
        private const string ServiceBaseURL = "http://localhost:60086/";       
        private DbOperation _dbOperation;          
        private GenericRepository<Contact> _contactRepository;    
        private ContactsEntities _dbEntities;
        private HttpClient _client;
        #endregion

        [TestFixtureSetUp]
        public void Setup()
        {
            _contacts = SetUpContacts();           
            _dbEntities = new Mock<ContactsEntities>().Object;
            _contactRepository = SetUpContactRepository();
            var dbOperation = new Mock<DbOperation>();
            dbOperation.SetupGet(s => s.ContactRepository).Returns(_contactRepository);
            _dbOperation = dbOperation.Object;
            _contactService = new ContactServices(_dbOperation);          
            _client = new HttpClient { BaseAddress = new Uri(ServiceBaseURL) };
         
        }

        #region Unit Tests

        /// <summary>
        /// Get all contacts test
        /// </summary>
        [Test]
        public void GetAllContactsTest()
        {
            var contactController = new ContactController(_contactService)
            {
                Request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(ServiceBaseURL + "v1/Contacts/Contact/all")
                }
            };
            contactController.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());
            _response = contactController.Get();
            var responseResult = JsonConvert.DeserializeObject<List<Contact>>(_response.Content.ReadAsStringAsync().Result);
            Assert.AreEqual(_response.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(responseResult.Any(), true);
            var comparer = new ContactComparer();
            CollectionAssert.AreEqual(
                responseResult.OrderBy(contact => contact, comparer),
                _contacts.OrderBy(contact => contact, comparer), comparer);
        }

        /// <summary>
        /// Get contact by Id
        /// </summary>
        [Test]
        public void GetcontactByIdTest()
        {
            var contactController = new ContactController(_contactService)
            {
                Request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(ServiceBaseURL + "v1/Contacts/Contact/contactid/2")
                }
            };
            contactController.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());

            _response = contactController.Get(2);

            var responseResult = JsonConvert.DeserializeObject<Contact>(_response.Content.ReadAsStringAsync().Result);
            Assert.AreEqual(_response.StatusCode, HttpStatusCode.OK);
            AssertObjects.PropertyValuesAreEquals(responseResult,                                                  
                _contacts.Find(a => a.FirstName.Contains("Harry")));
        }

        /// <summary>
        /// Get contact by wrong Id
        /// </summary>
        [Test]
       
        public void GetContactByWrongIdTest()
        {
            var contactController = new ContactController(_contactService)
            {
                Request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(ServiceBaseURL + "v1/C/Contact/conatctid/10")                    
                }
            };
            contactController.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());
            var ex = Assert.Throws<ApiDataException>(() => contactController.Get(10));
            Assert.That(ex.ErrorCode, Is.EqualTo(1001));
            Assert.That(ex.ErrorDescription, Is.EqualTo("No contact found for this id."));

        }
      

        /// <summary>
        /// Create contact test
        /// </summary>
        [Test]
        public void CreateProductTest()
        {
            var contactController = new ContactController(_contactService)
            {
                Request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri(ServiceBaseURL + "v1/Contacts/Contact/Create")
                }
            };
            contactController.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());

            var newContact = new ContactEntity()
            {
                FirstName = "Rackson"
            };

            var maxProductIDBeforeAdd = _contacts.Max(a => a.ID);
            newContact.ID = maxProductIDBeforeAdd + 1;
            contactController.Post(newContact);
            var addedcontact = new Contact() { FirstName = newContact.FirstName, ID = newContact.ID };
            AssertObjects.PropertyValuesAreEquals(addedcontact,_contacts.Last());
            Assert.That(maxProductIDBeforeAdd + 1, Is.EqualTo(_contacts.Last().ID));
        }

        /// <summary>
        /// Update Contact test
        /// </summary>
        [Test]
        public void UpdateContactTest()
        {
            var contactController = new ContactController(_contactService)
            {
                Request = new HttpRequestMessage
                {
                    Method = HttpMethod.Put,
                    RequestUri = new Uri(ServiceBaseURL + "v1/Contacts/Contact/Modify")
                }
            };
            contactController.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());

            var firstContact = _contacts.First();
            firstContact.FirstName = "Contact updated";
            var updatedContact = new ContactEntity() { FirstName = firstContact.FirstName, ID = firstContact.ID };
            contactController.Put(firstContact.ID, updatedContact);
            Assert.That(firstContact.ID, Is.EqualTo(1));
        }

        /// <summary>
        /// Delete contact test
        /// </summary>
        [Test]
        public void DeleteContactTest()
        {
            var contactController = new ContactController(_contactService)
            {
                Request = new HttpRequestMessage
                {
                    Method = HttpMethod.Put,
                    RequestUri = new Uri(ServiceBaseURL + "v1/Contacts/Contact/Remove")
                }
            };
            contactController.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());

            int maxID = _contacts.Max(a => a.ID);
            var lastContact = _contacts.Last();

            // Remove last Product
            contactController.Delete(lastContact.ID);
            Assert.That(maxID, Is.GreaterThan(_contacts.Max(a => a.ID)));
        }

        #endregion
        /// <summary>
        /// Setup dummy contacts data
        /// </summary>
        /// <returns></returns>
        private static List<Contact> SetUpContacts()
        {
            var Id = new int();
            var contacts = DataInitializer.GetAllContacts();
            foreach (Contact cont in contacts)
                cont.ID = ++Id;
            return contacts;
        }
        private GenericRepository<Contact> SetUpContactRepository()
        {
            // Initialise repository
            var mockRepo = new Mock<GenericRepository<Contact>>(MockBehavior.Default, _dbEntities);

            // Setup mocking behavior
            mockRepo.Setup(p => p.GetAll()).Returns(_contacts);

            mockRepo.Setup(p => p.GetByID(It.IsAny<int>()))
            .Returns(new Func<int, Contact>(
            id => _contacts.Find(p => p.ID.Equals(id))));

            mockRepo.Setup(p => p.Insert((It.IsAny<Contact>())))
            .Callback(new Action<Contact>(newContact =>
            {
                dynamic maxContactID = _contacts.Last().ID;
                dynamic nextContactID = maxContactID + 1;
                newContact.ID = nextContactID;
                _contacts.Add(newContact);
            }));

            mockRepo.Setup(p => p.Update(It.IsAny<Contact>()))
            .Callback(new Action<Contact>(cont =>
            {
                var oldContact = _contacts.Find(a => a.ID == cont.ID);
                oldContact = cont;
            }));

            mockRepo.Setup(p => p.Delete(It.IsAny<Contact>()))
            .Callback(new Action<Contact>(cont =>
            {
                var contactToRemove =
        _contacts.Find(a => a.ID == cont.ID);

                if (contactToRemove != null)
                    _contacts.Remove(contactToRemove);
            }));

            // Return mock implementation object
            return mockRepo.Object;
        }
        #region Tear Down
        /// <summary>
        /// Tears down each test data
        /// </summary>
        [TearDown]
        public void DisposeTest()
        {
            if (_response != null)
                _response.Dispose();
            if (_client != null)
                _client.Dispose();
        }

        #endregion
        
    }
}
