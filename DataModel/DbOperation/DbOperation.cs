using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModel.GenericRepository;
using System.Data.Entity.Validation;
using System.Diagnostics;

namespace DataModel.DbOperation
{

    /// <summary>
    /// Unit of Work class responsible for DB transactions
    /// </summary>
    public class DbOperation : IDisposable, IDbOperation
    {
        #region Private member variables...

        private readonly ContactsEntities _context = null;      
        private GenericRepository<Contact> _contactRepository;
       
        #endregion

        public DbOperation()
        {
            _context = new ContactsEntities();
        }

        #region Public Repository Creation properties...

        /// <summary>
        /// Get Property for contact repository.
        /// </summary>
        public GenericRepository<Contact> ContactRepository
        {
            get
            {
                if (this._contactRepository == null)
                    this._contactRepository = new GenericRepository<Contact>(_context);
                return _contactRepository;
            }
        }                    
        #endregion

        #region Public member methods...
        /// <summary>
        /// Save method.
        /// </summary>
        public void Save()
        {
            try
            {
                _context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {

                var outputLines = new List<string>();
                foreach (var eve in e.EntityValidationErrors)
                {
                    outputLines.Add(string.Format("{0}: Entity of type \"{1}\" in state \"{2}\" has the following validation errors:", DateTime.Now, eve.Entry.Entity.GetType().Name, eve.Entry.State));
                    foreach (var ve in eve.ValidationErrors)
                    {
                        outputLines.Add(string.Format("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage));
                    }
                }
                System.IO.File.AppendAllLines(@"C:\errors.txt", outputLines);

                throw e;
            }

        }

        #endregion

        #region Implementing IDiosposable...

        #region private dispose variable declaration...
        private bool disposed = false;
        #endregion

        /// <summary>
        /// Protected Virtual Dispose method
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    Debug.WriteLine("DbOperation is being disposed");
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        /// <summary>
        /// Dispose method
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
