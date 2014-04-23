namespace Trex.Server.Core.Model.Forecast
{
    public class ForecastType : EntityBase
    {
        protected ForecastType() { }

        public ForecastType(string name, string colorStringHex, bool supportsProjectHours, bool supportsDedicatedHours)
        {
            Name = name;
            ColorStringHex = colorStringHex;
            SupportsProjectHours = supportsProjectHours;
            SupportsDedicatedHours = supportsDedicatedHours;
            StatisticsInclusion = true; // default
        }

        public virtual int Id { get; protected set; }

        /// <summary>
        /// Name for this ForecastType
        /// </summary>
        public virtual string Name { get; protected set; }

        /// <summary>
        /// Detailed description for this ForecastType
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Color for this ForecastType in hex (eg. #FFFFF)
        /// </summary>
        public virtual string ColorStringHex { get; protected set; }

        /// <summary>
        /// Indicates if the relating forecast can/may have projectHours
        /// registered on it
        /// </summary>
        public virtual bool SupportsProjectHours { get; protected set; }

        /// <summary>
        /// Indicates if the relating forecast can/may have dedicated ForecastType 
        /// hours registered on it
        /// </summary>
        public virtual bool SupportsDedicatedHours { get; protected set; }

        /// <summary>
        /// Indicates if the relating forecast's dedicatedhours is to be included 
        /// when calculating statistics
        /// </summary>
        public virtual bool StatisticsInclusion { get; set; }

        public override int EntityId
        {
            get { return Id; }
        }

        public override string ToString()
        {
            return string.Format("{0} {1}", Id, Name);
        }
    }
}