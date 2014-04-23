using System.Xml.Serialization;

namespace Trex.Server.Core.Model
{
    public class InvoiceLine
    {
        private const double DEFAULT_VAT = 0.25;

        public InvoiceLine()
        {
            VatPercentage = DEFAULT_VAT;
        }

        public enum UnitTypes
        {
            Hours,
            Other
        }

        /// <summary>
        /// Gets or sets the ID.
        /// </summary>
        /// <value>The ID.</value>
        public virtual int ID { get; set; }

        /// <summary>
        /// Gets or sets the invoice.
        /// </summary>
        /// <value>The invoice.</value>
        /// 
        [XmlIgnore()]
        public virtual Invoice Invoice { get; set; }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        public virtual string Text { get; set; }

        /// <summary>
        /// Gets or sets the price.
        /// </summary>
        /// <value>The price.</value>
        public virtual double PricePrUnit { get; set; }

        /// <summary>
        /// Gets or sets the units.
        /// </summary>
        /// <value>The units.</value>
        public virtual double Units { get; set; }

        /// <summary>
        /// Gets or sets the unit.
        /// </summary>
        /// <value>The unit.</value>
        public virtual string Unit { get; set; }


        /// <summary>
        /// Gets or sets the type of the unit.
        /// </summary>
        /// <value>The type of the unit.</value>
        public virtual UnitTypes UnitType { get; set; }

        /// <summary>
        /// Gets or sets the sort Index
        /// </summary>
        /// <value>The index of the sort.</value>
        public virtual int SortIndex { get; set; }

        /// <summary>
        /// Gets or sets the total.
        /// </summary>
        /// <value>The total.</value>
        public virtual double Total
        {
            get
            {
                return Units * PricePrUnit;
            }
            set { }

        }

        public virtual double VatPercentage { get; set; }

        public virtual double TotalInclVat { get { return Total + VatAmount; } set { } }

        public virtual double VatAmount
        {
            get { return Total * VatPercentage; }
            set { }
        }


        public virtual bool IsExpense { get; set; }

    }
}