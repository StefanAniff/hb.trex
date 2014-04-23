namespace Trex.Core.Model
{
    public class InvoiceLine
    {
        public int ID { get; set; }
        public double Units { get; set; }
        public string Unit { get; set; }
        public double PricePrUnit { get; set; }
        public int InvoiceId { get; set; }
        public double VatPercentage { get; set; }
        public double VatAmount { get; set; }
        public bool IsExpense { get; set; }
        public double Total { get; set; }
        public string Text { get; set; }
        public int SortIndex { get; set; }
        public double TotalInclVat { get; set; }

        //public enum UnitTypes
        //{
        //    Hours,
        //    Other
        //}

        ///// <summary>
        ///// Gets or sets the Id.
        ///// </summary>
        ///// <value>The Id.</value>
        //public virtual int Id { get; set; }

        //public bool IsValidChild(IEntity entity)
        //{
        //    return false;
        //}

        ///// <summary>
        ///// Gets or sets the invoice.
        ///// </summary>
        ///// <value>The invoice.</value>
        ///// 
        //[XmlIgnore()]
        //public virtual Invoice Invoice { get; set; }

        ///// <summary>
        ///// Gets or sets the text.
        ///// </summary>
        ///// <value>The text.</value>
        //public virtual string Text { get; set; }

        ///// <summary>
        ///// Gets or sets the price.
        ///// </summary>
        ///// <value>The price.</value>
        //public virtual double PricePrUnit { get; set; }

        ///// <summary>
        ///// Gets or sets the units.
        ///// </summary>
        ///// <value>The units.</value>
        //public virtual double Units { get; set; }

        ///// <summary>
        ///// Gets or sets the unit.
        ///// </summary>
        ///// <value>The unit.</value>
        //public virtual string Unit { get; set; }

        ///// <summary>
        ///// Gets or sets the type of the unit.
        ///// </summary>
        ///// <value>The type of the unit.</value>
        //public virtual UnitTypes UnitType { get; set; }

        ///// <summary>
        ///// Gets or sets the sort Index
        ///// </summary>
        ///// <value>The index of the sort.</value>
        //public virtual int SortIndex { get; set; }

        ///// <summary>
        ///// Gets or sets the total.
        ///// </summary>
        ///// <value>The total.</value>
        //public virtual double Total
        //{
        //    get
        //    {
        //        return Units * PricePrUnit;
        //    }

        //}
    }
}