using System;

namespace Trex.Core.Model
{
    public class Invoice
    {
        public int ID { get; set; }
        public string CustomerName { get; set; }
        public int CustomerNumber { get; set; }
        public int CreatedBy { get; set; }
        public string StreetAddress { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime StartDate { get; set; }
        public bool Closed { get; set; }
        public DateTime EndDate { get; set; }
        public string Attention { get; set; }
        public string FooterText { get; set; }
        public DateTime? DueDate { get; set; }
        public string Regarding { get; set; }
        public string Address2 { get; set; }
        public double TotalExclVAT { get; set; }
        public double InvoicePeriod
        {
            get { return (EndDate - StartDate).TotalDays; }
        }
    }
}