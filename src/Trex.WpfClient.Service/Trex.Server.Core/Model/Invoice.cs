using System;
using System.Collections.Generic;
using System.Linq;

namespace Trex.Server.Core.Model
{
    public class Invoice
    {
        public Invoice()
        {
            InvoiceLines = new List<InvoiceLine>();
            VATPercentage = 0.25;
        }

        /// <summary>
        /// Gets or sets the ID.
        /// </summary>
        /// <value>The ID.</value>
        public virtual int ID { get; set; }

        /// <summary>
        /// Gets or sets the create date.
        /// </summary>
        /// <value>The create date.</value>
        public virtual DateTime CreateDate { get; set; }


        /// <summary>
        /// Gets or sets the invoice date.
        /// </summary>
        /// <value>The invoice date.</value>
        public virtual DateTime InvoiceDate { get; set; }

        /// <summary>
        /// Gets or sets the start date.
        /// </summary>
        /// <value>The start date.</value>
        public virtual DateTime StartDate { get; set; }

        /// <summary>
        /// Gets or sets the end date.
        /// </summary>
        /// <value>The end date.</value>
        public virtual DateTime EndDate { get; set; }

        /// <summary>
        /// Gets or sets the due date.
        /// </summary>
        /// <value>The due date.</value>
        public virtual DateTime? DueDate { get; set; }

        /// <summary>
        /// Gets or sets the customer.
        /// </summary>
        /// <value>The customer.</value>
        /// 
        public virtual Company Company { get; set; }

        /// <summary>
        /// Gets or sets the created by.
        /// </summary>
        /// <value>The created by.</value>
        public virtual User CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the attention.
        /// </summary>
        /// <value>The attention.</value>
        public virtual string Attention { get; set; }

        /// <summary>
        /// Gets or sets the name of the customer.
        /// </summary>
        /// <value>The name of the customer.</value>
        public virtual string CustomerName { get; set; }


        /// <summary>
        /// Gets or sets the street address.
        /// </summary>
        /// <value>The street address.</value>
        public virtual string StreetAddress { get; set; }

        /// <summary>
        /// Gets or sets the zip code.
        /// </summary>
        /// <value>The zip code.</value>
        public virtual string ZipCode { get; set; }

        /// <summary>
        /// Gets or sets the city.
        /// </summary>
        /// <value>The city.</value>
        public virtual string City { get; set; }

        /// <summary>
        /// Gets or sets the country.
        /// </summary>
        /// <value>The country.</value>
        public virtual string Country { get; set; }

        

        /// <summary>
        /// Gets or sets the total excl VAT.
        /// </summary>
        /// <value>The total excl VAT.</value>
        public virtual double TotalExclVAT
        {
            get
            {
                return InvoiceLines.Sum(il => il.Total);
            }
        }

        /// <summary>
        /// Gets or sets the VAT.
        /// </summary>
        /// <value>The VAT.</value>
        public virtual double VATPercentage { get; set; }


        /// <summary>
        /// Gets or sets the footer text.
        /// </summary>
        /// <value>The footer text.</value>
        public virtual string FooterText { get; set; }

        /// <summary>
        /// Gets or sets the invoice lines.
        /// </summary>
        /// <value>The invoice lines.</value>
        /// 
        public virtual IList<InvoiceLine> InvoiceLines { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Invoice"/> is closed.
        /// </summary>
        /// <value><c>true</c> if closed; otherwise, <c>false</c>.</value>
        public virtual bool Closed { get; set; }



        /// <summary>
        /// Gets or sets the regarding text
        /// </summary>
        /// <value>The regarding.</value>
        public virtual string Regarding { get; set; }

        public virtual string Address2 { get; set; }
    }
}