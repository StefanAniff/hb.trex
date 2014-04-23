namespace Trex.Server.Core.Model
{
    public class Tag
    {
        public Tag()
        {
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
        public virtual Company Company { get; set; }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        public virtual string Text { get; set; }
    }
}