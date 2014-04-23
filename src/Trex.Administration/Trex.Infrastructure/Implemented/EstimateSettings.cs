using Trex.Core.Services;

namespace Trex.Infrastructure.Implemented
{
    public class EstimateSettings : IEstimateSettings
    {
        #region IEstimateSettings Members

        public double PessimisticWeight
        {
            get { return 1; }
        }

        public double OptimisticWeight
        {
            get { return 1; }
        }

        public double RealisticWeight
        {
            get { return 3; }
        }

        public double MinimumThreshold
        {
            get { return 1; }
        }

        public double MaximumThreshold
        {
            get { return 2; }
        }

        #endregion
    }
}