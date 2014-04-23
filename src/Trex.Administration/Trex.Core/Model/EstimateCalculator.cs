using System;
using Trex.Core.Services;

namespace Trex.Core.Model
{
    public class EstimateCalculator : IEstimateCalculator
    {
        private readonly IEstimateSettings _estimateSettings;
        private double _meanValue;
        private double _standardDeviation;
        private double _variance;
        private double _weightedOptimistic;
        private double _weightedRealistic;
        private double _weihgtedPessimistic;

        public EstimateCalculator(IEstimateSettings estimateSettings)
        {
            _estimateSettings = estimateSettings;
        }

        #region IEstimateCalculator Members

        public void Calculate(double pessimisticEstimate, double optimisticEstimate, double realisticEstimate)
        {
            var weightSum = _estimateSettings.PessimisticWeight + _estimateSettings.OptimisticWeight +
                            _estimateSettings.RealisticWeight;

            _weihgtedPessimistic = _estimateSettings.PessimisticWeight*pessimisticEstimate;
            _weightedRealistic = _estimateSettings.RealisticWeight*realisticEstimate;
            _weightedOptimistic = _estimateSettings.OptimisticWeight*optimisticEstimate;

            if (weightSum != 0)
            {
                EstimateCalculated = (_weihgtedPessimistic + _weightedRealistic + _weightedOptimistic)/weightSum;
            }

            _meanValue = (pessimisticEstimate + optimisticEstimate + realisticEstimate)/3;

            _standardDeviation = (_weihgtedPessimistic - _weightedOptimistic)/weightSum;

            _variance = Math.Pow(_standardDeviation, 2);
        }

        public double EstimateCalculated { get; private set; }

        public EstimateStatus Status
        {
            get
            {
                if (_variance <= _estimateSettings.MinimumThreshold)
                {
                    return EstimateStatus.Ok;
                }

                if (_variance > _estimateSettings.MaximumThreshold)
                {
                    return EstimateStatus.MustBeDivided;
                }

                return EstimateStatus.ShouldBeDivided;
            }
        }

        #endregion
    }
}