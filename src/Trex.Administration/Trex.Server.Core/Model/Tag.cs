namespace Trex.Server.Core.Model
{
    public class Tag
    {
        public Tag() {}

        /// <summary>
        /// Initializes a new instance of the <see cref="Tag"/> class.
        /// </summary>
        /// <param name="customer">The customer.</param>
        /// <param name="text">The text.</param>
        public Tag(Customer customer, string text)
        {
            Customer = customer;
            Text = text;
        }

        /// <summary>
        /// Gets or sets the tag ID.
        /// </summary>
        /// <value>The tag ID.</value>
        public virtual int Id { get; set; }

        /// <summary>
        /// Gets or sets the customer.
        /// </summary>
        /// <value>The customer.</value>
        public virtual Customer Customer { get; set; }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        public virtual string Text { get; set; }
    }
}