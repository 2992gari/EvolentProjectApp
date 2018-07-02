using DataModel.GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.DbOperation
{
   public interface IDbOperation
    {

        #region Properties
        GenericRepository<Contact> ContactRepository { get; }

        #endregion
        /// <summary>
        /// Save method.
        /// </summary>
        void Save();
    }
}
