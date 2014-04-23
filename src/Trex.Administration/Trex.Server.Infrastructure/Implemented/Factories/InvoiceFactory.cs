using System;
using Trex.Server.Core.Exceptions;
using Trex.Server.Core.Model;
using Trex.Server.Core.Services;
using Trex.Server.Infrastructure.Extensions;

namespace Trex.Server.Infrastructure.Implemented.Factories
{
    public class InvoiceFactory : IInvoiceFactory
    {
        #region IInvoiceFactory Members

        public Invoice Create(Customer customer, DateTime startTime, DateTime endTime, DateTime invoiceDate, User createdBy)
        {
            if (customer == null)
            {
                throw new ParameterNullOrEmptyException("Error creating invoice: Customer cannot be null");
            }
            if (createdBy == null)
            {
                throw new ParameterNullOrEmptyException("Error creating invoice: Creator cannot be null");
            }

            var invoice = new Invoice();
            invoice.CreateDate = DateTime.Now;
            invoice.InvoiceDate = invoiceDate;

            if (customer.PaymentTermIncludeCurrentMonth)
            {
                invoice.DueDate = invoice.InvoiceDate.AddDays(customer.PaymentTermNumberOfDays);
            }
            else
            {
                invoice.DueDate = invoice.InvoiceDate.NextFirstOfMonth().AddDays(customer.PaymentTermNumberOfDays);
            }

            invoice.Customer = customer;
            invoice.StartDate = startTime;
            invoice.EndDate = endTime;
            invoice.CreatedBy = createdBy;
            invoice.CustomerName = customer.Name;
            invoice.StreetAddress = customer.StreetAddress;
            invoice.ZipCode = customer.ZipCode;
            invoice.City = customer.City;
            invoice.Country = customer.Country;
            invoice.Attention = customer.ContactName;
            invoice.CustomerNumber = customer.Id;

            return invoice;
        }

        #endregion
    }
}