using NHibernate;
using Trex.Server.Core.Exceptions;
using Trex.Server.Core.Model;
using Trex.Server.Core.Services;

namespace Trex.Server.Infrastructure.Implemented
{
    public class InvoiceRepository : RepositoryBase, IInvoiceRepository
    {
        #region IInvoiceRepository Members

        /// <summary>
        /// Inserts the specified invoice.
        /// </summary>
        /// <param name="invoice">The invoice.</param>
        /// <exception cref="EntityUpdateException"></exception>
        public void Update(Invoice invoice)
        {
            var session = GetSession();

            session.BeginTransaction();
            try
            {
                session.Update(invoice);
                session.Flush();
                session.Transaction.Commit();
            }
            catch (HibernateException ex)
            {
                session.Transaction.Rollback();
                throw new EntityUpdateException("Error updating invoice", ex);
            }
        }

        /// <summary>
        /// Gets the by id.
        /// </summary>
        /// <param name="invoiceId">The invoice id.</param>
        /// <returns></returns>
        public Invoice GetById(int invoiceId)
        {
            var session = GetSession();
            return session.Get<Invoice>(invoiceId);
        }

        #endregion
    }
}